
using Microsoft.Extensions.Options;
using po2tab_converter.Utils;
using po2tomi_converter.Settings;
using System.Text;

namespace po2tomi_converter.Commands
{
    public class FromPoCommand
    {
        private readonly MainSettings _settings;

        public FromPoCommand(IOptions<MainSettings> options) 
        {
            _settings = options.Value;
        }

        public void Execute()
        {
            string file = File.ReadAllText(_settings.PoFileLocation);
            var splitted = file.Split("msgctxt");

            var resultPl = new StringBuilder();
            var resultOrg = new StringBuilder();
            string errors = "";

            foreach (var text in splitted)
            {
                if (string.IsNullOrEmpty(text)) continue;
                var textWithCuttedStart = "msgctxt" + text;

                var splitter = new PoSplitter(textWithCuttedStart);
                if (!splitter.IsValid)
                    continue;

                try
                {
                    var markup = splitter.Markup;
                    var plText = splitter.PlText;
                    var orgText = splitter.OrgText;

                    var splittedMarkup = splitter.Markup.Split("_");
                    var number = splittedMarkup[0];
                    var sublineNumber = splittedMarkup[1];
                    var author = splittedMarkup[2];


                    var toAppendPl = "";
                    var toAppendOrg = "";
                    if (sublineNumber == "0")
                    {
                        toAppendPl = $"{number})  {author}\n{plText}\n";
                        toAppendOrg = $"{number})  {author}\n{orgText}\n";
                    }
                    else
                    {
                        toAppendPl = $"{plText}\n";
                        toAppendOrg = $"{orgText}\n";
                    }

                    resultPl.Append(toAppendPl);
                    resultOrg.Append(toAppendOrg);
                }
                catch (Exception ex)
                {
                    errors += textWithCuttedStart;
                    errors += ex.Message;
                    errors += "\n\n";
                }
            }

            System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            if (!string.IsNullOrEmpty(errors))
            {
                File.WriteAllText("errors.txt", errors, Encoding.GetEncoding("windows-1250"));
            }

            File.WriteAllText(_settings.SteamPlFileLocation, resultPl.ToString(), Encoding.GetEncoding("windows-1250"));
            File.WriteAllText(_settings.SteamEngFileLocation, resultOrg.ToString(), Encoding.GetEncoding("windows-1250"));
        }
    }
}
