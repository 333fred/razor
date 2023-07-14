// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static class PooledArrayBuilderExtensions
{
    public static void Push<T>(this PooledArrayBuilder<T> builder, T item)
    {
        builder.Add(item);
    }
    
    public static T Peek<T>(this PooledArrayBuilder<T> builder)
    {
        return builder[^1];
    }
    
    public static T Pop<T>(this PooledArrayBuilder<T> builder)
    {
        var item = builder[^1];
        builder.RemoveAt(builder.Count - 1);
        return item;
    }
    
    public static bool TryPop<T>(this PooledArrayBuilder<T> builder, [MaybeNullWhen(false)] out T item)
    {
        if (builder.Count == 0)
        {
            item = default;
            return false;
        }
        
        item = builder.Pop();
        return true;
    }
}
