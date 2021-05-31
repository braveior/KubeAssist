using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Agent.Models
{
    public class Deployment
    {
        public string Name { get; set; }
        public string Namespace { get; set; }

        public string TotalReplicas { get; set; }

        public string AvailableReplicas { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<Label> Labels { get; set; } = new List<Label>();

    }
}

