
using Microsoft.Extensions.Options;
using po2tomi_converter.Settings;

namespace po2tomi_converter.Commands
{
    public class ToPoCommand
    {
        private readonly MainSettings _settings;

        public ToPoCommand(IOptions<MainSettings> options) 
        {
            _settings = options.Value;
        }

        public void Execute()
        {

        }
    }
}
