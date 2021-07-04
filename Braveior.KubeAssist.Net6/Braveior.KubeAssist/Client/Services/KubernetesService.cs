using Braveior.KubeAssist.Services.Models;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Services
{
    public class KubernetesService : IKubernetesService
    {
        private HttpClient _httpClient { get; }

        public KubernetesService(HttpClient httpClient, IConfiguration config)
        {
            //httpClient.BaseAddress = new Uri(config.GetValue<string>("AppSettings:KubeAssistAPIBaseAddress"));
            //httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Blazor Web Assembly");
            _httpClient = httpClient;
        }
        public async Task<ClusterMetric> GetLatestClusterMetric()
        {
            return await GetDataAsync<ClusterMetric>("getlatestclustermetric");
        }
        public async Task<MetricResult> GetNamespaceMetricResult(string ns,string day)
        {
            return await GetDataAsync<MetricResult>($"getnamespacemetric/{ns}/{day}");
            

        }
        public async Task<MetricResult> GetPodMetricResult(string podname, string day)
        {

            return await GetDataAsync<MetricResult>($"getpodmetricresult/{podname}/{day}");
            

        }


        public async Task<string> GetPodLogs(string name, string ns)
        {
            return await GetDataAsync<string>($"getpodlogs/{name}/{ns}");
            
        }

        public async Task<V1PodList> GetPods(string ns)
        {
            return await GetDataAsync<V1PodList>($"getpods/{ns}");
            
        }

        public async Task<V1Pod> GetPod(string podname, string ns)
        {
            return await GetDataAsync<V1Pod>($"getpod/{podname}/{ns}");
            
        }
        public async Task<V1DeploymentList> GetDeployments(string ns)
        {
            return await GetDataAsync<V1DeploymentList>($"getdeployments/{ns}");
            
        }
        public async Task<V1NodeList> GetNodes()
        {
            return await GetDataAsync<V1NodeList>($"getnodes");
            

        }
        public async Task<V1Deployment> GetDeployment(string name, string ns)
        {
            return await GetDataAsync<V1Deployment>($"getdeployment/{name}/{ns}");
            

        }

        public async Task<V1ServiceList> GetServices(string ns)
        {
            return await GetDataAsync<V1ServiceList>($"getservices/{ns}");
            
        }

        public async Task<V1Service> GetService(string name, string ns)
        {
            return await GetDataAsync<V1Service>($"getservice/{name}/{ns}");
            
        }

        public async Task<V1PersistentVolumeList> GetPersistentVolumes(string ns)
        {
            return await GetDataAsync<V1PersistentVolumeList>($"getpersistentvolumes/{ns}");
            
        }

        public async Task<V1PersistentVolume> GetPersistentVolume(string name)
        {
            return await GetDataAsync<V1PersistentVolume>($"getpersistentvolume/{name}");

        }
        public async Task<V1StatefulSetList> GetStatefulSets(string ns)
        {
            return await GetDataAsync<V1StatefulSetList>($"getstatefulsets/{ns}");

        }

        public async Task<V1StatefulSet> GetStatefulSet(string name, string ns)
        {
            return await GetDataAsync<V1StatefulSet>($"getstatefulset/{name}/{ns}");

        }

        public async Task<V1PersistentVolumeClaimList> GetPersistentVolumeClaims(string ns)
        {
            return await GetDataAsync<V1PersistentVolumeClaimList>($"getpersistentvolumeclaims/{ns}");

        }

        //public async Task<V1PersistentVolumeClaim> GetPersistentVolumeClaim(string name, string ns)
        //{
        //    ////var config = KubernetesClientConfiguration.InClusterConfig();
        //    //var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
        //    //IKubernetes client = new Kubernetes(config);
        //    //return await client.ReadNamespacedPersistentVolumeClaimAsync(name,ns).ConfigureAwait(false);
        //    return null;
        //}

        private async Task<T> GetDataAsync<T>(string method)
        {
           
            var response = await _httpClient.GetAsync($"api/KubernetesService/{method}");
            if (response.IsSuccessStatusCode)
            {
                //Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                //return await response.Content.ReadFromJsonAsync<V1NodeList>();
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Invalid Access Token");
            }
            else
            {
                throw new Exception("Internal Server Error");
            }
        }
        public async Task<V1StorageClassList> GetStorageClasses()
        {

             return await GetDataAsync<V1StorageClassList>($"getstorageclasses");

        }
        public async Task<V1NamespaceList> GetNamespaces()
        {
            Console.WriteLine("baseaddress - " + _httpClient.BaseAddress.ToString());
            return await GetDataAsync<V1NamespaceList>($"getnamespaces");

        }
        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name.ToLower()) ?? Environment.GetEnvironmentVariable(name.ToUpper());
        }
    }
}
