using Crawler.App.Core.Parser;
using System;
using System.Linq;

namespace Crawler.App.Core.Validators
{
	public class UriNodeValidator : INodeValidator
	{
		private readonly string[] _avalibleSchemas;

		public UriNodeValidator(string[] avalibleSchemas)
		{
			_avalibleSchemas = avalibleSchemas;
		}

		public bool IsValid(ParsedNode node)
		{
			if (!(node.Type == NodeType.Anchor || node.Type == NodeType.Image)) 
				return true;

			return Uri.IsWellFormedUriString(node.Value, UriKind.Absolute) && 
				_avalibleSchemas.Contains(new Uri(node.Value).Scheme);
		}
	}
}
