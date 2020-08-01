using System;
using MediaBrowser.Model.Services;

namespace Jellyfin.Plugins.Gotify.Api
{
    /// <summary>
    /// Send test notification.
    /// </summary>
    [Route("/Notification/Gotify/Test/{UserId}", "Post", Summary = "Tests Gotify")]
    public class TestNotification : IReturnVoid
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        [ApiMember(Name = "UserId", Description = "User Id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "GET")]
        public Guid UserId { get; set; }
    }
}