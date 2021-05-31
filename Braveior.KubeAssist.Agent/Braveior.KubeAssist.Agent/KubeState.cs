using ByteSizeLib;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Braveior.KubeAssist.Agent.Models
{
    public class KubeState
    {
        private ILogger Logger { get; set; } 
        public KubeState(string[] metrics,ILogger logger)
        {
            Logger = logger;
            //Parse the metrics into KubeStateElements List
            ParseMetrics(metrics);

            AddNodeDetails();

            AddNoteStatusConditions();

            AddNodeRoles();

            AddNodeStatusCapacity();

            AddNamespaces();

            AddDeployments();

            AddPodDetails();

            AddServices();

            AddPersistentStorages();

            AddPersistentStorageClaims();

            AddStorageClasses();

            AddStatefulSets();
        }

        private void AddStatefulSets()
        {
            var statefulsets = KubeStateElements.Where(a => a.Name == "kube_statefulset_created");
            foreach (var statefulset in statefulsets)
            {
                try
                {
                    StatefulSets.Add(new StatefulSet()
                    {

                        Name = statefulset.keyValues["statefulset"],
                        Namespace = statefulset.keyValues["namespace"],
                        NoOfReplicas = KubeStateElements.Where(a => a.Name == "kube_statefulset_status_replicas" && a.keyValues["namespace"] == statefulset.keyValues["namespace"] && a.keyValues["statefulset"] == statefulset.keyValues["statefulset"]).FirstOrDefault().Value,
                        CurrentReplicas = KubeStateElements.Where(a => a.Name == "kube_statefulset_status_replicas_current" && a.keyValues["namespace"] == statefulset.keyValues["namespace"] && a.keyValues["statefulset"] == statefulset.keyValues["statefulset"]).FirstOrDefault().Value,
                        ReadyReplicas = KubeStateElements.Where(a => a.Name == "kube_statefulset_status_replicas_ready" && a.keyValues["namespace"] == statefulset.keyValues["namespace"] && a.keyValues["statefulset"] == statefulset.keyValues["statefulset"]).FirstOrDefault().Value,
                        UpdatedReplicas = KubeStateElements.Where(a => a.Name == "kube_statefulset_status_replicas_updated" && a.keyValues["namespace"] == statefulset.keyValues["namespace"] && a.keyValues["statefulset"] == statefulset.keyValues["statefulset"]).FirstOrDefault().Value,
                        NoOfDesiredPods = KubeStateElements.Where(a => a.Name == "kube_statefulset_replicas" && a.keyValues["namespace"] == statefulset.keyValues["namespace"] && a.keyValues["statefulset"] == statefulset.keyValues["statefulset"]).FirstOrDefault().Value,
                        CreatedDate = GetDateTime(statefulset.Value),
                        Labels = GetLabels(KubeStateElements.Where(a => a.Name == "kube_statefulset_labels" && a.keyValues["namespace"] == statefulset.keyValues["namespace"] && a.keyValues["statefulset"] == statefulset.keyValues["statefulset"] && a.Value == "1").FirstOrDefault().keyValues)

                    });
                }
                catch (Exception ex)
                {
                    Logger.Error("Error adding deployments  - " + ex.Message);
                }

            }
        }
        private void AddDeployments()
        {
            var deployments = KubeStateElements.Where(a => a.Name == "kube_deployment_created");
            foreach (var deployment in deployments)
            {
                try
                {
                    Deployments.Add(new Deployment()
                    {

                        Name = deployment.keyValues["deployment"],
                        Namespace = deployment.keyValues["namespace"],
                        TotalReplicas = KubeStateElements.Where(a => a.Name == "kube_deployment_status_replicas" && a.keyValues["namespace"] == deployment.keyValues["namespace"] && a.keyValues["deployment"] == deployment.keyValues["deployment"]).FirstOrDefault().Value,
                        AvailableReplicas = KubeStateElements.Where(a => a.Name == "kube_deployment_status_replicas_available" && a.keyValues["namespace"] == deployment.keyValues["namespace"] && a.keyValues["deployment"] == deployment.keyValues["deployment"]).FirstOrDefault().Value,
                        CreatedDate = GetDateTime(deployment.Value),
                        Labels = GetLabels(KubeStateElements.Where(a => a.Name == "kube_deployment_labels" && a.keyValues["namespace"] == deployment.keyValues["namespace"] && a.keyValues["deployment"] == deployment.keyValues["deployment"] && a.Value == "1").FirstOrDefault().keyValues)

                    });
                }
                catch(Exception ex)
                {
                    Logger.Error("Error adding deployments  - " + ex.Message);
                }

            }
        }
        private void AddStorageClasses()
        {
            var scs = KubeStateElements.Where(a => a.Name == "kube_storageclass_info");

            foreach (var sc in scs)
            {
                try
                {
                    StorageClasses.Add(new StorageClass()
                    {
                        Name = sc.keyValues["storageclass"],
                        Provisioner = sc.keyValues["provisioner"],
                        ReclaimPolicy = sc.keyValues["reclaimPolicy"],
                        VolumeBindingMode = sc.keyValues["volumeBindingMode"],
                        CreatedDate = GetDateTime(KubeStateElements.Where(a => a.Name == "kube_storageclass_created" && a.keyValues["storageclass"] == sc.keyValues["storageclass"]).FirstOrDefault().Value)
                    });
                }
                catch (Exception ex)
                {
                    Logger.Error("Error adding storage classes  - " + ex.Message);
                }
            }
        }

        private void AddPersistentStorageClaims()
        {
            var pvcs = KubeStateElements.Where(a => a.Name == "kube_persistentvolumeclaim_info");

            foreach (var pvc in pvcs)
            {
                try
                {
                    PersistentVolumeClaims.Add(new PersistentVolumeClaim()
                    {
                        Namespace = pvc.keyValues["namespace"],
                        Name = pvc.keyValues["persistentvolumeclaim"],
                        StorageClass = pvc.keyValues["storageclass"],
                        VolumeName = pvc.keyValues["volumename"],
                        Phase = KubeStateElements.Where(a => a.Name == "kube_persistentvolumeclaim_status_phase" && a.keyValues["persistentvolumeclaim"] == pvc.keyValues["persistentvolumeclaim"] && a.keyValues["namespace"] == pvc.keyValues["namespace"] && a.Value == "1").FirstOrDefault().keyValues["phase"],
                        AccessMode = KubeStateElements.Where(a => a.Name == "kube_persistentvolumeclaim_access_mode" && a.keyValues["persistentvolumeclaim"] == pvc.keyValues["persistentvolumeclaim"] && a.keyValues["namespace"] == pvc.keyValues["namespace"] && a.Value == "1").FirstOrDefault().keyValues["access_mode"],
                        ResourceRequestsStorage = GetByteSize(KubeStateElements.Where(a => a.Name == "kube_persistentvolumeclaim_resource_requests_storage_bytes" && a.keyValues["persistentvolumeclaim"] == pvc.keyValues["persistentvolumeclaim"] && a.keyValues["namespace"] == pvc.keyValues["namespace"]).FirstOrDefault().Value)
                    });
                }
                catch(Exception ex)
                {
                    Logger.Error("Error adding pvcs  - " + ex.Message);
                }
            }
        }

        private void AddPersistentStorages()
        {
            var pvs = KubeStateElements.Where(a => a.Name == "kube_persistentvolume_info");

            foreach (var pv in pvs)
            {
                try
                {
                    PersistentVolumes.Add(new PersistentVolume()
                    {
                        Name = pv.keyValues["persistentvolume"],
                        StorageClass = pv.keyValues["storageclass"],
                        Phase = KubeStateElements.Where(a => a.Name == "kube_persistentvolume_status_phase" && a.keyValues["persistentvolume"] == pv.keyValues["persistentvolume"] && a.Value == "1").FirstOrDefault().keyValues["phase"],
                        Capacity = GetByteSize(KubeStateElements.Where(a => a.Name == "kube_persistentvolume_capacity_bytes" && a.keyValues["persistentvolume"] == pv.keyValues["persistentvolume"]).FirstOrDefault().Value)

                    });
                }
                catch(Exception ex)
                {
                    Logger.Error("Error adding pvs  - " + ex.Message);
                }
            }
        }


        private void AddServices()
        {
            var services = KubeStateElements.Where(a => a.Name == "kube_service_info");

            foreach (var service in services)
            {
                try
                {
                    Services.Add(new Service()
                    {
                        Name = service.keyValues["service"],
                        Namespace = service.keyValues["namespace"],
                        ClusterIP = service.keyValues["cluster_ip"],
                        ServiceType = KubeStateElements.Where(a => a.Name == "kube_service_spec_type" && a.keyValues["namespace"] == service.keyValues["namespace"] && a.keyValues["service"] == service.keyValues["service"]).FirstOrDefault().keyValues["type"],
                        CreatedDate = GetDateTime(KubeStateElements.Where(a => a.Name == "kube_service_created" && a.keyValues["namespace"] == service.keyValues["namespace"] && a.keyValues["service"] == service.keyValues["service"]).FirstOrDefault().Value),
                        Labels = GetLabels(KubeStateElements.Where(a => a.Name == "kube_service_labels" && a.keyValues["namespace"] == service.keyValues["namespace"] && a.keyValues["service"] == service.keyValues["service"] && a.Value == "1").FirstOrDefault().keyValues)
                    }); ;
                }
                catch(Exception ex)
                {
                    Logger.Error("Error adding services  - " + ex.Message);
                }
            }
        }
        private void AddNamespaces()
        {
            var namespaces = KubeStateElements.Where(a => a.Name == "kube_namespace_created");

            foreach (var ns in namespaces)
            {
                try
                {
                    Namespaces.Add(new Namespace() { Name = ns.keyValues["namespace"], CreatedDate = GetDateTime(ns.Value) });
                }
                catch (Exception ex)
                {
                    Logger.Error("Error adding namespaces  - " + ex.Message);
                }
            }
        }
        private void AddNodeStatusCapacity()
        {
            foreach (var node in Nodes)
            {
                try
                {
                    NodeDetails.Where(a => a.Name == node).FirstOrDefault().CPUCore = KubeStateElements.Where(a => a.Name == "kube_node_status_capacity" && a.keyValues["resource"] == "cpu" && a.keyValues["node"] == node).FirstOrDefault().Value;
                    NodeDetails.Where(a => a.Name == node).FirstOrDefault().NoOfPods = KubeStateElements.Where(a => a.Name == "kube_node_status_capacity" && a.keyValues["resource"] == "pods" && a.keyValues["node"] == node).FirstOrDefault().Value;
                    NodeDetails.Where(a => a.Name == node).FirstOrDefault().Memory = GetByteSize(KubeStateElements.Where(a => a.Name == "kube_node_status_capacity" && a.keyValues["resource"] == "memory" && a.keyValues["node"] == node).FirstOrDefault().Value);
                    NodeDetails.Where(a => a.Name == node).FirstOrDefault().Storage = GetByteSize(KubeStateElements.Where(a => a.Name == "kube_node_status_capacity" && a.keyValues["resource"] == "ephemeral_storage" && a.keyValues["node"] == node).FirstOrDefault().Value);
                }
                catch(Exception ex)
                {
                    Logger.Error("Error adding node status capacity  - " + ex.Message);
                }

            }
        }
        private void AddNodeDetails()
        {
            var nodesInfo = KubeStateElements.Where(a => a.Name == "kube_node_info");
            foreach (var node in nodesInfo)
            {
                try
                {
                    NodeDetails.Add(new Node()
                    {

                        Name = node.keyValues["node"],
                        KernalVersion = node.keyValues["kernel_version"],
                        OSImage = node.keyValues["os_image"],
                        containerRuntimeVersion = node.keyValues["container_runtime_version"],
                        KubeletVersion = node.keyValues["kubelet_version"],
                        KubeProxyVersion = node.keyValues["kubeproxy_version"],
                        ProviderId = node.keyValues["provider_id"],
                        PodCIDR = node.keyValues["pod_cidr"],
                        CreatedDate = GetDateTime(KubeStateElements.Where(a => a.Name == "kube_node_created" && a.keyValues["node"] == node.keyValues["node"]).FirstOrDefault().Value)
                    });
                    Nodes.Add(node.keyValues["node"]);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error adding nodes info  - " + ex.Message);
                }
            }
        }

        private void AddNoteStatusConditions()
        {
            var nodeStatusConditions = KubeStateElements.Where(a => a.Name == "kube_node_status_condition" && a.Value == "1");
            foreach (var nodeStatusCondition in nodeStatusConditions)
            {
                try
                {
                    if (nodeStatusCondition.keyValues["condition"] == "MemoryPressure")
                    {
                        NodeDetails.Where(a => a.Name == nodeStatusCondition.keyValues["node"]).FirstOrDefault().MemoryPressure = nodeStatusCondition.keyValues["status"];
                    }
                    else if (nodeStatusCondition.keyValues["condition"] == "DiskPressure")
                    {
                        NodeDetails.Where(a => a.Name == nodeStatusCondition.keyValues["node"]).FirstOrDefault().DiskPressure = nodeStatusCondition.keyValues["status"];
                    }
                    else if (nodeStatusCondition.keyValues["condition"] == "PIDPressure")
                    {
                        NodeDetails.Where(a => a.Name == nodeStatusCondition.keyValues["node"]).FirstOrDefault().PIDPressure = nodeStatusCondition.keyValues["status"];
                    }
                    else if (nodeStatusCondition.keyValues["condition"] == "Ready")
                    {
                        NodeDetails.Where(a => a.Name == nodeStatusCondition.keyValues["node"]).FirstOrDefault().Readiness = nodeStatusCondition.keyValues["status"];
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Error adding node status  - " + ex.Message);
                }

            }
        }

        private void AddNodeRoles()
        {
            var nodeRoles = KubeStateElements.Where(a => a.Name == "kube_node_role");
            foreach (var nodeRole in nodeRoles)
            {
                try
                {
                    NodeDetails.Where(a => a.Name == nodeRole.keyValues["node"]).FirstOrDefault().Roles.Add(nodeRole.keyValues["role"]);
                }
                catch(Exception ex)
                {
                    Logger.Error("Error adding node roles  - " + ex.Message);
                }
            }

        }
        private List<Container> GetContainers(string ns, string pod)
        {
            List<Container> containerList = new List<Container>();
            var containers = KubeStateElements.Where(a => a.Name == "kube_pod_container_info" && a.keyValues["namespace"] == ns && a.keyValues["pod"] == pod && a.Value == "1");
            
            foreach (var container in containers)
            {
                try
                {
                    containerList.Add(new Container()
                    {
                        Namespace = container.keyValues["namespace"],
                        Pod = container.keyValues["pod"],
                        Name = container.keyValues["container"],
                        Image = container.keyValues["image"],
                        ImageId = container.keyValues["image_id"],
                        ContainerId = container.keyValues["container_id"],
                        Status = KubeStateElements.Where(a => (a.Name == "kube_pod_container_status_running" || a.Name == "kube_pod_container_status_waiting" || a.Name == "kube_pod_container_status_terminated") && a.keyValues["namespace"] == container.keyValues["namespace"] && a.keyValues["pod"] == container.keyValues["pod"] && a.keyValues["container"] == container.keyValues["container"] && a.Value == "1").FirstOrDefault().Name,
                        //StatusReason  = KubeStateElements.Where(a => (a.Name == "kube_pod_container_status_terminated_reason" || a.Name == "kube_pod_container_status_waiting_reason") && a.keyValues["namespace"] == container.keyValues["namespace"] && a.keyValues["pod"] == container.keyValues["pod"] && a.keyValues["container"] == container.keyValues["container"] && a.Value == "1").FirstOrDefault() == null ? "None" : .keyValues["reason"],
                        Readiness = KubeStateElements.Where(a => a.Name == "kube_pod_container_status_running" && a.keyValues["namespace"] == container.keyValues["namespace"] && a.keyValues["pod"] == container.keyValues["pod"] && a.keyValues["container"] == container.keyValues["container"] && a.Value == "1").FirstOrDefault() == null ? "Not Ready" : "Ready",
                        Restarts = KubeStateElements.Where(a => a.Name == "kube_pod_container_status_restarts_total" && a.keyValues["namespace"] == container.keyValues["namespace"] && a.keyValues["pod"] == container.keyValues["pod"] && a.keyValues["container"] == container.keyValues["container"]).FirstOrDefault().Value,
                        CPUResourceRequests = GetCPUValue(KubeStateElements.Where(a => a.Name == "kube_pod_container_resource_requests" && a.keyValues["namespace"] == container.keyValues["namespace"] && a.keyValues["pod"] == container.keyValues["pod"] && a.keyValues["container"] == container.keyValues["container"] && a.keyValues["resource"] == "cpu").FirstOrDefault()),
                        MemoryResourceRequests = GetMemoryValue(KubeStateElements.Where(a => a.Name == "kube_pod_container_resource_requests" && a.keyValues["namespace"] == container.keyValues["namespace"] && a.keyValues["pod"] == container.keyValues["pod"] && a.keyValues["container"] == container.keyValues["container"] && a.keyValues["resource"] == "memory").FirstOrDefault()),
                        CPUResourceLimits = GetCPUValue(KubeStateElements.Where(a => a.Name == "kube_pod_container_resource_limits" && a.keyValues["namespace"] == container.keyValues["namespace"] && a.keyValues["pod"] == container.keyValues["pod"] && a.keyValues["container"] == container.keyValues["container"] && a.keyValues["resource"] == "cpu").FirstOrDefault()),
                        MemoryResourceLimits = GetMemoryValue(KubeStateElements.Where(a => a.Name == "kube_pod_container_resource_limits" && a.keyValues["namespace"] == container.keyValues["namespace"] && a.keyValues["pod"] == container.keyValues["pod"] && a.keyValues["container"] == container.keyValues["container"] && a.keyValues["resource"] == "memory").FirstOrDefault())
                    }
                    );
                }
                catch(Exception ex)
                {
                    Logger.Error("Error adding containers  - " + ex.Message);
                }
            }
            return containerList;
        }
        private string GetCPUValue(KubeStateElement element)
        {
            if (element != null)
            {
                if (element.Value != null)
                {
                    return (double.Parse(element.Value) * 1000).ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        private string GetMemoryValue(KubeStateElement element)
        {
            if (element != null)
            {
                if (element.Value != null)
                {
                    return GetByteSize(element.Value);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        private void AddPodDetails()
        {

            var pods = KubeStateElements.Where(a => a.Name == "kube_pod_info"  && a.Value == "1");

            foreach (var pod in pods)
            {
                try
                {
                    Pods.Add(new Pod()
                    {
                        Name = pod.keyValues["pod"],
                        Namespace = pod.keyValues["namespace"],
                        HostIP = pod.keyValues["host_ip"],
                        PodIP = pod.keyValues["pod_ip"],
                        UID = pod.keyValues["uid"],
                        NodeName = pod.keyValues["node"],
                        StartTime = GetDateTime(KubeStateElements.Where(a => a.Name == "kube_pod_start_time" && a.keyValues["namespace"] == pod.keyValues["namespace"] && a.keyValues["pod"] == pod.keyValues["pod"]).FirstOrDefault().Value),
                        CreatedTime = GetDateTime(KubeStateElements.Where(a => a.Name == "kube_pod_created" && a.keyValues["namespace"] == pod.keyValues["namespace"] && a.keyValues["pod"] == pod.keyValues["pod"]).FirstOrDefault().Value),
                        Status = KubeStateElements.Where(a => a.Name == "kube_pod_status_phase" && a.keyValues["namespace"] == pod.keyValues["namespace"] && a.keyValues["pod"] == pod.keyValues["pod"] && a.Value == "1").FirstOrDefault().keyValues["phase"],
                        Readiness = KubeStateElements.Where(a => a.Name == "kube_pod_status_ready" && a.keyValues["namespace"] == pod.keyValues["namespace"] && a.keyValues["pod"] == pod.keyValues["pod"] && a.Value == "1").FirstOrDefault().keyValues["condition"],
                        Containers = GetContainers(pod.keyValues["namespace"], pod.keyValues["pod"]),
                        Labels = GetLabels(KubeStateElements.Where(a => a.Name == "kube_pod_labels" && a.keyValues["namespace"] == pod.keyValues["namespace"] && a.keyValues["pod"] == pod.keyValues["pod"] && a.Value == "1").FirstOrDefault().keyValues),
                         
                         
                    }
                    );
                }
                catch(Exception ex)
                {
                    Logger.Error("Error adding pods  - " + ex.Message);
                }
            }
        }

        private List<Label> GetLabels(Dictionary<string,string> keyValues)
        {
            List<Label> labels = new List<Label>();
            foreach (var key in keyValues.Keys.Where(a => a.StartsWith("label_")))
            {
                labels.Add(new Label() { Name = GetLabelName(key), Value = keyValues[key] });
            }
            return labels;
        }
        private string GetLabelName(string labelName)
        {
            if (labelName.Contains("_kubernetes_io_"))
            {
                labelName = labelName.Replace("_kubernetes_io_", ".kubernetes.io/");
            }
            labelName = labelName.Substring(6).Replace("_", "-");
            return labelName;
        }
        private DateTime GetDateTime(string strDate)
        {
            try
            {
                long numDate = long.Parse(strDate, System.Globalization.NumberStyles.Float);
                return UnixTimeToDateTime(numDate);
            }
            catch (Exception ex)
            {
                Logger.Error("Date parse error - " + ex.Message);
                throw ex;
            }
        }

        private DateTime UnixTimeToDateTime(long unixtime)
        {
            try
            {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
                return dtDateTime;
            }
            catch (Exception ex)
            {
                Logger.Error("UnixTime parse error - " + ex.Message);
                throw ex;
            }
        }

        private string GetByteSize(string size)
        {
            long numSize = long.Parse(size, System.Globalization.NumberStyles.Float);
            //return numSize.ToString();
            //return ByteSize.FromBytes(numSize).LargestWholeNumberBinaryValue.ToString() + " " + ByteSize.FromBytes(numSize).LargestWholeNumberBinarySymbol;
            return ByteSize.FromBytes(numSize).KibiBytes.ToString();
        }
        private void ParseMetrics(string[] metrics)
        {
            foreach (var line in metrics)
            {
                try
                {
                    if (!line.StartsWith("#"))
                    {
                        KubeStateElements.Add(new KubeStateElement() { Name = line.Substring(0, line.IndexOf('{')), keyValues = generateKeyValuePairList(line), Value = line.Substring(line.IndexOf('}') + 2) });
                    }
                }
                catch(Exception ex)
                {
                    Logger.Error("metric format error - " + ex.Message);
                }
            }
            
        }
        private Dictionary<string,string> generateKeyValuePairList(string line)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            var str = line.Substring(line.IndexOf('{') + 1, line.LastIndexOf('}') - line.IndexOf('{')-1).Replace("\"","");
            string[] keyValues = str.Split(',');
            foreach (string keyValue in keyValues)
            {
                string[] kv = keyValue.Split('=');
                dictionary.Add(kv[0], kv[1]);
            }
            return dictionary;
        }

        public List<StatefulSet> StatefulSets { get; set; } = new List<StatefulSet>();
        public List<Node> NodeDetails { get; set; } = new List<Node>();

        public List<KubeStateElement> KubeStateElements { get; set; } = new List<KubeStateElement>();

        public List<string> Nodes = new List<string>();

        public List<Namespace> Namespaces = new List<Namespace>();

        public List<Service> Services = new List<Service>();

        public List<PersistentVolume> PersistentVolumes = new List<PersistentVolume>();

        public List<PersistentVolumeClaim> PersistentVolumeClaims = new List<PersistentVolumeClaim>();

        public List<StorageClass> StorageClasses = new List<StorageClass>();

        public List<Deployment> Deployments = new List<Deployment>();

        public List<Pod> Pods = new List<Pod>();

        public DateTime TimeStamp { get; set; }

    }
}
