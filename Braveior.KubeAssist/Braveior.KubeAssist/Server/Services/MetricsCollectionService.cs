using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Server.Services
{
    internal sealed class MetricsCollectionService : EventListener, IHostedService
    {
        private List<string> RegisteredEventSources = new List<string>();
        private Task _newDataSourceTask;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _newDataSourceTask = Task.Run(async () =>
            {
                while (true)
                {
                    GetNewSources();
                    await Task.Delay(1000);
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            if (!RegisteredEventSources.Contains(eventSource.Name))
            {
                RegisteredEventSources.Add(eventSource.Name);
                EnableEvents(eventSource, EventLevel.LogAlways, EventKeywords.All, new Dictionary<string, string>
            {
                {"EventCounterIntervalSec", "10"}
            });
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData.EventName != "EventCounters"
                    || eventData.Payload.Count <= 0
                    || !(eventData.Payload[0] is IDictionary<string, object> data)
                    || !data.TryGetValue("CounterType", out var counterType)
                    || !data.TryGetValue("Name", out var name))
                return;

            var metricType = counterType.ToString();
            float metricValue = 0;

            if ("Sum".Equals(metricType) && data.TryGetValue("Increment", out var increment))
            {
                metricValue = Convert.ToSingle(increment);
            }
            else if ("Mean".Equals(metricType) && data.TryGetValue("Mean", out var mean))
            {
                metricValue = Convert.ToSingle(mean);
            }
            //Console.WriteLine("Event Counter Data - " + metricType + " - " + name + " - " + metricValue);
            // do something with your metric here...
        }

        private void GetNewSources()
        {
            foreach (var eventSource in EventSource.GetSources())
                OnEventSourceCreated(eventSource);
        }
    }
}
