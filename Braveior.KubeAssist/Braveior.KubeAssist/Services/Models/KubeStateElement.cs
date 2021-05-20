using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Services.Models
{
    public class KubeStateElement
    {
        public string Name { get; set; }

        public Dictionary<string, string> keyValues { get; set; } = new Dictionary<string, string>();

        public string Value { get; set; }
    }
}
