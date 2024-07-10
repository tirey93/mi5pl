
namespace po2tomi_converter.Settings
{
    public class MainSettings
    {
        public Mode Mode { get; set; }
        public string PoFileLocation { get; set; }
        public string SteamEngFileLocation { get; set; }
        public string SteamPlFileLocation { get; set; }
        public string GogEngFileLocation { get; set; }
        public string GogPlFileLocation { get; set; }
    }

    public enum Mode
    {
        ToPo,
        FromPo,
        Sort
    }
}
