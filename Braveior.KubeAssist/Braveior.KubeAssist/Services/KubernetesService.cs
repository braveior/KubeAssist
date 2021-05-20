using Braveior.KubeAssist.Services.Models;
using k8s;
using k8s.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Services
{
    public class KubernetesService
    {
        public KubeState GetKubeState()
        {
            var settings = new ConnectionSettings(new Uri("http://192.168.0.112:9200/"))
           .DefaultIndex("kubestate");
            var client = new ElasticClient(settings);
            // var asyncIndexResponse = await client.IndexDocumentAsync(kubeState);

            var searchResponse = client.Search<KubeState>(s => s
            .From(0)
            .Size(1)
            .Query(q => q.MatchAll())
            .Sort(s => s.Descending(a => a.TimeStamp)));

            return searchResponse.Documents.FirstOrDefault();
        }

    }
}
