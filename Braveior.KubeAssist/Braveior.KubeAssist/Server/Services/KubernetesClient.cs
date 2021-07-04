using k8s;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Server.Services
{
    public class KubernetesClient
    {
        private readonly ILogger _logger;
        public IKubernetes client;
        public KubernetesClient(ILogger logger)
        {
            _logger = logger;
            KubernetesClientConfiguration config;
            try
            {
                if (KubernetesClientConfiguration.IsInCluster())
                {
                    config = KubernetesClientConfiguration.InClusterConfig();
                }
                else
                {
                    config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
                }



                client = new Kubernetes(config);
            }
            catch (Exception ex)
            {
                _logger.Error("Not connected to the Kubernetes Cluster");
                throw ex;
            }
        }

    }
}
