using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Services.Models
{
    public class Pod
    {
        public string Namespace { get; set; }

        public string Name { get; set; }

        public string HostIP { get; set; }
        public string PodIP { get; set; }

        public string UID { get; set; }

        public string NodeName { get; set; }

        public string StartTime { get; set; }

        public string CreatedTime { get; set; }

        public string Status { get; set; }

        public string Readiness { get; set; }

        public List<Container> Containers { get; set; } = new List<Container>();

        public bool ShowDetails { get; set; }

        public List<Label> Labels { get; set; } = new List<Label>();

    }
}
