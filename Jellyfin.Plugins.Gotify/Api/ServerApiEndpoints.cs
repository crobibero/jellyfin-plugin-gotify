using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Jellyfin.Plugins.Gotify.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Services;

namespace Jellyfin.Plugins.Gotify.Api
{
    /// <summary>
    /// Api endpoints.
    /// </summary>
    public class ServerApiEndpoints : IService
    {
        private readonly IHttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerApiEndpoints"/> class.
        /// </summary>
        /// <param name="httpClient">Instance of the <see cref="IHttpClient"/> interface.</param>
        public ServerApiEndpoints(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private static GotifyOptions GetOptions(Guid userId)
        {
            return Plugin.Instance!.Configuration.Options
                .FirstOrDefault(u => u.UserId.Equals(userId));
        }

        private async Task PostAsync(TestNotification request)
        {
            var options = GetOptions(request.UserId);
            var body = new Dictionary<string, object>
            {
                { "message", "This is a test notification from Jellyfin" },
                { "title", "Test Notification" },
                { "priority", options.Priority }
            };

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

        /// <summary>
        /// Send a test notification.
        /// </summary>
        /// <param name="request">The test notification.</param>
        public void Post(TestNotification request)
        {
            PostAsync(request)
                .GetAwaiter()
                .GetResult();
        }
    }
}