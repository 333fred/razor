﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal interface IRazorFormattingService
{
    Task<TextEdit[]> GetDocumentFormattingEditsAsync(
       VersionedDocumentContext documentContext,
       TextEdit[] htmlEdits,
       Range? range,
       RazorFormattingOptions options,
       CancellationToken cancellationToken);

    Task<TextEdit[]> GetHtmlOnTypeFormattingEditsAsync(
      DocumentContext documentContext,
      TextEdit[] htmlEdits,
      RazorFormattingOptions options,
      int hostDocumentIndex,
      char triggerCharacter,
      CancellationToken cancellationToken);

    Task<TextEdit[]> GetCSharpOnTypeFormattingEditsAsync(
      DocumentContext documentContext,
      RazorFormattingOptions options,
      int hostDocumentIndex,
      char triggerCharacter,
      CancellationToken cancellationToken);

    Task<TextEdit?> GetSingleCSharpEditAsync(
        DocumentContext documentContext,
        TextEdit initialEdit,
        RazorFormattingOptions options,
        CancellationToken cancellationToken);

    Task<TextEdit?> GetCSharpCodeActionEditAsync(
       DocumentContext documentContext,
       TextEdit[] formattedEdits,
       RazorFormattingOptions options,
       CancellationToken cancellationToken);

    Task<TextEdit?> GetCSharpSnippetFormattingEditAsync(
       DocumentContext documentContext,
       TextEdit[] edits,
       RazorFormattingOptions options,
       CancellationToken cancellationToken);
}
