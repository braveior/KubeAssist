using Braveior.KubeAssist.Services.Models;
using k8s;
using Serilog;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Braveior.KubeAssist.Server.Services;

namespace Braveior.KubeAssist.Services
{
    public class Agent : BackgroundService
    {
        private readonly ILogger _logger;
        readonly KubernetesClient _kClient;
        readonly ElasticSearchClient _eClient;



        public Agent(KubernetesClient kClient, ElasticSearchClient eClient, ILogger logger)
        {
            _kClient = kClient;
            _logger = logger;
            _eClient = eClient;
            // Constructor's parameters validations...
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
                while (!stoppingToken.IsCancellationRequested)
                {

                    try
                    {
                        await GenerateMetrics(_eClient, _kClient);
                        _logger.Information("Heartbeat - Agent Sent metrics to Elastic Search");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Error sending Agent Metrics - {ex.Message} ");
                    }
                    await Task.Delay(5000, stoppingToken);
                }
        }
        private async Task GenerateMetrics(ElasticSearchClient eClient, KubernetesClient kclient)
        {
            List<PodMetric> podMetricList = await GeneratePodMetrics(kclient);
            await PostPodMetrics(eClient.client, podMetricList);
            List<NamespaceMetric> nsMetricList = await GenerateNamespaceMetrics(kclient, podMetricList);
            await PostNamespaceMetrics(eClient.client, nsMetricList);
            var clusterMetric = await GenerateClusterMetrics(kclient);
            await PostClusterMetrics(eClient.client, clusterMetric);

        }
        private async Task PostPodMetrics(ElasticClient client, List<PodMetric> podMetricList)
        {
            foreach (var metric in podMetricList)
                await client.IndexAsync(metric, idx => idx.Index("podmetrics-" + String.Format("{0:yyyy.MM.dd}", DateTime.Now.ToUniversalTime())));

        }
        private async Task PostClusterMetrics(ElasticClient client, ClusterMetric clusterMetric)
        {
            await client.IndexAsync(clusterMetric, idx => idx.Index("clustermetrics-" + String.Format("{0:yyyy.MM.dd}", DateTime.Now.ToUniversalTime())));
        }
        private async Task<List<PodMetric>> GeneratePodMetrics(KubernetesClient kClient)
        {

            var podMetrics = await kClient.client.GetKubernetesPodsMetricsAsync().ConfigureAwait(false);
            List<PodMetric> podMetricList = new List<PodMetric>();
            if (podMetrics != null && podMetrics.Items != null)
            {
                foreach (var metric in podMetrics.Items)
                {
                    var podMetric = new PodMetric() { Name = metric.Metadata.Name, Namespace = metric.Metadata.NamespaceProperty };
                    long cpu = 0;
                    long memory = 0;
                    foreach (var container in metric.Containers)
                    {
                        //Refactoring needed
                        cpu += Int64.Parse(container.Usage["cpu"].ToString().Replace("n", ""))/ 1000000;
                        memory += Int64.Parse((container.Usage["memory"].ToString().Replace("Ki", "")))/1024;
                    }
                    podMetric.CPU = cpu;
                    podMetric.Memory = memory;
                    podMetric.TimeStamp = DateTime.Now;
                    podMetricList.Add(podMetric);
                }
            }
            return podMetricList;
        }

        private async Task<ClusterMetric> GenerateClusterMetrics(KubernetesClient kClient)
        {

            var nodeMetrics = await kClient.client.GetKubernetesNodesMetricsAsync().ConfigureAwait(false);
            ClusterMetric clusterMetric = new ClusterMetric();
            if (nodeMetrics != null && nodeMetrics.Items != null)
            {
                long cpu = 0;
                long memory = 0;

                foreach (var metric in nodeMetrics.Items)
                {

                    //Refactoring needed
                    cpu += Int64.Parse(metric.Usage["cpu"].ToString().Replace("n", "")) / 1000000;
                    memory += Int64.Parse((metric.Usage["memory"].ToString().Replace("Ki", "")))/1024;
                }
                clusterMetric = new ClusterMetric() { Name = "cluster", CPU = cpu, Memory = memory, TimeStamp = DateTime.Now.ToUniversalTime() };
            }
            return clusterMetric;
        }
        private async Task<List<NamespaceMetric>> GenerateNamespaceMetrics(KubernetesClient kClient, List<PodMetric> podMetrics)
        {

            List<NamespaceMetric> nsMetricList = new List<NamespaceMetric>();
            var namespaces = await kClient.client.ListNamespaceAsync().ConfigureAwait(false);
            foreach (var ns in namespaces.Items)
            {
                //if(ns.Name.Equals(""))
                var nsPods = podMetrics.Where(a => a.Namespace.Equals(ns.Metadata.Name));
                long cpu = 0;
                long memory = 0;
                foreach (var pod in nsPods)
                {
                    cpu += pod.CPU;
                    memory += pod.Memory;
                }
                if (nsPods.Count() > 0)
                    nsMetricList.Add(new NamespaceMetric() { Name = ns.Metadata.Name, CPU = cpu, Memory = memory, TimeStamp = DateTime.Now.ToUniversalTime() });
            }
            return nsMetricList;
        }
        private async Task PostNamespaceMetrics(ElasticClient client, List<NamespaceMetric> nsMetricList)
        {
            foreach (var metric in nsMetricList)
                await client.IndexAsync(metric, idx => idx.Index("namespacemetrics-" + String.Format("{0:yyyy.MM.dd}", DateTime.Now.ToUniversalTime())));
        }
        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name.ToLower()) ?? Environment.GetEnvironmentVariable(name.ToUpper());
        }

    }
}
