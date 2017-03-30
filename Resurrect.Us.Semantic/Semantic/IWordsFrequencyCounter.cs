using System.Collections.Generic;

namespace Resurrect.Us.Semantic.Semantic
{
    public interface IWordsFrequencyCounter
    {
        Dictionary<string, int> GetWordsFrequencyCount(string text);
    }
}