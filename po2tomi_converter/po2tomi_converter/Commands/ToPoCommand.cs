
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
        private readonly Regex _regex = new Regex("\\d+\\)");

        public ToPoCommand(IOptions<MainSettings> options) 
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _settings = options.Value;
        }

        public void Execute()
        {
            var linesEngSteam = LoadLines(_settings.SteamEngFileLocation);
            var linesEngGog = LoadLines(_settings.GogEngFileLocation);
            var dictPl = LoadLines(_settings.SteamPlFileLocation)
                .ToDictionary(x => x.Number, y => y);

            var translations = new List<Translation>();
            foreach (var lineEng in linesEngSteam)
            {
                var translation = new Translation
                {
                    SteamEngLine = lineEng,
                    PlLine = dictPl[lineEng.Number],
                    GogEngLine = FindInGog(lineEng, linesEngGog)
                };
                translations.Add(translation);
            }

            var lackingGog = new List<Translation>();
            foreach (var lineEng in linesEngGog)
            {
                if (!FindInSteam(lineEng, linesEngSteam))
                {
                    var translation = new Translation
                    {
                        SteamEngLine = null,
                        PlLine = lineEng,
                        GogEngLine = lineEng
                    };
                    lackingGog.Add(translation);
                }
            }

            translations.AddRange(lackingGog);

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

        private static Line FindInGog(Line steamLine, List<Line> gogLines)
        {
            foreach (var line in gogLines)
            {
                if(line.Content ==  steamLine.Content)
                    return line;
            }
            return null;
        }

        private static bool FindInSteam(Line gogLine, List<Line> steamLines)
        {
            foreach (var searchLine in steamLines)
            {
                if (searchLine.Content == gogLine.Content)
                {
                    return true;
                }
            }
            return false;
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
