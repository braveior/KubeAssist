using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Agent.Models
{
    public class Pod
    {
        public string Namespace { get; set; }

        public string Name { get; set; }

        public string HostIP { get; set; }
        public string PodIP { get; set; }

        public string UID { get; set; }

        public string NodeName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Status { get; set; }

        public string Readiness { get; set; }

        public List<Container> Containers { get; set; } = new List<Container>();

        public List<Label> Labels { get; set; } = new List<Label>();



    }
}
