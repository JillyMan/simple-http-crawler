using System.Collections.Generic;

namespace Crawler.App.Core.Parser
{
	public interface ITagsParser
    {
        IEnumerable<ParsedNode> Parse(string content);
    }
}