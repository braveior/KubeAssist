using System;
using System.Collections.Generic;
using System.Text;

namespace Braveior.KubeAssist.Agent
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Shards
    {
        public int total { get; set; }
        public int successful { get; set; }
        public int skipped { get; set; }
        public int failed { get; set; }
    }

    public class Total
    {
        public int value { get; set; }
        public string relation { get; set; }
    }

    public class Hits
    {
        public Total total { get; set; }
        public object max_score { get; set; }
        public List<object> hits { get; set; }
    }

    public class AvgRam
    {
        private double _value; 
        public double value { get { return _value; } set { _value = Math.Round(value, 2); } }
    }

    public class AvgCpu
    {
        private double _value;
        public double value { get { return _value; } set { _value = Math.Round(value, 2); } }
    }

    public class Bucket
    {
        public DateTime key_as_string { get; set; }
        public object key { get; set; }
        public int doc_count { get; set; }
        public AvgRam avg_ram { get; set; }
        public AvgCpu avg_cpu { get; set; }
    }

    public class SalesOverTime
    {
        public List<Bucket> buckets { get; set; }
    }

    public class Aggregations
    {
        public SalesOverTime sales_over_time { get; set; }
    }

    public class MetricResult
    {
        public int took { get; set; }
        public bool timed_out { get; set; }
        public Shards _shards { get; set; }
        public Hits hits { get; set; }
        public Aggregations aggregations { get; set; }
    }
}
