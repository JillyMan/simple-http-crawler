using Crawler.App.Core.Parser;

namespace Crawler.App.Core.Validators
{
	public interface INodeValidator
    {
        bool IsValid(ParsedNode node);
    }
}