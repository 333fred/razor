﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal static class ILoggerExtensions
{
    public static bool TestOnlyLoggingEnabled = false;

    [Conditional("DEBUG")]
    public static void LogTestOnly(this ILogger logger, string message, params object?[] args)
    {
        if (TestOnlyLoggingEnabled)
        {
            // This is test-only, so we don't mind losing structured logging for it.
            logger.LogDebug($"{message}: {string.Join(",", args)}");
        }
    }
}
