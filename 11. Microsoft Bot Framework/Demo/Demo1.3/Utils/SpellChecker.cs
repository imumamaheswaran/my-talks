using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace demobot.Utils
{
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using demobot.Model;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class SpellChecker
    {
        public static async Task<string> CorrectSpelling(string sentence)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(String.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "5bc1a295e8c642f7857763477396089c");

            var uri = "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck?";
            string text = sentence;
            string mode = "proof";
            string mkt = "en-us";
            try
            {
                var result = await client.GetAsync(string.Format("{0}text={1}&mode={2}&mkt={3}", uri, text, mode, mkt));
                result.EnsureSuccessStatusCode();
                var json = await result.Content.ReadAsStringAsync();
                SpellCheckResponse spellCheckResponse = JsonConvert.DeserializeObject<SpellCheckResponse>(json);
                foreach (var correction in spellCheckResponse.FlaggedTokens)
                {
                    sentence = sentence.Replace(correction.Token, correction.suggestions[0].Suggestion);
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }



            return sentence;
        }
    }
}