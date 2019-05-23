using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace demobot.Model
{
    using Newtonsoft.Json;

    public class SpellCheckSuggestion
    {
        [JsonProperty("suggestion")]
        public string Suggestion { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public class FlaggedToken
    {
        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("suggestions")]
        public List<SpellCheckSuggestion> suggestions { get; set; }
    }

    public class SpellCheckResponse
    {
        [JsonProperty("_type")]
        public string _type { get; set; }

        [JsonProperty("flaggedTokens")]
        public List<FlaggedToken> FlaggedTokens { get; set; }
    }
}