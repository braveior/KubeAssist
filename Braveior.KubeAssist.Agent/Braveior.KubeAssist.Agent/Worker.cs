using Braveior.KubeAssist.Agent.Models;
using Elasticsearch.Net;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            while (!stoppingToken.IsCancellationRequested)
            {
                await GenerateMetrics(logger);
                
                await Task.Delay(1000, stoppingToken);
            }
        }
        private async Task GenerateMetrics(ILogger logger)
        {
            var settings = new ConnectionConfiguration(new Uri("http://192.168.0.112:9200/"))
            .RequestTimeout(TimeSpan.FromMinutes(2));
            var client = new ElasticLowLevelClient(settings);
            var kubeState = GetKubeStateMetrics(logger);
            await PostKubeStateMetrics(client, kubeState);
            List<PodMetric> podMetricList = await GeneratePodMetrics(kubeState);
            await PostPodMetrics(client,podMetricList);
            //List<NamespaceMetric> nsMetricList 

        }
        private KubeState GetKubeStateMetrics(ILogger logger)
        {
            var metrics = File.ReadAllLines("metrics.txt");
            KubeState kubeState = new KubeState(metrics, logger);
            kubeState.TimeStamp = DateTime.Now;
            return kubeState;
        }
        private async Task PostKubeStateMetrics(ElasticLowLevelClient client, KubeState kubeState)
        {
            var response = await client.IndexAsync<StringResponse>("kubestate-" + String.Format("{0:yyyy.MM.dd}", DateTime.Now), PostData.Serializable(kubeState));
        }
        private async Task PostPodMetrics(ElasticLowLevelClient client, List<PodMetric> podMetricList)
        {
            foreach(var metric in podMetricList)
                await client.IndexAsync<StringResponse>("podmetrics-" + String.Format("{0:yyyy.MM.dd}", DateTime.Now), PostData.Serializable(metric));
        }
        private async Task<List<PodMetric>> GeneratePodMetrics(KubeState kubeState)
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            var client = new Kubernetes(config);
            var podMetrics = await client.GetKubernetesPodsMetricsAsync();
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
                        cpu = Int32.Parse(container.Usage["cpu"].ToString().Replace("m","")); 
                        memory += container.Usage["memory"].ToInt64();
                    }
                    podMetric.CPU = cpu;
                    podMetric.Memory = memory;
                    podMetric.TimeStamp = DateTime.Now;
                    podMetricList.Add(podMetric);
                }
            }
            return podMetricList;
        }
        //private async Task<List<NamespaceMetric>> GenerateNamespaceMetrics(List<PodMetric> podMetrics,KubeState kubeState)
        //{ 
        //    foreach(var ns in kubeState.Namespaces)
        //    {

        //    }
        //}
    }
}
