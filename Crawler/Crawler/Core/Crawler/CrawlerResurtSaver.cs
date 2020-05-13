using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crawler.App.Core.Crawler
{
    public class CrawlerResultSaver : ICrawlerResultSaver
	{
        private readonly string BaseFolder;

        public CrawlerResultSaver(string baseFolder)
        {
            BaseFolder = baseFolder;
        }

		public void Save(IList<CrawlerResultElement> elements)
		{
            FileSystemFascade.CreateFolderWithRetry(BaseFolder);

            foreach (var el in elements)
            {
                var segments = new Uri(el.Url).Segments;
                var name = segments.Last();

                if (el.TypeResult == CrawlerResultEnum.Page)
                {
                    name = FormatName(name);
                }

                var saveToFolder = BaseFolder;
                if (segments.Length > 2)
                {
                    saveToFolder = CreateSubFolders(BaseFolder, segments.Skip(1).SkipLast(1).ToArray());
                }

                var saveToPath = $"{saveToFolder}/{name}";
                //----
                    Console.WriteLine($"Try to save by path: {saveToPath}");
                //----
                File.Create(saveToPath).Dispose();
                File.WriteAllText(saveToPath, el.Content);
            }
        }

        private string CreateSubFolders(string basePath, string[] folders)
        {
            var newFolder = basePath + "/";
            for (int i = 0; i < folders.Length; ++i)
            {
                newFolder += folders[i];
                if (!Directory.Exists(newFolder))
                {
                    FileSystemFascade.CreateFolderWithRetry(newFolder);
                }
            }

            return newFolder;
        }

        private string FormatName(string name)
        {
            if (name == "/" || string.IsNullOrEmpty(name))
                return "index.html";

            if (name.Last() == '/')
                return FormatName(new string(name.SkipLast(1).ToArray()));

            return name.Contains(".html") ? name : name + ".html";
        }
    }
}
