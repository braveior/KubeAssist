using Braveior.KubeAssist.Services.Models;
using k8s;
using k8s.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Services
{
    public interface IKubernetesService
    {
        Task<V1NodeList> GetNodes();
        Task<V1DeploymentList> GetDeployments(string ns);

        Task<ClusterMetric> GetLatestClusterMetric();

        Task<MetricResult> GetNamespaceMetricResult(string ns, string day);

        Task<MetricResult> GetPodMetricResult(string podname, string day);

        Task<V1PodList> GetPods(string ns);

        Task<V1Pod> GetPod(string podname, string ns);

        Task<string> GetPodLogs(string name, string ns);

        Task<V1Deployment> GetDeployment(string name, string ns);

        Task<V1PersistentVolume> GetPersistentVolume(string name);

        Task<V1PersistentVolumeList> GetPersistentVolumes(string ns);

        Task<V1PersistentVolumeClaimList> GetPersistentVolumeClaims(string ns);

        Task<V1Service> GetService(string name, string ns);

        Task<V1ServiceList> GetServices(string ns);

        Task<V1StatefulSetList> GetStatefulSets(string ns);

        Task<V1StatefulSet> GetStatefulSet(string name, string ns);

        Task<V1StorageClassList> GetStorageClasses();

        Task<V1NamespaceList> GetNamespaces();


    }
}
