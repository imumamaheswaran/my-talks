using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace demobot.Model
{
    public class LinquisticResponse
    {
        [JsonProperty("analyzerId")]
        public string AnalyzerId { get; set; }
        [JsonProperty("result")]
        public List<string> Result { get; set; }
    }
}