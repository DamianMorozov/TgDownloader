// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.ComponentModel;
using TgCore.Interfaces;

namespace TgCore.Utils;

public static class ExtUtils
{
    #region Public and private methods - Core

    private static object? GetPropertyDefaultValueCore<TItem>(this TItem item, string name)
    {
        if (item is null) return null;
        AttributeCollection? attributes = TypeDescriptor.GetProperties(item)[name]?.Attributes;
        Attribute? attribute = attributes?[typeof(DefaultValueAttribute)];
        if (attribute is DefaultValueAttribute defaultValueAttribute)
            return defaultValueAttribute.Value;
        return null;
    }

    private static string GetPropertyDefaultValueAsStringCore<TItem>(this TItem item, string name) =>
        GetPropertyDefaultValueCore(item, name)?.ToString() ?? string.Empty;

    private static int GetPropertyDefaultValueAsIntCore<TItem>(this TItem item, string name) =>
        GetPropertyDefaultValueCore(item, name) is int value ? value : default;

    private static long GetPropertyDefaultValueAsLongCore<TItem>(this TItem item, string name) =>
        GetPropertyDefaultValueCore(item, name) is long value ? value : default;

    private static bool GetPropertyDefaultValueAsBoolCore<TItem>(this TItem item, string name) =>
        GetPropertyDefaultValueCore(item, name) is bool value ? value : default;

    private static TResult? GetPropertyDefaultValueAsGenericCore<TResult, TItem>(this TItem item, string name) =>
        GetPropertyDefaultValueCore(item, name) is TResult value ? value : default;

    #endregion

    #region Public and private methods - IBase

    public static object? GetPropertyDefaultValue(this IBase item, string name) =>
        GetPropertyDefaultValueCore(item, name);

    public static string GetPropertyDefaultValueAsString(this IBase item, string name) =>
        GetPropertyDefaultValueAsStringCore(item, name);

    public static int GetPropertyDefaultValueAsInt(this IBase item, string name) =>
        GetPropertyDefaultValueAsIntCore(item, name);

    public static long GetPropertyDefaultValueAsLong(this IBase item, string name) =>
        GetPropertyDefaultValueAsLongCore(item, name);

    public static bool GetPropertyDefaultValueAsBool(this IBase item, string name) =>
        GetPropertyDefaultValueAsBoolCore(item, name);

    public static TResult? GetPropertyDefaultValueAsGeneric<TResult>(this IBase item, string name) =>
        GetPropertyDefaultValueAsGenericCore<TResult, IBase>(item, name);

    #endregion
}