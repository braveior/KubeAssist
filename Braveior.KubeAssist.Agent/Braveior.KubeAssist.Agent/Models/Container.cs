﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Agent.Models
{
    public class Container
    {
        public string Namespace { get; set; }

        public string Pod { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string ImageId { get; set; }

        public string ContainerId { get; set; }

        public string Status { get; set; }

        public string StatusReason { get; set; }

        public string LastTerminatedReason { get; set; }

        public string Readiness { get; set; }

        public string Restarts { get; set; }

        public string CPUResourceRequests { get; set; }

        public string MemoryResourceRequests { get; set; }

        public string CPUResourceLimits { get; set; }

        public string MemoryResourceLimits { get; set; }


    }
}
