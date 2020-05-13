using Crawler.App.Core;
using Crawler.App.Core.Parser;
using Crawler.App.Core.Validators;
using Crawler.App.Transformer;
using System;
using System.Collections.Generic;

namespace Crawler
{
    public class MYCrawler
    {
        private readonly HashSet<string> _elementCache = new HashSet<string>();

        private readonly int _analyzeDeep;

        private readonly ITagsParser _tagExporter;
        private readonly IDataProvider _dataProvider;
        private readonly INodeValidator[] _nodeValidators;
        private readonly IParsedNodeTransformer _transformer;

        public Action<string> OnUrlStartProcess;
        public Action<string, Exception> OnUrlProcessError;

        public MYCrawler(IDataProvider dataProvider,
            ITagsParser tagExporter,
            INodeValidator[] nodeValidators,
            IParsedNodeTransformer transformer,
            int analyzeDeep)
        {
            _analyzeDeep = analyzeDeep;
            _tagExporter = tagExporter;
            _transformer = transformer;
            _dataProvider = dataProvider;
            _nodeValidators = nodeValidators;
        }

        public MYCrawler(HashSet<string> elementCache)
        {
            _elementCache = elementCache;
        }

        public IList<CrawlerResultElement> Parse(string baseUrl)
        {
            _elementCache.Clear();
            if (Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
                return ParseHtml(baseUrl);

            return new List<CrawlerResultElement>();
        }

        private List<CrawlerResultElement> ParseHtml(string url, int currentDeep = 0)
        {
            var result = new List<CrawlerResultElement>();

            if (currentDeep > _analyzeDeep || _elementCache.Contains(url))
            {
                return result;
            }

            OnUrlStartProcess?.Invoke(url);

            string content;
            try
            {
                content = _dataProvider.GetFrom(url).GetAwaiter().GetResult();
            }
            catch(Exception e)
            {
                OnUrlProcessError?.Invoke($"Some error occured while loading from the url: {url}", e);
                return result;
            }

            result.Add(new CrawlerResultElement()
            {
                Url = url,
                Content = content,
                Deep = currentDeep,
                TypeResult = CrawlerResultEnum.Page,
            });
            _elementCache.Add(url);

            result.AddRange(ProcessNodes(_tagExporter.Parse(content), currentDeep));
            return result;
        }

        private List<CrawlerResultElement> ProcessNodes(IEnumerable<ParsedNode> nodes, int currentDeep)
        {
            var result = new List<CrawlerResultElement>();

            foreach (var node in nodes)
            {
                if (IsValid(node) && !_elementCache.Contains(node.Value))
                {
                    var newNode = _transformer.Transorm(node);

                    switch (newNode.Type)
                    {
                        case NodeType.Anchor:
                            var crawlerResult = ParseHtml(newNode.Value, currentDeep + 1);
                            result.AddRange(crawlerResult);
                            break;
                        case NodeType.Image:
                            _elementCache.Add(node.Value);
                            result.Add(new CrawlerResultElement()
                            {
                                Content = _dataProvider.GetFrom(newNode.Value).GetAwaiter().GetResult(),
                                TypeResult = CrawlerResultEnum.Image,
                                Deep = currentDeep,
                                Url = newNode.Value
                            });
                            break;
                        default:
                            throw new ApplicationException($"Unknow type of node: {newNode.Type}");
                    }
                }
            }

            return result;
        }

        private bool IsValid(ParsedNode node)
        {
            foreach(var validator in _nodeValidators)
            {
                if (!validator.IsValid(node))
                {
                    return false;
                }
            }
            return true;
        }
    }
}