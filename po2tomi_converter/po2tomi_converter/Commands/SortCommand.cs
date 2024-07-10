using Microsoft.Extensions.Options;
using po2tomi_converter.Dtos;
using po2tomi_converter.Settings;
using po2tomi_converter.Utils;
using System.Text;
using System.Threading.Tasks;

namespace po2tomi_converter.Commands
{
    public class SortCommand
    {
        private readonly MainSettings _settings;
        private readonly List<Line> _engSteam;
        private readonly List<Line> _engGog;
        private readonly List<Line> _plSteam;
        private readonly List<Line> _plGog;

        public SortCommand(IOptions<MainSettings> options)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _settings = options.Value;

            _engSteam = LineUtils.LoadLines(_settings.SteamEngFileLocation);
            _engGog = LineUtils.LoadLines(_settings.GogEngFileLocation);
            _plSteam = LineUtils.LoadLines(_settings.SteamPlFileLocation);
            //_plGog = LineUtils.LoadLines(_settings.GogPlFileLocation);
        }

        public void Execute()
        {
            var engSteamBuilder = new StringBuilder();
            var engGogBuilder = new StringBuilder();
            var plSteamBuilder = new StringBuilder();
            var plGogBuilder = new StringBuilder();

            File.WriteAllText(_settings.SteamEngFileLocation, BuildFile(_engSteam).ToString(), Encoding.GetEncoding("windows-1250"));
            File.WriteAllText(_settings.GogEngFileLocation, BuildFile(_engGog).ToString(), Encoding.GetEncoding("windows-1250"));
            File.WriteAllText(_settings.SteamPlFileLocation, BuildFile(_plSteam).ToString(), Encoding.GetEncoding("windows-1250"));
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
