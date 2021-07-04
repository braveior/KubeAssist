using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Services.Models
{
    public class NamespaceMetric
    {
        public string Name { get; set; }
        
        public long CPU { get; set; }

        public long Memory { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
