using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace demobot.Model
{
    using Newtonsoft.Json;

    public class RequestDocument
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class TextAnalyticsRequest
    {
        [JsonProperty("documents")]
        public List<RequestDocument>  Documents { get; set; }
    }
}