using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Dekanat.ScheduleSdk.Enums;
using Dekanat.ScheduleSdk.Exceptions;
using Dekanat.ScheduleSdk.Internal;
using Dekanat.ScheduleSdk.Json;
using Dekanat.ScheduleSdk.Models;
using Dekanat.ScheduleSdk.Options;
using Dekanat.ScheduleSdk.Requests;

namespace Dekanat.ScheduleSdk;

/// <summary>
/// Реалізація <see cref="IPsRozkladClient"/> на базі <see cref="HttpClient"/>.
/// </summary>
public sealed class PsRozkladClient : IPsRozkladClient
{
    static PsRozkladClient()
    {
        // Потрібно для TextEncodingMode.Windows1251 у .NET Core / .NET 5+.
        Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
    }

    private readonly HttpClient _httpClient;
    private readonly PsRozkladClientOptions _options;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Створює клієнт з налаштованим <see cref="HttpClient"/> (використовуйте DI або <see cref="DependencyInjection.ServiceCollectionExtensions.AddPsRozkladClient"/>.
    /// </summary>
    /// <param name="httpClient">HTTP-клієнт з базовою адресою та таймаутом.</param>
    /// <param name="options">Опції SDK.</param>
    public PsRozkladClient(HttpClient httpClient, IOptions<PsRozkladClientOptions> options)
        : this(httpClient, options.Value)
    {
    }

    /// <summary>
    /// Створює клієнт без контейнера DI.
    /// </summary>
    public PsRozkladClient(HttpClient httpClient, PsRozkladClientOptions? options = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options ?? new PsRozkladClientOptions();
        _jsonOptions = PsRozkladJsonSerializerOptions.Default;

        if (_httpClient.BaseAddress is null)
        {
            _httpClient.BaseAddress = _options.BaseUrl;
        }

        if (_httpClient.Timeout == TimeSpan.FromSeconds(100))
        {
            _httpClient.Timeout = _options.RequestTimeout;
        }
    }

    /// <inheritdoc />
    public Task<PsRozkladExport> GetObjectListAsync(
        ObjectListRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        Dictionary<string, string> query = ApiQueryBuilder.CreateBaseQuery(
            request.Encoding ?? _options.Encoding);
        query["req_mode"] = ApiQueryBuilder.ToRequestMode(request.Mode);
        query["req_type"] = ApiQueryBuilder.ToRequestType(RequestType.ObjectList);

        if (request.IncludeIds)
        {
            query["show_ID"] = "yes";
        }

        return GetExportAsync(query, request.Encoding, cancellationToken);
    }

    /// <inheritdoc />
    public Task<PsRozkladExport> GetScheduleAsync(
        ScheduleRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateScheduleRequest(request);

        Dictionary<string, string> query = ApiQueryBuilder.CreateBaseQuery(
            request.Encoding ?? _options.Encoding);
        query["req_mode"] = ApiQueryBuilder.ToRequestMode(request.Mode);
        query["req_type"] = ApiQueryBuilder.ToRequestType(RequestType.Schedule);
        query["begin_date"] = ApiQueryBuilder.FormatApiDate(request.BeginDate);
        query["end_date"] = ApiQueryBuilder.FormatApiDate(request.EndDate);
        query["ros_text"] = ApiQueryBuilder.ToScheduleTextFormat(request.TextFormat);

        if (!string.IsNullOrWhiteSpace(request.ObjectId))
        {
            query["OBJ_ID"] = request.ObjectId;
        }

        if (!string.IsNullOrWhiteSpace(request.ObjectName))
        {
            query["OBJ_name"] = request.ObjectName;
        }

        if (!string.IsNullOrWhiteSpace(request.DepartmentName))
        {
            query["dep_name"] = request.DepartmentName;
        }

        if (request.ShowEmptyDays)
        {
            query["show_empty"] = "yes";
        }

        if (request.IncludeAllStreamComponents)
        {
            query["all_stream_components"] = "yes";
        }

        return GetExportAsync(query, request.Encoding, cancellationToken);
    }

    /// <inheritdoc />
    public Task<PsRozkladExport> GetFreeRoomsAsync(
        FreeRoomsListRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        Dictionary<string, string> query = ApiQueryBuilder.CreateBaseQuery(
            request.Encoding ?? _options.Encoding);
        query["req_mode"] = ApiQueryBuilder.ToRequestMode(RequestMode.Room);
        query["req_type"] = ApiQueryBuilder.ToRequestType(RequestType.FreeRoomsList);
        query["rooms_date"] = ApiQueryBuilder.FormatApiDate(request.Date);
        query["lesson"] = request.LessonNumber.ToString();

        if (!string.IsNullOrWhiteSpace(request.BuildingName))
        {
            query["block_name"] = request.BuildingName;
        }

        if (!string.IsNullOrWhiteSpace(request.RoomType))
        {
            query["room_type"] = request.RoomType;
        }

        if (request.MinimumCapacity is not null)
        {
            query["size_min"] = request.MinimumCapacity.Value.ToString();
        }

        if (request.MaximumCapacity is not null)
        {
            query["size_max"] = request.MaximumCapacity.Value.ToString();
        }

        return GetExportAsync(query, request.Encoding, cancellationToken);
    }

    /// <inheritdoc />
    public Task<PsRozkladExport> GetRoomTypesAsync(
        RoomTypeListRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        Dictionary<string, string> query = ApiQueryBuilder.CreateBaseQuery(
            request.Encoding ?? _options.Encoding);
        query["req_mode"] = ApiQueryBuilder.ToRequestMode(RequestMode.Room);
        query["req_type"] = ApiQueryBuilder.ToRequestType(RequestType.RoomTypeList);

        return GetExportAsync(query, request.Encoding, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PsRozkladResponse> SendAsync(
        IReadOnlyDictionary<string, string> queryParameters,
        TextEncodingMode? encodingOverride = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(queryParameters);

        TextEncodingMode encoding = encodingOverride ?? _options.Encoding;
        string requestUri = BuildRequestUri(queryParameters, encoding);
        Encoding responseEncoding = ResolveResponseEncoding(encoding);

        using HttpRequestMessage httpRequest = new(HttpMethod.Get, requestUri);
        using HttpResponseMessage httpResponse = await _httpClient
            .SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        httpResponse.EnsureSuccessStatusCode();

        await using Stream responseStream = await httpResponse.Content
            .ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);

        using StreamReader reader = new(responseStream, responseEncoding);
        string json = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);

        PsRozkladResponse? response = JsonSerializer.Deserialize<PsRozkladResponse>(json, _jsonOptions);
        if (response?.Export is null)
        {
            throw new InvalidOperationException("Відповідь API не містить об'єкта psrozklad_export.");
        }

        if (_options.ThrowOnApiError && !response.Export.IsSuccess)
        {
            throw new PsRozkladApiException(response.Export);
        }

        return response;
    }

    private async Task<PsRozkladExport> GetExportAsync(
        Dictionary<string, string> query,
        TextEncodingMode? encodingOverride,
        CancellationToken cancellationToken)
    {
        PsRozkladResponse response = await SendAsync(query, encodingOverride, cancellationToken)
            .ConfigureAwait(false);
        return response.Export!;
    }

    private static void ValidateScheduleRequest(ScheduleRequest request)
    {
        bool hasId = !string.IsNullOrWhiteSpace(request.ObjectId);
        bool hasName = !string.IsNullOrWhiteSpace(request.ObjectName);
        bool hasDepartment = !string.IsNullOrWhiteSpace(request.DepartmentName);
        int selectorCount = (hasId ? 1 : 0) + (hasName ? 1 : 0) + (hasDepartment ? 1 : 0);

        if (selectorCount != 1)
        {
            throw new ArgumentException(
                "Потрібно вказати рівно одне з: ObjectId, ObjectName або DepartmentName.",
                nameof(request));
        }

        if (request.EndDate < request.BeginDate)
        {
            throw new ArgumentException("EndDate не може бути раніше за BeginDate.", nameof(request));
        }
    }

    private static string BuildRequestUri(
        IReadOnlyDictionary<string, string> queryParameters,
        TextEncodingMode encoding)
    {
        Dictionary<string, string> merged = ApiQueryBuilder.CreateBaseQuery(encoding);

        foreach (KeyValuePair<string, string> pair in queryParameters)
        {
            merged[pair.Key] = pair.Value;
        }

        StringBuilder builder = new();
        builder.Append('?');
        bool first = true;

        foreach (KeyValuePair<string, string> pair in merged)
        {
            if (!first)
            {
                builder.Append('&');
            }

            builder.Append(WebUtility.UrlEncode(pair.Key));
            builder.Append('=');
            builder.Append(WebUtility.UrlEncode(pair.Value));
            first = false;
        }

        return builder.ToString();
    }

    private static Encoding ResolveResponseEncoding(TextEncodingMode encoding) =>
        encoding switch
        {
            TextEncodingMode.Utf8 => Encoding.UTF8,
            TextEncodingMode.Windows1251 => Encoding.GetEncoding(1251),
            _ => Encoding.UTF8,
        };
}
