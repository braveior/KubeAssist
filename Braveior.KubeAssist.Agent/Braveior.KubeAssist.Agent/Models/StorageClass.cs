using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Agent.Models
{
    public class StorageClass
    {
        public string Name { get; set; }
        public string Provisioner { get; set; }

        public string ReclaimPolicy { get; set; }

        public string VolumeBindingMode { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
