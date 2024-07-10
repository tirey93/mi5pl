
using Microsoft.Extensions.Options;
using po2tomi_converter.Settings;
using System.Text;
using po2tomi_converter.Dtos;
using po2tomi_converter.Utils;
using System.Collections.Generic;

namespace po2tomi_converter.Commands
{
    public class ToPoCommand
    {
        private readonly MainSettings _settings;
        private readonly Dictionary<int, Line> _dictEngSteam;
        private readonly int _maxLineNumberSteam;
        private readonly List<Line> _linesEngGog;
        private readonly Dictionary<int, Line> _dictPl;

        public ToPoCommand(IOptions<MainSettings> options) 
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _settings = options.Value;

            _dictEngSteam = LineUtils.LoadLines(_settings.SteamEngFileLocation)
                .ToDictionary(x => x.Number, y => y);
            _maxLineNumberSteam = _dictEngSteam.Keys.Max();
            _linesEngGog = LineUtils.LoadLines(_settings.GogEngFileLocation)
                .OrderBy(x => x.Number).ToList();
            _dictPl = LineUtils.LoadLines(_settings.SteamPlFileLocation)
                .ToDictionary(x => x.Number, y => y);
        }

        public void Execute()
        {
            var shift = 0;
            var translations = new List<Translation>();
            foreach (var lineEngGog in _linesEngGog)
            {
                if (lineEngGog.Number == 118)
                {

                }
                if(!_dictEngSteam.ContainsKey(lineEngGog.Number + shift))
                {
                    var newShift = FindInSteam(lineEngGog, shift, _dictEngSteam);
                    if (newShift != null)
                    {
                        translations.AddRange(AddLinesBeforeShift(shift, newShift, lineEngGog));
                        shift = newShift.Value;
                    }
                    else
                    {
                        translations.Add(new Translation
                        {
                            SteamEngLine = null,
                            PlLine = lineEngGog,
                            GogEngLine = lineEngGog
                        });
                        continue;
                    }
                }
                if (lineEngGog.Content != _dictEngSteam[lineEngGog.Number + shift].Content)
                {
                    var newShift = FindInSteam(lineEngGog, shift, _dictEngSteam);
                    if (newShift == null)
                    {
                        translations.Add(new Translation
                        {
                            SteamEngLine = null,
                            PlLine = lineEngGog,
                            GogEngLine = lineEngGog
                        });
                        continue;
                    }
                    translations.AddRange(AddLinesBeforeShift(shift, newShift, lineEngGog));
                    shift = newShift.Value;
                }
                translations.Add(new Translation
                {
                    SteamEngLine = _dictEngSteam[lineEngGog.Number + shift],
                    PlLine = _dictPl[lineEngGog.Number + shift],
                    GogEngLine = lineEngGog
                });
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

        private List<Translation> AddLinesBeforeShift(int shift, int? newShift, Line lineEngGog)
        {
            var result = new List<Translation>();
            for (int i = 0; i < newShift - shift; i++)
            {
                if (!_dictEngSteam.ContainsKey(lineEngGog.Number + shift + i))
                    continue;

                var steamTranslation = new Translation
                {
                    SteamEngLine = _dictEngSteam[lineEngGog.Number + shift + i],
                    PlLine = _dictPl[lineEngGog.Number + shift + i],
                    GogEngLine = null
                };
                result.Add(steamTranslation);
            }
            return result;
        }

        private int? FindInSteam(Line gogLine, int shift, Dictionary<int, Line> dictEngSteam)
        {
            var result = shift - 5;
            for (int i = 0; i < _maxLineNumberSteam - gogLine.Number + shift + 5; i++)
            {
                result = result + 1;
                if (!dictEngSteam.ContainsKey(gogLine.Number + result))
                    continue;
                if (dictEngSteam[gogLine.Number + result].Content == gogLine.Content)
                {
                    return result;
                }
            }

            return null;
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

        private static string ToPo(string markup, string engStr, string plStr)
        {
            var result = $"msgctxt \"{markup}\"\n";
            result += $"msgid \"{engStr}\"\n";
            result += $"msgstr \"{plStr}\"\n\n";

            return result;
        }
    }
}
