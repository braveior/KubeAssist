using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Agent.Models
{
    public class PersistentVolume
    {
        public string Name { get; set; }

        public string StorageClass { get; set; }

        public string Phase { get; set; }

        public string Capacity { get; set; }

    }
}
