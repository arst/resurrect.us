using HtmlAgilityPack;
using Resurrect.Us.Semantic.Services;
using Resurrect.Us.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resurrect.Us.Web.Service
{
    public class DOMProcessingService : IDOMProcessingService
    {
        private readonly ISemanticService semanticService;

        public DOMProcessingService(ISemanticService semanticService)
        {
            this.semanticService = semanticService;
        }
        public HTMLKeypointsResult ExtractHTMLKeypoints(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            HTMLKeypointsResult result = new HTMLKeypointsResult();
            doc.LoadHtml(html);
            var keywordElements = doc.DocumentNode.Descendants("keywords");
            var titleElements = doc.DocumentNode.Descendants("title");

            if (titleElements != null && titleElements.Any())
            {
                result.Title = titleElements.First().InnerText;
            }

            if (keywordElements != null && keywordElements.Any())
            {
                List<string> keyWords = keywordElements.First().GetAttributeValue("content", "").Split(',').ToList();

                if (keyWords.Count > 0)
                {
                    result.Keywords = keyWords;   
                }
            }
            else
            {
                var text = doc.DocumentNode.InnerText;
                result.Keywords = this.semanticService.GetTopKeywords(text, 10);
            }

            return result;
        }
    }
}
