using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace demobot.Model
{
    using Newtonsoft.Json;

    public class LinguisticRequest
    {
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("analyzerIds")]
        public List<string> AnalyzerIds { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}