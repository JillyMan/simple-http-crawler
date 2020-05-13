using Crawler.App.Core.Parser;
using System;

namespace Crawler.App.Core.Validators
{
	public class DomainNodeValidator : INodeValidator
    {
        private readonly Uri _domain;
        private readonly MoveRestriction _moveRestriction;

        public DomainNodeValidator(string domain, MoveRestriction moveRestriction = MoveRestriction.Default)
        {
            _domain = new Uri(domain);
            _moveRestriction = moveRestriction;
        }

        public bool IsValid(ParsedNode node)
        {
            if (node.Type == NodeType.Anchor || node.Type == NodeType.Image)
            {
                var nodeUri = new Uri(node.Value);
                return _moveRestriction switch
                {
                    MoveRestriction.Default => true,
                    MoveRestriction.CurrentDomain => nodeUri.Host == _domain.Host,
                    MoveRestriction.NotHigherThenCurrent => _domain.IsBaseOf(nodeUri),
                    _ => false,
                };
            }

            return true;
        }
    }
}