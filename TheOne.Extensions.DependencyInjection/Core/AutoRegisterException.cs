using System;

namespace TheOne.Extensions.DependencyInjection.Core;

public class AutoRegisterException(string message, Exception? innerException = null) : Exception(message, innerException);