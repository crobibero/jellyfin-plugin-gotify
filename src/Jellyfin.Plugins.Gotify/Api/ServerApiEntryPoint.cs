using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jellyfin.Plugins.Gotify.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Services;

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

        
        public ServerApiEndpoints(IHttpClient httpClient)
        {
            _httpClient = httpClient;
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

            var body = new Dictionary<string, string>
            {
                {"message", "This is a test notification from Emby"},
                {"title", "Test Notification"}
            };

            var requestOptions = new HttpRequestOptions
            {
                Url = options.Url.TrimEnd('/') + $"/message?token={options.Token}"
            };

            requestOptions.SetPostData(body);

            await _httpClient.Post(requestOptions).ConfigureAwait(false);
        }
        
        public void Post(TestNotification request)
        {
            var task = PostAsync(request);
            Task.WaitAll(task);
        }
    }
}