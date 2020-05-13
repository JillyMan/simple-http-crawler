using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.App.Core.Parser
{
    public class HtmlTagsExporter : ITagsParser
    {
        private readonly HtmlParser _htmlParser;
        private readonly ParserRule[] _parserRules;

        public HtmlTagsExporter(ParserRule[] parserRules)
        {
            _parserRules = parserRules;
            _htmlParser = new HtmlParser();
        }

        public IEnumerable<ParsedNode> Parse(string content)
        {
            var document = _htmlParser.ParseDocument(content);
            var result = _parserRules
                .SelectMany(x => Selector(document, x.Name, x.Attr, x.Type))
                .ToList();
            return result;
        }

        private IList<ParsedNode> Selector(IHtmlDocument doc, string nameEl, string attr, NodeType type)
        {
            return doc.QuerySelectorAll(nameEl)
                .Select(x => x.GetAttribute(attr))
                .Distinct()
                .Select(x => new ParsedNode()
                {
                    Value = x,
                    Type = type
                }).ToList();
        }
    }
}