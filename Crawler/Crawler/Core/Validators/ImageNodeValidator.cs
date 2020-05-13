using Crawler.App.Core.Parser;
using System.Linq;

namespace Crawler.App.Core.Validators
{
    public class ImageNodeValidator : INodeValidator
    {
        private readonly string[] _notAvalibaleImageTypes;

        public ImageNodeValidator(string[] notAvalibaleImageTypes)
        {
            _notAvalibaleImageTypes = notAvalibaleImageTypes;
        }

        public bool IsValid(ParsedNode node)
        {
            if (node.Type == NodeType.Image)
            {
                return !_notAvalibaleImageTypes.Any(x => node.Value.Contains($".{x}"));
            }
            return true;
        }
    }
}