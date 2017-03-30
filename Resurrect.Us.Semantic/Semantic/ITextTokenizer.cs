using System.Collections.Generic;

namespace Resurrect.Us.Semantic.Semantic
{
    public interface ITextTokenizer
    {
        List<string> Tokenize(string tokenizeTarget);
    }
}