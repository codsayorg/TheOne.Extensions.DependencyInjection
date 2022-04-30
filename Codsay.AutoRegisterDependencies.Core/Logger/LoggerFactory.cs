using System;

namespace Codsay.AutoRegisterDependencies.Core.Logger
{
    /// <summary>
    /// Minimal logger to trace for details when registering dependencies
    /// </summary>
    public interface IAutoRegisterLogger
    {
        /// <summary>
        /// Log a trace message
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);

        /// <summary>
        /// Log a trace message
        /// </summary>
        /// <param name="messageBuilder"></param>
        void Trace(Func<string> messageBuilder);
    }

    /// <summary>
    /// The logger factory which allowing external applications register way to trace for details when registering dependencies
    /// </summary>
    public class LoggerFactory
    {
        /// <summary>
        /// Set the logger factory
        /// </summary>
        public static IAutoRegisterLogger Logger { get; set; }

        /// <summary>
        /// Trace message
        /// </summary>
        /// <param name="messageBuilder"></param>
        public static void Info(string message)
        {
            if (Logger != null)
            {
                Logger.Info(message);
            }
        }

        /// <summary>
        /// Trace message
        /// </summary>
        /// <param name="messageBuilder"></param>
        public static void Trace(Func<string> messageBuilder)
        {
            if (Logger != null)
            {
                Logger.Trace(messageBuilder);
            }
        }
    }
}
