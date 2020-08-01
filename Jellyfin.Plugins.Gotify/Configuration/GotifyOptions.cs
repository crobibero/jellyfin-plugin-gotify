using System;

namespace Jellyfin.Plugins.Gotify.Configuration
{
    /// <summary>
    /// Gotify options container.
    /// </summary>
    public class GotifyOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Gets or sets the userId.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the notification priority.
        /// </summary>
        public int Priority { get; set; }
    }
}