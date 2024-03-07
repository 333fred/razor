﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Xunit;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

public class CSharpLanguageCharacteristicsTest
{
    [Fact]
    public void GetSample_RightShiftAssign_ReturnsCorrectToken()
    {
        // Arrange & Act
        var token = CSharpLanguageCharacteristics.Instance.GetSample(SyntaxKind.CSharpOperator);

        // Assert
        Assert.Equal(">>=", token);
    }
}
