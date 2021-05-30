using Braveior.KubeAssist.Services.Models;
using Elasticsearch.Net;
using k8s;
using k8s.Models;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Services
{
    public class KubernetesService
    {
        public KubeState GetKubeState()
        {
            //var settings = new ConnectionSettings(new Uri(GetEnvironmentVariable("elasticuri")))
           //.DefaultIndex("kubestate-*");

            var settings = new ConnectionSettings(new Uri("http://192.168.0.112:9200/"))
          .DefaultIndex("kubestate-*");


            var client = new ElasticClient(settings);

            var searchResponse = client.Search<KubeState>(s => s
             .From(0)
             .Size(1)
             .Query(q => q.MatchAll())
             .Sort(s => s.Descending(a => a.TimeStamp)));

            return searchResponse.Documents.FirstOrDefault();

        }

        public ClusterMetric GetLatestClusterMetric()
        {
            var settings = new ConnectionSettings(new Uri("http://192.168.0.112:9200/"))
          .DefaultIndex("clustermetrics-*");

          // var settings = new ConnectionSettings(new Uri(GetEnvironmentVariable("elasticuri")))
          //.DefaultIndex("clustermetrics-*");


            var client = new ElasticClient(settings);
            // var asyncIndexResponse = await client.IndexDocumentAsync(kubeState);

            var searchResponse = client.Search<ClusterMetric>(s => s
            .From(0)
            .Size(1)
            .Query(q => q.MatchAll())
            .Sort(s => s.Descending(a => a.TimeStamp)));

            return searchResponse.Documents.FirstOrDefault();
        }
        public MetricResult GetNamespaceMetricResult(string ns,string day)
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
            var searchResponse = lc.Search<BytesResponse>
            ("namespacemetrics-*", namespaceMetricsQueryTemplate.Replace("#NAMESPACE#", ns).Replace("#DAYS#", day)).Body;
            string utfString = Encoding.UTF8.GetString(searchResponse, 0, searchResponse.Length);
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<MetricResult>(utfString, jsonSettings);
        }
        public MetricResult GetPodMetricResult(string podName, string day)
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

            var searchResponse = lc.Search<BytesResponse>
            ("podmetrics-*", podMetricsQueryTemplate.Replace("#PODNAME#", podName).Replace("#DAYS#", day)).Body;
            string utfString = Encoding.UTF8.GetString(searchResponse, 0, searchResponse.Length);
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return  JsonConvert.DeserializeObject<MetricResult>(utfString, jsonSettings);
        }


        public async Task<string> GetPodLogs(string name, string ns)
        {
            //var config = KubernetesClientConfiguration.InClusterConfig();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            var response = await client.ReadNamespacedPodLogWithHttpMessagesAsync(name, ns, follow: false).ConfigureAwait(false);
            var stream = response.Body;
            StreamReader reader = new StreamReader(stream);
            string log = reader.ReadToEnd();
            return log;
        }
        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name.ToLower()) ?? Environment.GetEnvironmentVariable(name.ToUpper());
        }
    }
}
