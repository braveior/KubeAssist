using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Agent.Models
{
    public class KubeStateElement
    {
        public string Name { get; set; }

        public Dictionary<string, string> keyValues { get; set; } = new Dictionary<string, string>();

        //public List<Label> Labels { get; set; } = new List<Label>();

        public string Value { get; set; }
    }
}
