using System;

namespace TheOne.Extensions.DependencyInjection.Core;

/// <summary>
/// The logger factory which allowing external applications register way to trace for details when registering dependencies
/// </summary>
public class LoggerFactory
{
    public delegate void LogMessage(string message);
    public delegate void LogMessageProvider(Func<string> message);

    public static LogMessage? LogInfo { get; set; }
    public static LogMessageProvider? LogTrace { get; set; }

    public static void Info(string message)
    {
        LogInfo?.Invoke(message);
    }

    public static void Trace(Func<string> msgProvider)
    {
        LogTrace?.Invoke(msgProvider);
    }
}