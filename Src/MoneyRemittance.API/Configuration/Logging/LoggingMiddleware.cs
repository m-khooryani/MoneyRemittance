using Microsoft.IO;

namespace MoneyRemittance.API.Configuration.Logging;

internal class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

    public LoggingMiddleware(
        RequestDelegate next,
        ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
    }

    public async Task Invoke(HttpContext context)
    {
        Action<string, object[]> logAction = _logger.LogInformation;
        var correlationId = Guid.NewGuid();
        _logger.LogInformation("starting scope: {correlationId}", correlationId);
        using (_logger.BeginScope(correlationId))
        {
            try
            {
                await TryLogging(context, logAction);
            }
            catch (Exception ex)
            {
                _logger.LogError("HTTP request failed");
                _logger.LogError(ex.ToString());
            }
            finally
            {
                logAction(
                    "Response => {statusCode}",
                    new object[]
                    {
                        context.Response.StatusCode
                    });
            }
        }
    }

    private async Task TryLogging(HttpContext context, Action<string, object[]> logAction)
    {
        // enable buffering and streaming
        context.Request.EnableBuffering();
        var requestStream = _recyclableMemoryStreamManager.GetStream();
        await context.Request.Body.CopyToAsync(requestStream);

        // log
        LogRequest(context, logAction, requestStream);

        // reset stream position
        context.Request.Body.Position = 0;

        await _next(context);
    }

    private static void LogRequest(HttpContext context, Action<string, object[]> logAction, MemoryStream requestStream)
    {
        logAction(
            "Request from {ip} {method} {url}",
            new object[]
            {
                context.Connection?.RemoteIpAddress?.ToString()?? "",
                context.Request?.Method?? "",
                context.Request?.Path.Value?? ""
            });
        logAction(
            "QueryString {querystring}",
            new object[]
            {
                context.Request?.QueryString.ToString()?? "",
            });
        logAction(
            "Request Body {requestbody}",
            new object[]
            {
                ReadStreamInChunks(requestStream)
            });
    }

    private static string ReadStreamInChunks(Stream stream)
    {
        const int readChunkBufferLength = 4096;
        stream.Seek(0, SeekOrigin.Begin);
        using var textWriter = new StringWriter();
        using var reader = new StreamReader(stream);
        var readChunk = new char[readChunkBufferLength];
        int readChunkLength;
        do
        {
            readChunkLength = reader.ReadBlock(
                readChunk,
                0,
                readChunkBufferLength);
            textWriter.Write(readChunk, 0, readChunkLength);
        }
        while (readChunkLength > 0);
        return textWriter.ToString();
    }
}
