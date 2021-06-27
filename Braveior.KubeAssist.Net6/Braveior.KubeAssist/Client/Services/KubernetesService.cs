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
            httpClient.BaseAddress = new Uri(config.GetValue<string>("AppSettings:KubeAssistAPIBaseAddress"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Blazor Web Assembly");
            _httpClient = httpClient;
        }
        public async Task<ClusterMetric> GetLatestClusterMetric()
        {

            var response = await _httpClient.GetAsync($"KubernetesService/getlatestclustermetric");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<ClusterMetric>(await response.Content.ReadAsStringAsync());
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
        public async Task<MetricResult> GetNamespaceMetricResult(string ns,string day)
        {
            var response = await _httpClient.GetAsync($"KubernetesService/getnamespacemetric/{ns}/{day}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<MetricResult>(await response.Content.ReadAsStringAsync());
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
        public async Task<MetricResult> GetPodMetricResult(string podname, string day)
        {

            var response = await _httpClient.GetAsync($"KubernetesService/getpodmetricresult/{podname}/{day}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<MetricResult>(await response.Content.ReadAsStringAsync());
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


        public async Task<string> GetPodLogs(string name, string ns)
        {
            var response = await _httpClient.GetAsync($"KubernetesService/getpodlogs/{name}/{ns}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
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

        public async Task<V1PodList> GetPods(string ns)
        {
            var response = await _httpClient.GetAsync($"KubernetesService/getpods/{ns}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<V1PodList>(await response.Content.ReadAsStringAsync());
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

        public async Task<V1Pod> GetPod(string podname, string ns)
        {
            var response = await _httpClient.GetAsync($"KubernetesService/getpod/{podname}/{ns}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<V1Pod>(await response.Content.ReadAsStringAsync());
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
        public async Task<V1DeploymentList> GetDeployments(string ns)
        {

            var response = await _httpClient.GetAsync($"KubernetesService/getdeployments/{ns}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<V1DeploymentList>(await response.Content.ReadAsStringAsync());
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
        public async Task<V1NodeList> GetNodes()
        {
            //REST API call for Search
            var response = await _httpClient.GetAsync($"KubernetesService/getnodes");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<V1NodeList>(await response.Content.ReadAsStringAsync());
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
        public async Task<V1Deployment> GetDeployment(string name, string ns)
        {
            var response = await _httpClient.GetAsync($"KubernetesService/getdeployment/{name}/{ns}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<V1Deployment>(await response.Content.ReadAsStringAsync());
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

        public async Task<V1ServiceList> GetServices(string ns)
        {
            var response = await _httpClient.GetAsync($"KubernetesService/getservices/{ns}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<V1ServiceList>(await response.Content.ReadAsStringAsync());
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

        public async Task<V1Service> GetService(string name, string ns)
        {
            var response = await _httpClient.GetAsync($"KubernetesService/getservice/{name}/{ns}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<V1Service>(await response.Content.ReadAsStringAsync());
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

        public async Task<V1PersistentVolumeList> GetPersistentVolumes(string ns)
        {
            var response = await _httpClient.GetAsync($"KubernetesService/getpersistentvolumes/{ns}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<V1PersistentVolumeList>(await response.Content.ReadAsStringAsync());
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

        public async Task<V1PersistentVolume> GetPersistentVolume(string name)
        {
            var response = await _httpClient.GetAsync($"KubernetesService/getpersistentvolume/{name}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<V1PersistentVolume>(await response.Content.ReadAsStringAsync());
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
        public async Task<V1StatefulSetList> GetStatefulSets(string ns)
        {
            var response = await _httpClient.GetAsync($"KubernetesService/getstatefulsets/{ns}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<V1StatefulSetList>(await response.Content.ReadAsStringAsync());
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

        public async Task<V1StatefulSet> GetStatefulSet(string name, string ns)
        {
            var response = await _httpClient.GetAsync($"KubernetesService/getstatefulset/{name}/{ns}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<V1StatefulSet>(await response.Content.ReadAsStringAsync());
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

        public async Task<V1PersistentVolumeClaimList> GetPersistentVolumeClaims(string ns)
        {
            var response = await _httpClient.GetAsync($"KubernetesService/getpersistentvolumeclaims/{ns}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<V1PersistentVolumeClaimList>(await response.Content.ReadAsStringAsync());
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

        public async Task<V1PersistentVolumeClaim> GetPersistentVolumeClaim(string name, string ns)
        {
            ////var config = KubernetesClientConfiguration.InClusterConfig();
            //var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            //IKubernetes client = new Kubernetes(config);
            //return await client.ReadNamespacedPersistentVolumeClaimAsync(name,ns).ConfigureAwait(false);
            return null;
        }
        public async Task<V1StorageClassList> GetStorageClasses()
        {
            var response = await _httpClient.GetAsync($"KubernetesService/getstorageclasses");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<V1StorageClassList>(await response.Content.ReadAsStringAsync());
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
        public async Task<V1NamespaceList> GetNamespaces()
        {
            var response = await _httpClient.GetAsync($"KubernetesService/getnamespaces");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return JsonConvert.DeserializeObject<V1NamespaceList>(await response.Content.ReadAsStringAsync());
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
        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name.ToLower()) ?? Environment.GetEnvironmentVariable(name.ToUpper());
        }
    }
}
