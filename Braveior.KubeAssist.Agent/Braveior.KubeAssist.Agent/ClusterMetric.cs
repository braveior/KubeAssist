using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Agent
{
    public class ClusterMetric
    {
        public string Name { get; set; }
        
        public int CPU { get; set; }

        public int Memory { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
