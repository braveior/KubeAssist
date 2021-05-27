using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Agent.Models
{
    public class Node
    {
        public string Name { get; set; }
        public string KernalVersion { get; set; }
        public string OSImage { get; set; }

        public string containerRuntimeVersion { get; set; }

        public string KubeletVersion { get; set; }
        public string KubeProxyVersion { get; set; }

        public string ProviderId { get; set; }

        public string PodCIDR { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<Label> Labels { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        public string MemoryPressure { get; set; }

        public string DiskPressure { get; set; }

        public string PIDPressure { get; set; }

        public string Readiness { get; set; }

        public string Memory { get; set; }

        public string NoOfPods { get; set; }

        public string CPUCore { get; set; }

        public string Storage { get; set; }



    }
}
