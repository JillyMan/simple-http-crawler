using Crawler.App.Core.Parser;

namespace Crawler.App.Transformer
{
	public interface IParsedNodeTransformer
    {
        ParsedNode Transorm(ParsedNode el);
    }
}