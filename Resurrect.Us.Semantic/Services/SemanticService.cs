using Resurrect.Us.Semantic.Semantic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Resurrect.Us.Semantic.Services
{
    public class SemanticService : ISemanticService
    {
        private readonly IWordsFrequencyCounter wordsFrequencyCounter;

        public SemanticService(IWordsFrequencyCounter wordsFrequencyCounter)
        {
            this.wordsFrequencyCounter = wordsFrequencyCounter;
        }

        public List<string> GetTopKeywords(string text, int? limit)
        {
            var frequencies = this.wordsFrequencyCounter.GetWordsFrequencyCount(text).ToList();
            frequencies.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            return frequencies.Take(limit ?? Int32.MaxValue).Select(kv => kv.Key).ToList();        
        }
    }
}
