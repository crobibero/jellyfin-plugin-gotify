using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Data.Entities;
using Jellyfin.Plugins.Gotify.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Notifications;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugins.Gotify
{
    /// <summary>
    /// Notification service entry point.
    /// </summary>
    public class Notifier : INotificationService
    {
        private readonly IHttpClient _httpClient;
        private readonly ILogger<Notifier> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Notifier"/> class.
        /// </summary>
        /// <param name="httpClient">Instance of the <see cref="IHttpClient"/> interface.</param>
        /// <param name="logger">Instance of the <see cref="ILogger{Notifier}"/> interface.</param>
        public Notifier(IHttpClient httpClient, ILogger<Notifier> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <inheritdoc />
        public string Name => Plugin.Instance!.Name;

        /// <summary>
        /// Send notification.
        /// </summary>
        /// <param name="request">The notification request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task SendNotification(UserNotification request, CancellationToken cancellationToken)
        {
            var options = GetOptions(request.User);

            var body = new Dictionary<string, object>();

            // message parameter is required. If it is sent as null
            // put name as message

            if (string.IsNullOrEmpty(request.Description))
            {
                body.Add("message", request.Name);
            }
            else
            {
                if (!string.IsNullOrEmpty(request.Name))
                {
                    body.Add("title", request.Name);
                }

                body.Add("message", request.Description);
            }

            body.Add("priority", options.Priority);

            if (string.IsNullOrEmpty(options.Url))
            {
                return;
            }

            var requestOptions = new HttpRequestOptions
            {
                Url = options.Url.TrimEnd('/') + $"/message?token={options.Token}",
                RequestContent = JsonSerializer.Serialize(body),
                RequestContentType = MediaTypeNames.Application.Json,
                LogErrorResponseBody = true
            };

            await _httpClient.Post(requestOptions).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public bool IsEnabledForUser(User user)
        {
            var options = GetOptions(user);
            return options != null && IsValid(options) && options.Enabled;
        }

        private static GotifyOptions GetOptions(User user)
        {
            return Plugin.Instance!.Configuration.Options
                .FirstOrDefault(u => u.UserId.Equals(user.Id));
        }

        private static bool IsValid(GotifyOptions options)
        {
            return !string.IsNullOrEmpty(options.Token)
                   && !string.IsNullOrEmpty(options.Url);
        }
    }
}