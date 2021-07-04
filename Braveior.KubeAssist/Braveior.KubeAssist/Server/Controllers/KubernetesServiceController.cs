using k8s;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Braveior.KubeAssist.Models;
using Nest;
using Elasticsearch.Net;
using System.Text;
using k8s.Models;
using System.IO;
using Braveior.KubeAssist.Server.Services;
using Serilog;

namespace Braveior.KubeAssist.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KubernetesServiceController : ControllerBase
    {

        //private readonly ILogger<WeatherForecastController> _logger;
        readonly KubernetesClient _kClient;
        private readonly ILogger _logger;
        public KubernetesServiceController(KubernetesClient kClient, ILogger logger)
        {
            _kClient = kClient;
            _logger = logger;
        }

        
        [HttpGet("getlatestclustermetric")]
        public async Task<IActionResult> GetLatestClusterMetric()
        {
            var nodeMetrics = await _kClient.client.GetKubernetesNodesMetricsAsync().ConfigureAwait(false);
            ClusterMetric clusterMetric = new ClusterMetric();
            if (nodeMetrics != null && nodeMetrics.Items != null)
            {
                int cpu = 0;
                long memory = 0;

                foreach (var metric in nodeMetrics.Items)
                {

                    //Refactoring needed
                    cpu += Int32.Parse(metric.Usage["cpu"].ToString().Replace("n", "")) / 1000000;
                    memory += Int64.Parse((metric.Usage["memory"].ToString().Replace("Ki", "")))/1024;
                }
                clusterMetric = new ClusterMetric() { Name = "cluster", CPU = cpu, Memory = memory, TimeStamp = DateTime.Now.ToUniversalTime() };
            }
            return Ok(clusterMetric);

       }
        [HttpGet("getnamespacemetric/{ns}/{day}")]
        public async Task<IActionResult> GetNamespaceMetricResult(string ns, string day)
        {
            var settings = new ConnectionConfiguration(new Uri("http://192.168.0.112:9200"))
            .RequestTimeout(TimeSpan.FromMinutes(2));

            //var settings = new ConnectionConfiguration(new Uri(GetEnvironmentVariable("elasticuri")))
            //.RequestTimeout(TimeSpan.FromMinutes(2));

            var lc = new ElasticLowLevelClient(settings);
            var namespaceMetricsQueryTemplate = @"{
        ""query"": {
        ""bool"": {
        ""filter"": [{
            ""term"": { ""name.keyword"": ""#NAMESPACE#"" }
        },
        {
            ""range"": {
                    ""timeStamp"": {
                    ""gte"": ""now-#DAYS#d/d"",
                    ""lte"": ""now/d""
                }
            }
            }
                ]
                }
                },
                ""aggs"": {
                ""sales_over_time"": {
                ""date_histogram"": {
                ""field"": ""timeStamp"",
                ""calendar_interval"": ""hour""
                },
                ""aggs"" : {
                ""avg_cpu"" : {
                ""avg"": {
                ""field"": ""cPU""
                }
                },
                ""avg_ram"" : {
                ""avg"": {
                ""field"": ""memory""
                }
                }
                }
                }
                }
                }";
            var searchResponse = (await lc.SearchAsync<BytesResponse>
            ("namespacemetrics-*", namespaceMetricsQueryTemplate.Replace("#NAMESPACE#", ns).Replace("#DAYS#", day))).Body;
            string utfString = Encoding.UTF8.GetString(searchResponse, 0, searchResponse.Length);
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return Ok(JsonConvert.DeserializeObject<MetricResult>(utfString, jsonSettings));
        }
        [HttpGet("getpodmetricresult/{podname}/{day}")]
        public async Task<IActionResult> GetPodMetricResult(string podname, string day)
        {
            var settings = new ConnectionConfiguration(new Uri("http://192.168.0.112:9200"))
            .RequestTimeout(TimeSpan.FromMinutes(2));
            //  var settings = new ConnectionConfiguration(new Uri(GetEnvironmentVariable("elasticuri")))
            //   .RequestTimeout(TimeSpan.FromMinutes(2));
            var lc = new ElasticLowLevelClient(settings);
            var podMetricsQueryTemplate = @"{
    ""query"": {
    ""bool"": {
    ""filter"": [{
        ""term"": { ""name.keyword"": ""#PODNAME#"" }
    },
    {
        ""range"": {
                ""timeStamp"": {
                ""gte"": ""now-#DAYS#d/d"",
                ""lte"": ""now/d""
            }
        }
        }
            ]
            }
            },
            ""aggs"": {
            ""sales_over_time"": {
            ""date_histogram"": {
            ""field"": ""timeStamp"",
            ""calendar_interval"": ""hour""
            },
            ""aggs"" : {
            ""avg_cpu"" : {
            ""avg"": {
            ""field"": ""cPU""
            }
            },
            ""avg_ram"" : {
            ""avg"": {
            ""field"": ""memory""
            }
            }
            }
            }
            }
            }";

            var searchResponse = (await lc.SearchAsync<BytesResponse>
            ("podmetrics-*", podMetricsQueryTemplate.Replace("#PODNAME#", podname).Replace("#DAYS#", day))).Body;
            string utfString = Encoding.UTF8.GetString(searchResponse, 0, searchResponse.Length);
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return Ok(JsonConvert.DeserializeObject<MetricResult>(utfString, jsonSettings));
        }

        [HttpGet("getpodlogs/{name}/{ns}")]
        public async Task<IActionResult> GetPodLogs(string name, string ns)
        {
            var response = await _kClient.client.ReadNamespacedPodLogWithHttpMessagesAsync(name, ns, follow: false).ConfigureAwait(false);
            var stream = response.Body;
            StreamReader reader = new StreamReader(stream);
            string log = reader.ReadToEnd();
            return Ok(log);
        }
        /// <summary>
        /// Endpoint to get monthly average ratings for member
        /// </summary>
        /// <param name="ratedfor"></param>
        /// <returns></returns>
        [HttpGet("getnodes")]
        public async Task<IActionResult> GetNodes()
        {
            var nodes = await _kClient.client.ListNodeAsync().ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(nodes), "application/json");
        }
        [HttpGet("getpods/{ns}")]
        public async Task<IActionResult> GetPods(string ns)
        {
            var pods = await _kClient.client.ListNamespacedPodAsync(ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(pods), "application/json");
        }
        [HttpGet("getpod/{podname}/{ns}")]
        public async Task<IActionResult> GetPod(string podname, string ns)
        {
            var pod = await _kClient.client.ReadNamespacedPodAsync(podname, ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(pod), "application/json");
        }
        [HttpGet("getnode/{nodename}")]
        public async Task<IActionResult> GetNode(string nodename)
        {
            var node = await _kClient.client.ReadNodeAsync(nodename).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(node), "application/json");
        }
        [HttpGet("getdeployments/{ns}")]
        public async Task<IActionResult> GetDeployments(string ns)
        {
            var deployments = await _kClient.client.ListNamespacedDeploymentAsync(ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(deployments), "application/json");
        }

        [HttpGet("getdeployment/{name}/{ns}")]
        public async Task<IActionResult> GetDeployment(string name, string ns)
        {
            var deployment = await _kClient.client.ReadNamespacedDeploymentAsync(name, ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(deployment), "application/json");
        }
        [HttpGet("getservices/{ns}")]
        public async Task<IActionResult> GetServices(string ns)
        {
            var services = await _kClient.client.ListNamespacedServiceAsync(ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(services), "application/json");
        }
        [HttpGet("getservice/{name}/{ns}")]
        public async Task<IActionResult> GetService(string name, string ns)
        {
            var service = await _kClient.client.ReadNamespacedServiceAsync(name, ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(service), "application/json");
        }
        [HttpGet("getpersistentvolumes/{ns}")]
        public async Task<IActionResult> GetPersistentVolumes(string ns)
        {
            var pvs = await _kClient.client.ListPersistentVolumeAsync().ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(pvs), "application/json");
        }
        [HttpGet("getpersistentvolume/{name}")]
        public async Task<IActionResult> GetPersistentVolume(string name)
        {
            var pv = await _kClient.client.ReadPersistentVolumeAsync(name).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(pv), "application/json");
        }
        [HttpGet("getstatefulsets/{ns}")]
        public async Task<IActionResult> GetStatefulSets(string ns)
        {
            var sfs = await _kClient.client.ListNamespacedStatefulSetAsync(ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(sfs), "application/json");
        }
        [HttpGet("getstatefulset/{name}/{ns}")]
        public async Task<IActionResult> GetStatefulSet(string name, string ns)
        {
            var sf=  await _kClient.client.ReadNamespacedStatefulSetAsync(name, ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(sf), "application/json");
        }
        [HttpGet("getconfigmaps/{ns}")]
        public async Task<IActionResult> GetConfigMaps(string ns)
        {
            var sfs = await _kClient.client.ListNamespacedConfigMapAsync(ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(sfs), "application/json");
        }
        [HttpGet("getconfigmap/{name}/{ns}")]
        public async Task<IActionResult> GetConfigMap(string name, string ns)
        {
            var sf = await _kClient.client.ReadNamespacedConfigMapAsync(name, ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(sf), "application/json");
        }
        [HttpGet("getpersistentvolumeclaims/{ns}")]
        public async Task<IActionResult> GetPersistentVolumeClaims(string ns)
        {
            var pvcs = await _kClient.client.ListNamespacedPersistentVolumeClaimAsync(ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(pvcs), "application/json");
        }
        [HttpGet("getpersistentvolumeclaim/{name}/{ns}")]
        public async Task<IActionResult> GetPersistentVolumeClaim(string name, string ns)
        {
            var pvc = await _kClient.client.ReadNamespacedPersistentVolumeClaimAsync(name, ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(pvc), "application/json");
        }
        [HttpGet("getstorageclasses")]
        public async Task<IActionResult> GetStorageClasses()
        {
            var sc = await _kClient.client.ListStorageClassAsync().ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(sc), "application/json");
        }
        [HttpGet("getnamespaces")]
        public async Task<IActionResult> GetNamespaces()
        {
            var namespaces = await _kClient.client.ListNamespaceAsync().ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(namespaces), "application/json");
        }
        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name.ToLower()) ?? Environment.GetEnvironmentVariable(name.ToUpper());
        }
    }
}
