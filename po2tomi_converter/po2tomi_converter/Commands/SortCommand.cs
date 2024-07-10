using Microsoft.Extensions.Options;
using po2tomi_converter.Dtos;
using po2tomi_converter.Settings;
using po2tomi_converter.Utils;
using System.Text;

namespace po2tomi_converter.Commands
{
    public class SortCommand
    {
        private readonly MainSettings _settings;

        public SortCommand(IOptions<MainSettings> options)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _settings = options.Value;
        }

        public void Execute()
        {
            SortFile(_settings.SteamEngFileLocation);
            SortFile(_settings.GogEngFileLocation);
            SortFile(_settings.SteamPlFileLocation);
            SortFile(_settings.GogPlFileLocation);
        }

        private void SortFile(string location)
        {
            if (!File.Exists(location)) 
                return;
            var lines = LineUtils.LoadLines(location);
            File.WriteAllText(location, BuildFile(lines).ToString(), Encoding.GetEncoding("windows-1250"));

        }

        private StringBuilder BuildFile(List<Line> lines)
        {
            var result = new StringBuilder();
            foreach (var line in lines.OrderBy(x => x.Number))
            {
                result.Append($"{line.Number})  {line.Author}\n{line.Content}\n");
            }
            return result;
        }
    }
}
