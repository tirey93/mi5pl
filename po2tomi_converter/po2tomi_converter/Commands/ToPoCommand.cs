
using Microsoft.Extensions.Options;
using po2tomi_converter.Settings;
using po2tomi_converter.Utils;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var linesEng = LoadLines(_settings.TomiEngFileLocation);
            var dictPl = LoadLines(_settings.TomiPlFileLocation)
                .ToDictionary(x => x.Number, y => y);

            var lines = new List<Line[]>();
            foreach (var lineEng in linesEng)
            {
                var array = new Line[2];
                array[0] = lineEng;
                array[1] = dictPl[lineEng.Number];
                lines.Add(array);
            }

            var poResult = new StringBuilder();
            foreach (var line in lines)
            {

                var splittedEng = line[0].Content.Split('\n');
                var splittedPl = line[1].Content.Split('\n');
                int i = 0;
                foreach(var splittedLine in splittedEng)
                {
                    var markup = $"{line[0].Number}_{i}_{line[0].Author}";

                    poResult.Append(ToPo(markup, splittedEng[i], splittedPl[i]));
                    i = i + 1;
                }
            }
            
            File.WriteAllText(_settings.PoFileLocation, poResult.ToString());
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
