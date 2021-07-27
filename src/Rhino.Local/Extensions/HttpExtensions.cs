using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rhino.Local.Extensions
{
    public static class HttpExtensions
    {
        #region *** Content ***
        public static ContentResult GetContentResult(this HttpResponseMessage response)
        {
            var isJson = response.TryGetJson(out string content);
            return new ContentResult
            {
                Content = content,
                StatusCode = (int)response.StatusCode,
                ContentType = isJson ? MediaTypeNames.Application.Json : MediaTypeNames.Text.Plain
            };
        }

        private static bool TryGetJson(this HttpResponseMessage response, out string content)
        {
            content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            try
            {
                JsonDocument.Parse(content);
                return true;
            }
            catch (Exception e) when (e != null)
            {
                return false;
            }
        }
        #endregion

        #region *** Client  ***
        public static async Task<IActionResult> InvokeBridgeRequest(
            this HttpClient client, HttpRequest request, HttpResponse response, ILogger logger)
        {
            // setup
            var requestUri = request.QueryString.HasValue
                ? $"{request.Path}{request.QueryString}"
                : $"{request.Path}";
            var content = await GetContent(request);
            var method = GetMethod(request);

            // build
            var requestMessage = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };
            SetAuthentication(requestMessage, request);

            // send
            var _response = await client.SendAsync(requestMessage);
            logger.LogInformation("Resolve-RhinoRoute " +
                $"-Method {method} " +
                $"-Mode Bridge " +
                $"-Path {request.Path} = {_response.StatusCode}");

            // build
            SetResponseHeaders(response, _response.Headers);

            // get
            return _response.GetContentResult();
        }

        private static async Task<HttpContent> GetContent(HttpRequest request)
        {
            // setup
            var reader = new StreamReader(request.Body);
            var requestBody = await reader.ReadToEndAsync();

            // not found
            if (string.IsNullOrEmpty(requestBody))
            {
                return null;
            }

            // content
            return string.IsNullOrEmpty(request.ContentType)
                ? new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json)
                : new StringContent(requestBody, Encoding.UTF8, request.ContentType);
        }

        private static void SetResponseHeaders(HttpResponse response, HttpResponseHeaders headers)
        {
            foreach (var header in headers)
            {
                if (!header.Key.StartsWith("Rhino"))
                {
                    continue;
                }
                response.Headers[header.Key] = header.Value.FirstOrDefault();
            }
        }

        private static HttpMethod GetMethod(HttpRequest request)
        {
            // setup
            var property = typeof(HttpMethod)
                .GetProperties()
                .FirstOrDefault(i => i.Name.Equals(request.Method, StringComparison.OrdinalIgnoreCase));

            // not found
            if (property == null)
            {
                return HttpMethod.Get;
            }

            // build
            return (HttpMethod)property.GetValue(null);
        }

        private static void SetAuthentication(HttpRequestMessage requestMessage, HttpRequest request)
        {
            // setup
            var token = !request.Headers.ContainsKey("Authorization")
                ? string.Empty
                : Regex.Match(request.Headers["Authorization"], @"(?<=^Basic\s).*$").Value;

            // not found
            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            // setup
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
        }
        #endregion
    }
}