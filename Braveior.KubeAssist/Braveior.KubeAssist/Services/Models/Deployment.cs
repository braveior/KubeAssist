using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Services.Models
{
    public class Deployment
    {
        public string Name { get; set; }
        public string Namespace { get; set; }

        public string TotalReplicas { get; set; }

        public string AvailableReplicas { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}

