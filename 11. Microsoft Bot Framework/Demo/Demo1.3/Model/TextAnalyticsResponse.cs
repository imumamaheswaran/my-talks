using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace demobot.Model
{
    using Newtonsoft.Json;

    public class ResponseDocument
    {
        [JsonProperty("score")]
        public double score { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }
    }

    public class Error
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("message")]
        public string message { get; set; }
    }

    public class TextAnalyticsResponse
    {
        [JsonProperty("documents")]
        public List<ResponseDocument> documents { get; set; }

        [JsonProperty("errors")]
        public List<Error> errors { get; set; }
    }
}