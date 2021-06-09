using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Agent.Models
{
    public class PersistentVolumeClaim
    {
        public string Name { get; set; }
        public string Namespace { get; set; }

        public string StorageClass { get; set; }

        public string VolumeName { get; set; }

        public string Phase { get; set; }

        public string ResourceRequestsStorage { get; set; }

        public string AccessMode { get; set; }

        public List<Label> Labels { get; set; } = new List<Label>();

    }
}
