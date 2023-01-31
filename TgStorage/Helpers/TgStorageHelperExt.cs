// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo;
using TgStorage.Models;
using TgStorage.Models.Apps;
using TgStorage.Models.Documents;
using TgStorage.Models.Messages;
using TgStorage.Models.Proxies;
using TgStorage.Models.Sources;
using TgStorage.Models.SourcesSettings;
using TgStorage.Utils;

namespace TgStorage.Helpers;

public partial class TgStorageHelper : IHelper
{
    #region Public and private methods

    public string FixMessageString(long sourceId, string message)
    {
        if (message.StartsWith(sourceId.ToString()))
        {
            message = message.Substring(sourceId.ToString().Length + 1, message.Length - sourceId.ToString().Length - 1);
            message = message.TrimStart('[');
            message = message.TrimEnd(']');
        }

        if (message.StartsWith("> "))
        {
            message = message.Substring(2, message.Length - 2);
        }

        if (Equals(message, "(no message)"))
        {
            message = string.Empty;
        }

        return message;
    }

    public bool IsValid<T>(T item) where T : SqlTableBase, new()
    {
        ValidationResult? validationResult = item switch
        {
            SqlTableDocumentModel document => new SqlTableDocumentValidator().Validate(document),
            SqlTableMessageModel message => new SqlTableMessageValidator().Validate(message),
            SqlTableSourceModel source => new SqlTableSourceValidator().Validate(source),
            SqlTableSourceSettingModel sourceSetting => new SqlTableSourceSettingValidator().Validate(sourceSetting),
            _ => null
        };
        return validationResult?.IsValid ?? false;
    }

    public bool IsValidXpLite<T>(T item) where T : SqlTableXpLiteBase, new()
    {
        ValidationResult? validationResult = item switch
        {
            SqlTableAppModel app => new SqlTableAppValidator().Validate(app),
            SqlTableProxyModel proxy => new SqlTableProxyValidator().Validate(proxy),
            _ => null
        };
        return validationResult?.IsValid ?? false;
    }

    public List<ISqlTable> GetTableModels() => new()
    {
        new SqlTableAppModel(),
        new SqlTableProxyModel(),
    };

    public List<Type> GetTableTypes() => new()
    {
        typeof(SqlTableAppModel),
        typeof(SqlTableProxyModel),
    };

    #endregion
}