using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Intrinsicly.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProxyController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ProxyController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> GetHtmlContent([FromQuery] string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return BadRequest("URL parameter is required.");
            }

            try
            {
                // Clear previous headers and set custom ones
                _httpClient.DefaultRequestHeaders.Clear();

                // Add the User-Agent header
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36");

                // Accept Headers
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));

                // Add the Referer header
                _httpClient.DefaultRequestHeaders.Referrer = new Uri("https://www.google.com/");

                // Add the Accept-Language header
                _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US", 0.9));
                _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en", 0.8));

                // Add the Cache-Control and Pragma headers
                _httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true
                };
                _httpClient.DefaultRequestHeaders.Pragma.Add(new NameValueHeaderValue("no-cache"));

                // Add the Upgrade-Insecure-Requests header
                _httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, contentType);
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, $"Request Error: {e.Message}");
            }
        }
    }
}
