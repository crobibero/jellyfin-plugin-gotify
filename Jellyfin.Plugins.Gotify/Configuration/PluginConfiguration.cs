using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugins.Gotify.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public GotifyOptions[] Options { get; set; }

        public PluginConfiguration()
        {
            Options = new GotifyOptions[0];
        }
    }

    public class GotifyOptions
    {
        public bool Enabled { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public int Priority { get; set; }
    }
}