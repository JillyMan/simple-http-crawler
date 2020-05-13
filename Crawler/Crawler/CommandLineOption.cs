using CommandLine;
using System.Collections.Generic;

namespace Crawler
{
    public enum MoveRestriction
    {
        CurrentDomain,
        NotHigherThenCurrent,
        Default
    }

    public class CommandLineOptions
    {
        [Option('d',
            "deep",
            Default = 0,
            HelpText = "Введите ограничение на глубину анализа ссылок.")]
        public int AnalyzeDeep { get; set; }

        [Option('m',
            "move restrictions",
            Default = "default",
            HelpText = "Введите ограничение на переход на другие домены. (currentdomain, nothiherthencurrent, default)")]
        public MoveRestriction MoveRestriction { get; set; }

        [Option('r',
            "allowed-resources",
            Separator = ',',
            HelpText = "Ограничение на «расширение» скачиваемых ресурсов (можно задавать списком, например так: gif,jpeg,jpg,pdf.)")]
        public IList<string> AllowedResources { get; set; }

        [Option('v',
            "verbose-mode",
            HelpText = "Трассировка (verbose режим): показ на экране текущей обрабатываемой страницы/документа")]
        public bool VerboseMode { get; set; }

        [Option ('f', Default = "./ParsedSite")]
        public string DestinationFolder { get; set; }


        [Option('b', "baseurl")]
        public string BaseUrl { get; set; }

        [Option('s', "starturl")]
        public string StartUrl { get; set; }

    }
}
