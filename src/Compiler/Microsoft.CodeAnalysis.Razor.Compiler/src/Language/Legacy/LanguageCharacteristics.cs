// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language.Syntax.InternalSyntax;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

internal abstract class LanguageCharacteristics<TTokenizer>
    where TTokenizer : Tokenizer
{
    public abstract string GetSample(SyntaxKind type);
    public abstract TTokenizer CreateTokenizer(SeekableTextReader source);
    public abstract SyntaxKind FlipBracket(SyntaxKind bracket);
    public abstract SyntaxToken CreateMarkerToken();

    public virtual IEnumerable<SyntaxToken> TokenizeString(string content)
    {
        return TokenizeString(SourceLocation.Zero, content);
    }

    public virtual IEnumerable<SyntaxToken> TokenizeString(SourceLocation start, string input)
    {
        using (var reader = new SeekableTextReader(input, start.FilePath))
        {
            var tok = CreateTokenizer(reader);
            SyntaxToken? token;
            while ((token = tok.NextToken()) != null)
            {
                yield return token;
            }
        }
    }

    public virtual bool IsWhitespace([NotNullWhen(true)] SyntaxToken? token)
    {
        return IsKnownTokenType(token, KnownTokenType.Whitespace);
    }

    public virtual bool IsNewLine([NotNullWhen(true)] SyntaxToken? token)
    {
        return IsKnownTokenType(token, KnownTokenType.NewLine);
    }

    public virtual bool IsIdentifier([NotNullWhen(true)] SyntaxToken? token)
    {
        return IsKnownTokenType(token, KnownTokenType.Identifier);
    }

    public virtual bool IsKeyword([NotNullWhen(true)] SyntaxToken? token)
    {
        return IsKnownTokenType(token, KnownTokenType.Keyword);
    }

    public virtual bool IsTransition([NotNullWhen(true)] SyntaxToken? token)
    {
        return IsKnownTokenType(token, KnownTokenType.Transition);
    }

    public virtual bool IsCommentStart([NotNullWhen(true)] SyntaxToken? token)
    {
        return IsKnownTokenType(token, KnownTokenType.CommentStart);
    }

    public virtual bool IsCommentStar([NotNullWhen(true)] SyntaxToken? token)
    {
        return IsKnownTokenType(token, KnownTokenType.CommentStar);
    }

    public virtual bool IsCommentBody([NotNullWhen(true)] SyntaxToken? token)
    {
        return IsKnownTokenType(token, KnownTokenType.CommentBody);
    }

    public virtual bool IsUnknown([NotNullWhen(true)] SyntaxToken? token)
    {
        return IsKnownTokenType(token, KnownTokenType.Unknown);
    }

    public virtual bool IsKnownTokenType([NotNullWhen(true)] SyntaxToken? token, KnownTokenType type)
    {
        return token != null && (token.Kind == GetKnownTokenType(type));
    }

    public virtual (SyntaxToken left, SyntaxToken? right) SplitToken(SyntaxToken token, int splitAt, SyntaxKind leftType)
    {
        var left = CreateToken(token.Content[..splitAt], leftType, []);

        SyntaxToken? right = null;
        if (splitAt < token.Content.Length)
        {
            right = CreateToken(token.Content[splitAt..], token.Kind, token.GetDiagnostics());
        }

        return (left, right);
    }

    public abstract SyntaxKind GetKnownTokenType(KnownTokenType type);

    public virtual bool KnowsTokenType(KnownTokenType type)
    {
        return type == KnownTokenType.Unknown || (GetKnownTokenType(type) != GetKnownTokenType(KnownTokenType.Unknown));
    }

    protected abstract SyntaxToken CreateToken(string content, SyntaxKind type, RazorDiagnostic[] errors);
}
