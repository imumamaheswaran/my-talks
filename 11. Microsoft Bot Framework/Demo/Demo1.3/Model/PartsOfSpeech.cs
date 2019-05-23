using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace demobot.Model
{
    public class PartsOfSpeech
    {
        public PartsOfSpeech(string tag, string description)
        {
            this.Tag = tag;
            this.Description = description;
        }
        public string Tag { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
    }
}