
using Microsoft.Extensions.Options;
using po2tomi_converter.Settings;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using po2tomi_converter.Dtos;

namespace po2tomi_converter.Commands
{
    public class ToPoCommand
    {
        private readonly MainSettings _settings;
        private readonly Dictionary<int, Line> _dictEngSteam;
        private readonly Regex _regex = new Regex("\\d+\\)");
        private readonly int _maxLineNumberSteam;
        private readonly List<Line> _linesEngGog;
        private readonly Dictionary<int, Line> _dictPl;

        public ToPoCommand(IOptions<MainSettings> options) 
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _settings = options.Value;

            _dictEngSteam = LoadLines(_settings.SteamEngFileLocation)
                .ToDictionary(x => x.Number, y => y);
            _maxLineNumberSteam = _dictEngSteam.Keys.Max();
            _linesEngGog = LoadLines(_settings.GogEngFileLocation)
                .OrderBy(x => x.Number).ToList();
            _dictPl = LoadLines(_settings.SteamPlFileLocation)
                .ToDictionary(x => x.Number, y => y);
        }

        public void Execute()
        {
            var shift = 0;
            var translations = new List<Translation>();
            foreach (var lineEngGog in _linesEngGog)
            {
                if (lineEngGog.Number == 2413)
                {
                }
                if (lineEngGog.Content != _dictEngSteam[lineEngGog.Number + shift].Content)
                {
                    var newShift = FindInSteam(lineEngGog, shift, _dictEngSteam);
                    for (int i = 0; i < newShift - shift; i++)
                    {
                        var steamTranslation = new Translation
                        {
                            SteamEngLine = _dictEngSteam[lineEngGog.Number + shift + i],
                            PlLine = _dictPl[lineEngGog.Number + shift + i],
                            GogEngLine = null
                        };
                        translations.Add(steamTranslation);
                    }
                    shift = newShift;
                }
                var translation = new Translation
                {
                    SteamEngLine = _dictEngSteam[lineEngGog.Number + shift],
                    PlLine = _dictPl[lineEngGog.Number + shift],
                    GogEngLine = lineEngGog
                };
                translations.Add(translation);
            }

            var poResult = new StringBuilder();
            foreach (var translation in translations)
            {
                string[] splittedEng;
                string[] splittedPl;
                if (translation.SteamEngLine != null)
                {
                    splittedEng = translation.SteamEngLine.Content.Split('\n');
                }
                else
                {
                    splittedEng = translation.GogEngLine.Content.Split('\n');
                }
                splittedPl = translation.PlLine.Content.Split('\n');


                int i = 0;

                foreach(var splittedLine in splittedEng)
                {
                    var markup = SetMarkup(translation, i);

                    poResult.Append(ToPo(markup, splittedEng[i], splittedPl[i]));
                    i = i + 1;
                }
            }
            File.WriteAllText(_settings.PoFileLocation, poResult.ToString());
        }

        private static string SetMarkup(Translation translation, int i)
        {
            if (translation.SteamEngLine != null && translation.GogEngLine != null)
            {
                return $"{translation.SteamEngLine.Number}_{translation.GogEngLine.Number}_{i}_{translation.SteamEngLine.Author}";
            }
            else if (translation.SteamEngLine != null && translation.GogEngLine == null)
            {
                return $"{translation.SteamEngLine.Number}__{i}_{translation.SteamEngLine.Author}";
            }
            else if (translation.SteamEngLine == null && translation.GogEngLine != null)
            {
                return $"_{translation.GogEngLine.Number}_{i}_{translation.GogEngLine.Author}";
            }
            return string.Empty;
        }

        private int FindInSteam(Line gogLine, int shift, Dictionary<int, Line> dictEngSteam)
        {
            var result = shift - 5;
            for (int i = 0; i < _maxLineNumberSteam - gogLine.Number + shift + 5; i++)
            {
                if (dictEngSteam[gogLine.Number + result].Content == gogLine.Content)
                {
                    return result;
                }
                result = result + 1;
            }
            return result;
        }

        private static string ToPo(string markup, string engStr, string plStr)
        {
            var result = $"msgctxt \"{markup}\"\n";
            result += $"msgid \"{engStr}\"\n";
            result += $"msgstr \"{plStr}\"\n\n";

            return result;
        }

        private List<Line> LoadLines(string fileLocation)
        {
            var lines = File.ReadAllLines(fileLocation, Encoding.GetEncoding("windows-1250"));

            var matchedLines = new List<Line>();

            var currentLine = new Line(lines[0]);
            for (int i = 1; i < lines.Length; i++)
            {
                var strLine = lines[i];

                if (_regex.Match(strLine).Success)
                {
                    matchedLines.Add(currentLine);
                    currentLine = new Line(strLine);
                }
                else
                {
                    currentLine.AddContent(strLine);
                }
            }

            return matchedLines;
        }
    }
}
