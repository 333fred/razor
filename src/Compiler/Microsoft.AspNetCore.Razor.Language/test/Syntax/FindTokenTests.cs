// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language.Test;

public class FindTokenTests
{
    private const string PositionMarker = "$$";

    private (RazorSyntaxTree Tree, int Position) ParseWithPosition(string textWithPosition)
    {
        var position = textWithPosition.IndexOf(PositionMarker, StringComparison.Ordinal);
        if (position == -1)
        {
            throw new ArgumentException("The text must contain a '$$' character to indicate the position to find the token at.", nameof(textWithPosition));
        }

        var text = textWithPosition.Remove(position, PositionMarker.Length);
        var tree = RazorSyntaxTree.Parse(new StringSourceDocument(text, System.Text.Encoding.Default, RazorSourceDocumentProperties.Default));
        return (tree, position);
    }

    [Fact]
    public void ReturnsEofOnFileEnd()
    {
        var text = "<div></div>$$";
        var (tree, position) = ParseWithPosition(text);

        var token = tree.Root.FindToken(position);

        AssertEx.AssertEqualToleratingWhitespaceDifferences("""EndOfFile;[];""", SyntaxSerializer.Serialize(token));
    }

    [Fact]
    public void ReturnsOpenAngle()
    {
        var text = "$$<div></div>";
        var (tree, position) = ParseWithPosition(text);

        var token = tree.Root.FindToken(position);

        AssertEx.AssertEqualToleratingWhitespaceDifferences("""OpenAngle;[<];""", SyntaxSerializer.Serialize(token));
    }

    [Theory]
    [InlineData("<$$div></div>")]
    [InlineData("<d$$iv></div>")]
    [InlineData("<di$$v></div>")]
    public void ReturnsStartDivTag(string text)
    {
        var (tree, position) = ParseWithPosition(text);

        var token = tree.Root.FindToken(position);

        AssertEx.AssertEqualToleratingWhitespaceDifferences("""Text;[div];""", SyntaxSerializer.Serialize(token));
    }

    [Fact]
    public void ReturnsCloseAngle()
    {
        var text = "<div$$></div>";
        var (tree, position) = ParseWithPosition(text);

        var token = tree.Root.FindToken(position);

        AssertEx.AssertEqualToleratingWhitespaceDifferences("""CloseAngle;[>];""", SyntaxSerializer.Serialize(token));
    }

    [Fact]
    public void CSharpTransition_01()
    {
        var text = """
        $$@if (true)
        {
        }
        """;
        var (tree, position) = ParseWithPosition(text);

        var token = tree.Root.FindToken(position);

        AssertEx.AssertEqualToleratingWhitespaceDifferences("""Transition;[@];""", SyntaxSerializer.Serialize(token));
    }

    [Fact]
    public void CSharpTransition_02()
    {
        var text = """
        @$$if (true)
        {
        }
        """;
        var (tree, position) = ParseWithPosition(text);

        var token = tree.Root.FindToken(position);

        AssertEx.AssertEqualToleratingWhitespaceDifferences("""Keyword;[if];""", SyntaxSerializer.Serialize(token));
    }

    [Fact]
    public void CSharpTransition_03_IgnoreWhitespace()
    {
        var text = """
        @if$$ (true)
        {
        }
        """;
        var (tree, position) = ParseWithPosition(text);

        var token = tree.Root.FindToken(position);

        AssertEx.AssertEqualToleratingWhitespaceDifferences("""Keyword;[if];""", SyntaxSerializer.Serialize(token));
    }

    [Fact]
    public void CSharpTransition_03_IncludeWhitespace()
    {
        var text = """
        @if$$ (true)
        {
        }
        """;
        var (tree, position) = ParseWithPosition(text);

        var token = tree.Root.FindToken(position, includeWhitespace: true);

        AssertEx.AssertEqualToleratingWhitespaceDifferences("""Whitespace;[ ];""", SyntaxSerializer.Serialize(token));
    }

    [Fact]
    public void IgnoreWhitespace_BeforeNewline()
    {
        var text = """
        <div>    $$

        </div>
        """;
        var (tree, position) = ParseWithPosition(text);

        var token = tree.Root.FindToken(position, includeWhitespace: false);

        AssertEx.AssertEqualToleratingWhitespaceDifferences("""Whitespace;[ ];""", SyntaxSerializer.Serialize(token));
    }

    [Fact]
    public void IgnoreWhitespace_AfterNewline()
    {
        var text = """
        <div>    
        $$
        </div>
        """;
        var (tree, position) = ParseWithPosition(text);

        var token = tree.Root.FindToken(position, includeWhitespace: false);

        AssertEx.AssertEqualToleratingWhitespaceDifferences("""Whitespace;[ ];""", SyntaxSerializer.Serialize(token));
    }
}
