using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace demobot.Utils
{
    using System.Text;

    using demobot.Model;

    public static class GrammarStatistics
    {
        static GrammarStatistics()
        {
            SpeechParts = new List<PartsOfSpeech>();
            SpeechParts.Add(new PartsOfSpeech("CC", "conjunction, coordinating"));
            SpeechParts.Add(new PartsOfSpeech("CD", "numeral, cardinal"));
            SpeechParts.Add(new PartsOfSpeech("DT", "determiner"));
            SpeechParts.Add(new PartsOfSpeech("EX", "existential thereExistential"));
            SpeechParts.Add(new PartsOfSpeech("FW", "foreign word"));
            SpeechParts.Add(new PartsOfSpeech("IN", "preposition or subordinating conjunction"));
            SpeechParts.Add(new PartsOfSpeech("JJ", "adjective or numeral, ordinal"));
            SpeechParts.Add(new PartsOfSpeech("JJR", "adjective, comparative"));
            SpeechParts.Add(new PartsOfSpeech("JJS", "adjective, superlative"));
            SpeechParts.Add(new PartsOfSpeech("LS", "list item marker"));
            SpeechParts.Add(new PartsOfSpeech("MD", "modal auxiliary"));
            SpeechParts.Add(new PartsOfSpeech("NN", "noun, common, singular or mass"));
            SpeechParts.Add(new PartsOfSpeech("NNP", "noun, proper, singular"));
            SpeechParts.Add(new PartsOfSpeech("NNPS", "noun, proper, plural"));
            SpeechParts.Add(new PartsOfSpeech("NNS", "noun, common, plural"));
            SpeechParts.Add(new PartsOfSpeech("PDT", "pre - determiner"));
            SpeechParts.Add(new PartsOfSpeech("POS", "genitive marker"));
            SpeechParts.Add(new PartsOfSpeech("PRP", "pronoun, personal"));
            SpeechParts.Add(new PartsOfSpeech("PRP$", "pronoun, possessive"));
            SpeechParts.Add(new PartsOfSpeech("RB", "adverb"));
            SpeechParts.Add(new PartsOfSpeech("RBR", "adverb, comparative"));
            SpeechParts.Add(new PartsOfSpeech("RBS", "adverb, superlative"));
            SpeechParts.Add(new PartsOfSpeech("RP", "particle"));
            SpeechParts.Add(new PartsOfSpeech("SYM", "symbol"));
            SpeechParts.Add(new PartsOfSpeech("TO", "'to' as preposition or infinitive marker"));
            SpeechParts.Add(new PartsOfSpeech("UH", "interjection"));
            SpeechParts.Add(new PartsOfSpeech("VB", "verb, base form"));
            SpeechParts.Add(new PartsOfSpeech("VBD", "verb, past tense"));
            SpeechParts.Add(new PartsOfSpeech("VBG", "verb, present participle or gerund"));
            SpeechParts.Add(new PartsOfSpeech("VBN", "verb, past participle"));
            SpeechParts.Add(new PartsOfSpeech("VBP", "verb, present tense, not 3rd person singular"));
            SpeechParts.Add(new PartsOfSpeech("VBZ", "verb, present tense, 3rd person singular"));
            SpeechParts.Add(new PartsOfSpeech("WDT", "WH - determiner"));
            SpeechParts.Add(new PartsOfSpeech("WP", "WH - pronoun"));
            SpeechParts.Add(new PartsOfSpeech("WP$", "WH - pronoun, possessive"));
            SpeechParts.Add(new PartsOfSpeech("WRB", "Wh - adverb"));
        }
        private static int partsFound = 0;
        private static List<PartsOfSpeech> speechParts;

        public static List<PartsOfSpeech> SpeechParts
        {
            get { return speechParts; }
            set { speechParts = value; }
        }

        public static void ParseParts(LinquisticResponse parts)
        {
            foreach (var part in SpeechParts)
            {
                part.Count = 0;
            }
            partsFound = 0;

            var listOfParts = parts.Result[0].Split(' ');
            foreach (var smallPart in listOfParts)
            {
                if (smallPart.Substring(0, 1) == "(")
                {
                    var loc = SpeechParts.Find(a => a.Tag == smallPart.Substring(1));
                    if (loc != null)
                    {
                        partsFound++;
                        loc.Count++;
                    }
                }
            }
        }

        public static string AnalysisResult
        {
            get
            {
                StringBuilder result = new StringBuilder();
                foreach (var part in SpeechParts)
                {
                    if (part.Count > 0)
                    {
                        result.Append(part.Description);
                        result.Append(" ");
                        result.Append(Math.Truncate(Convert.ToDecimal(part.Count) / partsFound * 100));
                        result.Append("%");
                        result.AppendLine();
                    }
                }
                if (result.Length > 0)
                {
                    result.Remove(result.Length - 1, 1);
                }
                return result.ToString();
            }
        }

    }
}