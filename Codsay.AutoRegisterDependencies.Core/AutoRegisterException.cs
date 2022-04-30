using System;

namespace Codsay.AutoRegisterDependencies.Core
{
    /// <summary>
    /// General exception
    /// </summary>
    public class AutoRegisterException : Exception
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        public AutoRegisterException(string message) : this(message, innerException: null)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public AutoRegisterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
