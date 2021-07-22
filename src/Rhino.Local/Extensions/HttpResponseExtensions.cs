using Microsoft.AspNetCore.Mvc;

using System.Net.Http;
using System.Net.Mime;

namespace Rhino.Local.Extensions
{
    public static class HttpResponseExtensions
    {
        public static ContentResult GetContentResult(this HttpResponseMessage response) => new()
        {
            Content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
            StatusCode = (int)response.StatusCode,
            ContentType = MediaTypeNames.Application.Json
        };
    }
}