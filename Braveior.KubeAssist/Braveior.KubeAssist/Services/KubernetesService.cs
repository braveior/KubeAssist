using Braveior.KubeAssist.Services.Models;
using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Services
{
    public class KubernetesService
    {


        public List<KNamespace> GetNamespaces()
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            var client = new Kubernetes(config);
            var namespaces = client.ListNamespace();
            var namespaceList = new List<KNamespace>();
            foreach (var ns in namespaces.Items)
            {
                namespaceList.Add(new KNamespace() { Name = ns.Metadata.Name });
            }
            return namespaceList;
        }
    }
}
