using Braveior.KubeAssist.Services.Models;
using Elasticsearch.Net;
using k8s;
using k8s.Models;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Services
{
    public interface IKubernetesService
    {
        Task<V1NodeList> GetNodes();
    }
}
