using CommandLine;
using Crawler.App.Core;
using Crawler.App.Core.Crawler;
using Crawler.App.Core.Parser;
using Crawler.App.Core.Validators;
using Crawler.App.Transformer;
using System;
using System.Linq;

namespace Crawler
{
    class Program
    {
        public CommandLineOptions Options;

        public Program(CommandLineOptions options)
        {
            Options = options;
        }

        public void Start()
        {
            using var httpProvider = new HttpProvider();
            var exporter = new CrawlerResultSaver(Options.DestinationFolder);
            var tagExporter = new HtmlTagsExporter(
                    new ParserRule[]
                    {
                        new ParserRule() { Name = "a",   Attr = "href", Type = NodeType.Anchor },
                        new ParserRule() { Name = "img", Attr = "src",  Type = NodeType.Image }
                    }
                );

            var validators = new INodeValidator[]
                {
                    new UriNodeValidator(new string[]{ "https", "http", "file" }),
                    new DomainNodeValidator(
                        Options.BaseUrl,
                        Options.MoveRestriction
                    ),
                    new ImageNodeValidator(Options.AllowedResources.ToArray())
                };

            var transformer = new FromRelativeToAbsoluteTransformer(Options.BaseUrl);

            var crawler = new MYCrawler(
                httpProvider,
                tagExporter,
                validators,
                transformer,
                Options.AnalyzeDeep);

            if (Options.VerboseMode)
            {
                crawler.OnUrlStartProcess += (url) => Console.WriteLine("start process: {0}", url);
                crawler.OnUrlProcessError += (message, e) =>
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"{message}\n{e.Message}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                };
            }

            var result = crawler.Parse(Options.StartUrl);
            exporter.Save(result);
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(options =>
                {
                    try
                    {
                        new Program(options).Start();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });
        }
    }
}