using System;
using MediaBrowser.Model.Plugins;
#pragma warning disable CA1819

namespace Jellyfin.Plugins.Gotify.Configuration
{
    /// <summary>
    /// Plugin configuration.
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginConfiguration"/> class.
        /// </summary>
        public PluginConfiguration()
        {
            Options = Array.Empty<GotifyOptions>();
        }

        /// <summary>
        /// Gets or sets configured options.
        /// </summary>
        public GotifyOptions[] Options { get; set; }
    }
}