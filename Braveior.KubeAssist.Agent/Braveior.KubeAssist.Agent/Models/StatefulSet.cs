using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Agent.Models
{
    public class StatefulSet
    {
        public string Name { get; set; }
        public string Namespace { get; set; }

        public string NoOfReplicas { get; set; }

        public string CurrentReplicas { get; set; }

        public string ReadyReplicas { get; set; }

        public string UpdatedReplicas { get; set; }

        public string NoOfDesiredPods { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<Label> Labels { get; set; } = new List<Label>();

    }
}

