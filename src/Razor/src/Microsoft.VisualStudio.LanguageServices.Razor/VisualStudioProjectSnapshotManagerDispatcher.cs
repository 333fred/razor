﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.VisualStudio.Razor;

[Export(typeof(ProjectSnapshotManagerDispatcher))]
[method: ImportingConstructor]
internal class VisualStudioProjectSnapshotManagerDispatcher(ILoggerFactory loggerFactory)
    : ProjectSnapshotManagerDispatcher(loggerFactory)
{
}
