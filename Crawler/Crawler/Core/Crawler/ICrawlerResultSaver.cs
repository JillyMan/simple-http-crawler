using System.Collections.Generic;

namespace Crawler.App.Core.Crawler
{
	public interface ICrawlerResultSaver
	{
		void Save(IList<CrawlerResultElement> elements);
	}
}
