using System;
using System.Collections.Generic;
using System.Text;

namespace Resurrect.Us.Semantic.Semantic
{
    public class WordsFrequencyCounter : IWordsFrequencyCounter
    {
        private readonly ITextTokenizer tokenizer;

        public WordsFrequencyCounter(ITextTokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
        }

        public Dictionary<string, int> GetWordsFrequencyCount(string text)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            var tokens = this.tokenizer.Tokenize(text);

            foreach (var token in tokens)
            {
                if (result.ContainsKey(token))
                {
                    result[token]++;
                }
                else
                {
                    result.Add(token, 1);
                }
            }

            return result;
        }
    }
}
