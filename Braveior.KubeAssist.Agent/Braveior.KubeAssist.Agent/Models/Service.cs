using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Agent.Models
{
    public class Service
    {
        public string Name { get; set; }

        public string Namespace { get; set; }

        public string ClusterIP { get; set; }

        public string ExternalName { get; set; }

        public string LoadBalancerIP { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ServiceType { get; set; }

        public List<Label> Labels { get; set; } 

    }
}
