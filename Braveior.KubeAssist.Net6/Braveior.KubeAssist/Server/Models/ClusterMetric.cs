using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Models
{
    public class ClusterMetric
    {
        public string Name { get; set; }
        
        public int CPU { get; set; }

        public long Memory { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
