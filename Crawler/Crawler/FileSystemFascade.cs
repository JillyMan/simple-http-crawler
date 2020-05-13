using System;
using System.IO;

namespace Crawler
{
	public class FileSystemFascade
    {
        public static void CreateFolderWithRetry(string path)
        {
            Execute(() =>
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                Directory.CreateDirectory(path);

                if (!Directory.Exists(path))
                {
                    throw new Exception("Folder is not created.");
                }
            }, 10);
        }

        private static void Execute(Action action, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception)
                {
                }
            }
        }
    }
}