using System.Net;
using System.Text;

namespace Dekanat.ScheduleSdk.Tests.Helpers;

/// <summary>
/// Перехоплює останній HTTP-запит і повертає задану JSON-відповідь (для юніт-тестів клієнта).
/// </summary>
internal sealed class RecordingHttpMessageHandler : HttpMessageHandler
{
    public HttpRequestMessage? LastRequest { get; private set; }

    public string ResponseContent { get; set; } =
        """{"psrozklad_export":{"code":"0"}}""";

    public Encoding ResponseEncoding { get; set; } = Encoding.UTF8;

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        LastRequest = request;
        byte[] bytes = ResponseEncoding.GetBytes(ResponseContent);
        ByteArrayContent content = new(bytes);
        content.Headers.ContentType = new("application/json")
        {
            CharSet = ResponseEncoding.WebName,
        };

        HttpResponseMessage response = new(HttpStatusCode.OK) { Content = content };
        return Task.FromResult(response);
    }

    /// <summary>Повертає query-параметри останнього запиту.</summary>
    public IReadOnlyDictionary<string, string> GetLastQuery()
    {
        if (LastRequest?.RequestUri is null)
        {
            return new Dictionary<string, string>();
        }

        string query = LastRequest.RequestUri.Query.TrimStart('?');
        Dictionary<string, string> result = new(StringComparer.Ordinal);

        if (string.IsNullOrEmpty(query))
        {
            return result;
        }

        foreach (string pair in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            int separator = pair.IndexOf('=');
            if (separator < 0)
            {
                continue;
            }

            string key = WebUtility.UrlDecode(pair[..separator]);
            string value = WebUtility.UrlDecode(pair[(separator + 1)..]);
            result[key] = value;
        }

        return result;
    }
}
