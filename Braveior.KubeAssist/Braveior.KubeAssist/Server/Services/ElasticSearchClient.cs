using k8s;
using Nest;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Server.Services
{
    public class ElasticSearchClient
    {
        private readonly ILogger _logger;
        public ElasticClient client;

        public ElasticSearchClient(ILogger logger)
        {
            _logger = logger;
            try 
            { 
                var settings = new ConnectionSettings(new Uri("http://192.168.0.112:9200/"));
                client = new ElasticClient(settings);
                var response = client.Ping();
                if (!response.IsValid)
                {
                    _logger.Error("Not connected to the Elastic Cluster");
                    _logger.Error(response.OriginalException.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

    }
}
