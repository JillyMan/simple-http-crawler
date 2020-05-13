using Crawler.App.Core.Parser;
using System;

namespace Crawler.App.Transformer
{
	public class FromRelativeToAbsoluteTransformer : IParsedNodeTransformer
    {
        private string _baseAddress;

        public FromRelativeToAbsoluteTransformer(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public ParsedNode Transorm(ParsedNode node)
        {
            if (node.Type == NodeType.Image || node.Type == NodeType.Anchor)
            {
                if (Uri.IsWellFormedUriString(node.Value, UriKind.Relative))
                {
                    return new ParsedNode()
                    {
                        Type = node.Type,
                        Value = $"{_baseAddress}{node.Value}"
                    };
                }
            }

            return node;
        }
    }
}