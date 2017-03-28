using System.Collections.Generic;

namespace Resurrect.Us.Semantic.Services
{
    public interface ISemanticService
    {
        List<string> GetTopKeywords(string text, int? limit);
    }
}