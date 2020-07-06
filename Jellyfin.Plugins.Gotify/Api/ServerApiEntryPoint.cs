using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jellyfin.Plugins.Gotify.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Serialization;
using MediaBrowser.Model.Services;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugins.Gotify.Api
{
    [Route("/Notification/Gotify/Test/{UserId}", "Post", Summary = "Tests Gotify")]
    public class TestNotification : IReturnVoid
    {
        [ApiMember(Name = "UserId", Description = "User Id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "GET")]
        public string UserId { get; set; }
    }

    public class ServerApiEndpoints : IService
    {
        private readonly IHttpClient _httpClient;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ILogger<ServerApiEndpoints> _logger;

        public ServerApiEndpoints(IHttpClient httpClient, IJsonSerializer jsonSerializer, ILoggerFactory loggerFactory)
        {
            _httpClient = httpClient;
            _jsonSerializer = jsonSerializer;
            _logger = loggerFactory.CreateLogger<ServerApiEndpoints>();
        }
        
        private static GotifyOptions GetOptions(string userId)
        {
            return Plugin.Instance.Configuration.Options
                .FirstOrDefault(u => string.Equals(u.UserId, userId,
                    StringComparison.OrdinalIgnoreCase));
        }

        private async Task PostAsync(TestNotification request)
        {
            var options = GetOptions(request.UserId);
            var body = new Dictionary<string, object>
            {
                {"message", "This is a test notification from Jellyfin"},
                {"title", "Test Notification"},
                {"priority", options.Priority}
            };

            var requestOptions = new HttpRequestOptions
            {
                Url = options.Url.TrimEnd('/') + $"/message?token={options.Token}",
                RequestContent = _jsonSerializer.SerializeToString(body),
                RequestContentType = "application/json",
                LogErrorResponseBody = true
            };

            await _httpClient.Post(requestOptions).ConfigureAwait(false);
        }

        public void Post(TestNotification request)
        {
            PostAsync(request)
                .GetAwaiter()
                .GetResult();
        }
    }
}