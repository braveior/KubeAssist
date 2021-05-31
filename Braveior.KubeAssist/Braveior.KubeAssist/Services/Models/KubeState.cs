using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Services.Models
{
    public class KubeState
    {
        public List<KNode> NodeDetails { get; set; } = new List<KNode>();

        public List<KubeStateElement> KubeStateElements { get; set; } = new List<KubeStateElement>();

        public List<string> Nodes = new List<string>();

        public List<Namespace> Namespaces = new List<Namespace>();

        public List<Deployment> Deployments = new List<Deployment>();

        public List<Pod> Pods = new List<Pod>();

        public List<Service> Services = new List<Service>();

        public List<PersistentVolume> PersistentVolumes = new List<PersistentVolume>();

        public List<PersistentVolumeClaim> PersistentVolumeClaims = new List<PersistentVolumeClaim>();

        public List<StorageClass> StorageClasses = new List<StorageClass>();

        public List<StatefulSet> StatefulSets { get; set; } = new List<StatefulSet>();
        public DateTime TimeStamp { get; set; }

    }
}
