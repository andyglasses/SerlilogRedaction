namespace Serilog_redaction;
using Microsoft.Extensions.Logging;

internal static partial class LoggedMessages
{
    [LoggerMessage(Level = LogLevel.Warning, Message = "Successfully Processed Message about: {Name}")]
    internal static partial void ProcessedMessage(
        ILogger logger,
        string name,
        [LogProperties] Payload payload);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Successfully Processed Message2: {@Payload}")]
    internal static partial void ProcessedMessage2(
        ILogger logger,
        Payload payload);
}
