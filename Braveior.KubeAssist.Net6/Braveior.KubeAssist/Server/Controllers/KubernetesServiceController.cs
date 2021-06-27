using Braveior.KubeAssist.Shared;
using k8s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

namespace Braveior.KubeAssist.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KubernetesServiceController : ControllerBase
    {
        
        //private readonly ILogger<WeatherForecastController> _logger;

        public KubernetesServiceController()
        {
            
        }

        
        [HttpGet("getlatestclustermetric")]
        public async Task<IActionResult> GetLatestClusterMetric()
        {
            var settings = new ConnectionSettings(new Uri("http://192.168.0.112:9200/"))
          .DefaultIndex("clustermetrics-*");

            // var settings = new ConnectionSettings(new Uri(GetEnvironmentVariable("elasticuri")))
            //.DefaultIndex("clustermetrics-*");


            var client = new ElasticClient(settings);
            // var asyncIndexResponse = await client.IndexDocumentAsync(kubeState);

            var searchResponse = await client.SearchAsync<ClusterMetric>(s => s
            .From(0)
            .Size(1)
            .Query(q => q.MatchAll())
            .Sort(s => s.Descending(a => a.TimeStamp)));

            return Content(JsonConvert.SerializeObject(searchResponse.Documents.FirstOrDefault()), "application/json");
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
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var response = await client.ReadNamespacedPodLogWithHttpMessagesAsync(name, ns, follow: false).ConfigureAwait(false);
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
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var nodes = await client.ListNodeAsync().ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(nodes), "application/json");
        }
        [HttpGet("getpods/{ns}")]
        public async Task<IActionResult> GetPods(string ns)
        {
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var pods = await client.ListNamespacedPodAsync(ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(pods), "application/json");
        }
        [HttpGet("getpod/{podname}/{ns}")]
        public async Task<IActionResult> GetPod(string podname, string ns)
        {
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var pod = await client.ReadNamespacedPodAsync(podname, ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(pod), "application/json");
        }
        [HttpGet("getdeployments/{ns}")]
        public async Task<IActionResult> GetDeployments(string ns)
        {
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var deployments = await client.ListNamespacedDeploymentAsync(ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(deployments), "application/json");
        }

        //public async Task<V1NodeList> GetNodes()
        //{
        //    //var config = KubernetesClientConfiguration.InClusterConfig();
        //    var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
        //    IKubernetes client = new Kubernetes(config);
        //    return await client.ListNodeAsync().ConfigureAwait(false);
        //}
        [HttpGet("getdeployment/{name}/{ns}")]
        public async Task<IActionResult> GetDeployment(string name, string ns)
        {
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var deployment = await client.ReadNamespacedDeploymentAsync(name, ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(deployment), "application/json");
        }
        [HttpGet("getservices/{ns}")]
        public async Task<IActionResult> GetServices(string ns)
        {
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var services = await client.ListNamespacedServiceAsync(ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(services), "application/json");
        }
        [HttpGet("getservice/{name}/{ns}")]
        public async Task<IActionResult> GetService(string name, string ns)
        {
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var service = await client.ReadNamespacedServiceAsync(name, ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(service), "application/json");
        }
        [HttpGet("getpersistentvolumes/{ns}")]
        public async Task<IActionResult> GetPersistentVolumes(string ns)
        {
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var pvs = await client.ListPersistentVolumeAsync().ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(pvs), "application/json");
        }
        [HttpGet("getpersistentvolume/{name}")]
        public async Task<IActionResult> GetPersistentVolume(string name)
        {
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var pv = await client.ReadPersistentVolumeAsync(name).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(pv), "application/json");
        }
        [HttpGet("getstatefulsets/{ns}")]
        public async Task<IActionResult> GetStatefulSets(string ns)
        {
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var sfs = await client.ListNamespacedStatefulSetAsync(ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(sfs), "application/json");
        }
        [HttpGet("getstatefulset/{name}/{ns}")]
        public async Task<IActionResult> GetStatefulSet(string name, string ns)
        {
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var sf=  await client.ReadNamespacedStatefulSetAsync(name, ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(sf), "application/json");
        }
        [HttpGet("getpersistentvolumeclaims/{ns}")]
        public async Task<IActionResult> GetPersistentVolumeClaims(string ns)
        {
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var pvcs = await client.ListNamespacedPersistentVolumeClaimAsync(ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(pvcs), "application/json");
        }
        [HttpGet("getpersistentvolumeclaim/{name}/{ns}")]
        public async Task<IActionResult> GetPersistentVolumeClaim(string name, string ns)
        {
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var pvc = await client.ReadNamespacedPersistentVolumeClaimAsync(name, ns).ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(pvc), "application/json");
        }
        [HttpGet("getstorageclasses")]
        public async Task<IActionResult> GetStorageClasses()
        {
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var sc = await client.ListStorageClassAsync().ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(sc), "application/json");
        }
        [HttpGet("getnamespaces")]
        public async Task<IActionResult> GetNamespaces()
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var namespaces = await client.ListNamespaceAsync().ConfigureAwait(false);
            return Content(JsonConvert.SerializeObject(namespaces), "application/json");
        }
        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name.ToLower()) ?? Environment.GetEnvironmentVariable(name.ToUpper());
        }
    }
}
