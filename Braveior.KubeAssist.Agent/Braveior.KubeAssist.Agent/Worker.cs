using Braveior.KubeAssist.Agent.Models;
using Elasticsearch.Net;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Hosting;
using Nest;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Agent
{
    public class Worker : BackgroundService
    {
        private List<PodMetric> PodMetrics { get; set; } = new List<PodMetric>();

        public Worker()
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var logger = new LoggerConfiguration()
           .WriteTo.Console()
           .CreateLogger();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var kclient = new Kubernetes(config);
            var settings = new ConnectionSettings(new Uri("http://192.168.0.112:9200/"));
            //var settings = new ConnectionSettings(new Uri(GetEnvironmentVariable("elasticuri")));
            var client = new ElasticClient(settings);
            while (!stoppingToken.IsCancellationRequested)
            {
                
                await GenerateMetrics(logger,client,kclient);
                logger.Information("Message posted - " + DateTime.Now.ToUniversalTime());
                await Task.Delay(1000, stoppingToken);
            }
        }
        private async Task GenerateMetrics(ILogger logger, ElasticClient client, Kubernetes kclient)
        {
            HttpClient webClient = new HttpClient();
            var response = await webClient.GetStringAsync("http://localhost:64945/metrics");
            //var response = await webClient.GetStringAsync(GetEnvironmentVariable("metricsuri"));
            string[] metrics = response.Split("\n",StringSplitOptions.RemoveEmptyEntries);
            var kubeState = GetKubeStateMetrics(logger, metrics);
            await PostKubeStateMetrics(client, kubeState);
            List<PodMetric> podMetricList = await GeneratePodMetrics(kubeState, kclient);
            await PostPodMetrics(client, podMetricList);
            List<NamespaceMetric> nsMetricList =  GenerateNamespaceMetrics(podMetricList,kubeState);
            await PostNamespaceMetrics(client, nsMetricList);
            var clusterMetric = await GenerateClusterMetrics(kubeState,kclient);
            await PostClusterMetrics(client,clusterMetric);

        }
        private KubeState GetKubeStateMetrics(ILogger logger, string[] metrics)
        {
            KubeState kubeState = new KubeState(metrics, logger);
            kubeState.KubeStateElements = null;
            kubeState.TimeStamp = DateTime.Now;
            return kubeState;
        }
        private async Task PostKubeStateMetrics(ElasticClient client, KubeState kubeState)
        {
            var response = await client.IndexAsync( kubeState, idx => idx.Index("kubestate-" + String.Format("{0:yyyy.MM.dd}", DateTime.Now.ToUniversalTime())) );
        }
        private async Task PostPodMetrics(ElasticClient client, List<PodMetric> podMetricList)
        {
            foreach(var metric in podMetricList)
                await client.IndexAsync(metric, idx => idx.Index("podmetrics-" + String.Format("{0:yyyy.MM.dd}", DateTime.Now.ToUniversalTime())));

        }
        private async Task PostClusterMetrics(ElasticClient client, ClusterMetric clusterMetric)
        {
                await client.IndexAsync(clusterMetric, idx => idx.Index("clustermetrics-" + String.Format("{0:yyyy.MM.dd}", DateTime.Now.ToUniversalTime())));
        }
        private async Task<List<PodMetric>> GeneratePodMetrics(KubeState kubeState, Kubernetes client)
        {
            
            var podMetrics = await client.GetKubernetesPodsMetricsAsync().ConfigureAwait(false);
            List<PodMetric> podMetricList = new List<PodMetric>(); 
            if (podMetrics != null && podMetrics.Items != null)
            {
                foreach (var metric in podMetrics.Items)
                {
                    var podMetric = new PodMetric() { Name = metric.Metadata.Name,Namespace = metric.Metadata.NamespaceProperty };
                    int cpu = 0;
                    long memory = 0;
                    foreach(var container in metric.Containers)
                    {
                        //Refactoring needed
                        cpu += Int32.Parse(container.Usage["cpu"].ToString().Replace("m","")); 
                        memory += Int32.Parse((container.Usage["memory"].ToString().Replace("Ki", "")));
                    }
                    podMetric.CPU = cpu;
                    podMetric.Memory = memory;
                    podMetric.TimeStamp = DateTime.Now;
                    podMetricList.Add(podMetric);
                }
            }
            return podMetricList;
        }

        private async Task<ClusterMetric> GenerateClusterMetrics(KubeState kubeState, Kubernetes client)
        {

            var nodeMetrics = await client.GetKubernetesNodesMetricsAsync().ConfigureAwait(false);
            ClusterMetric clusterMetric = new ClusterMetric();
            if (nodeMetrics != null && nodeMetrics.Items != null)
            {
                int cpu = 0;
                long memory = 0;
                
                foreach (var metric in nodeMetrics.Items)
                {

                    //Refactoring needed
                    cpu += Int32.Parse(metric.Usage["cpu"].ToString().Replace("m", ""));
                    memory += Int32.Parse((metric.Usage["memory"].ToString().Replace("Ki", "")));
                }
                clusterMetric = new ClusterMetric() { Name = "cluster", CPU = cpu, Memory = memory, TimeStamp = DateTime.Now.ToUniversalTime() };
            }
            return clusterMetric;
        }
        private List<NamespaceMetric> GenerateNamespaceMetrics(List<PodMetric> podMetrics, KubeState kubeState)
        {

            List<NamespaceMetric> nsMetricList = new List<NamespaceMetric>();
            foreach (var ns in kubeState.Namespaces)
            {
                //if(ns.Name.Equals(""))
                var nsPods = podMetrics.Where(a => a.Namespace.Equals(ns.Name));
                int cpu = 0;
                long memory = 0;
                foreach (var pod in nsPods)
                {
                    cpu += pod.CPU;
                    memory += pod.Memory;
                }
                if(nsPods.Count()>0)
                    nsMetricList.Add(new NamespaceMetric() { Name =ns.Name, CPU = cpu, Memory = memory, TimeStamp = DateTime.Now.ToUniversalTime() });
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
