﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting
{
    internal enum FormattingSpanKind
    {
        Transition,
        MetaCode,
        Comment,
        Code,
        Markup,
        None
    }
}