// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorageCore.Models.Apps;
using TgStorageCore.Models.Documents;
using TgStorageCore.Models.Messages;
using TgStorageCore.Models.Sources;
using TgStorageCore.Models.SourcesSettings;
using TgStorageCore.Utils;

namespace TgStorageCore.Helpers;

public partial class TgStorageHelper
{
    #region Public and private methods

    public void AddOrUpdateRecordApp(string apiHash, string phoneNumber, bool isUseUpdate)
    {
        if (string.IsNullOrEmpty(apiHash) && string.IsNullOrEmpty(phoneNumber)) return;
        TableAppModel item = GetRecord<TableAppModel>();
        if (!IsValid(item))
        {
            item = new(apiHash, phoneNumber);
            if (IsValid(item))
                SqLiteCon.Insert(item);
        }
        else if (isUseUpdate)
        {
            item.ApiHash = apiHash;
            item.PhoneNumber = phoneNumber;
            if (IsValid(item))
                SqLiteCon.Update(item);
        }
    }

    public void AddOrUpdateRecordMessage(long? id, long? sourceId, DateTime dtCreate, string message, string type, long size, bool isUseUpdate)
    {
        if (id is not { } lid) return;
        if (sourceId is not { } sid) return;
        TableMessageModel item = GetRecord<TableMessageModel>(id, sid);
        message = FixMessageString(item.SourceId, message);

        if (!IsValid(item))
        {
            item = new(lid, sid, dtCreate, message, type, size);
            item.Message = FixMessageString(item.SourceId, item.Message);
            if (IsValid(item))
                SqLiteCon.Insert(item);
        }
        else if (isUseUpdate)
        {
            //if (!Equals(message, item.Message))
            {
                item.DtCreate = dtCreate;
                item.Message = message;
                item.Type = type;
                item.Size = size;
                if (IsValid(item))
                {
                    // This table hasn't primary key.
                    SqLiteCon.Execute($"DELETE FROM MESSAGES WHERE ID = {item.Id} AND SOURCE_ID = {item.SourceId}");
                    SqLiteCon.Insert(item);
                }
            }
        }
    }

    public void AddOrUpdateRecordDocument(long? id, long? sourceId, long? messageId, string fileName, long fileSize, long accessHash, bool isUseUpdate)
    {
        if (id is not { } lid) return;
        if (sourceId is not { } sid) return;
        if (messageId is not { } mid) return;
        TableDocumentModel item = GetRecord<TableDocumentModel>(id, sid, mid);
        if (!IsValid(item))
        {
            item = new(lid, sid, mid, fileName, fileSize, accessHash);
            if (IsValid(item))
                SqLiteCon.Insert(item);
        }
        else if (isUseUpdate)
        {
            if (!Equals(fileName, item.FileName) || !Equals(fileSize, item.FileSize) || !Equals(accessHash, item.AccessHash))
            {
                item.FileName = fileName;
                item.FileSize = fileSize;
                item.AccessHash = accessHash;
                if (IsValid(item))
                {
                    // This table hasn't primary key.
                    SqLiteCon.Execute($"DELETE FROM DOCUMENTS WHERE ID = {id} AND SOURCE_ID = {sourceId} AND MESSAGE_ID = {messageId}");
                    SqLiteCon.Insert(item);
                }
            }
        }
    }

    public void AddOrUpdateRecordSource(long? id, string userName, string title, string about, int count, bool isUseUpdate)
    {
        if (id is not { } lid) return;
        TableSourceModel item = GetRecord<TableSourceModel>(id);
        if (!IsValid(item))
        {
            item = new(lid, userName, title, about, count);
            if (IsValid(item))
                SqLiteCon.Insert(item);
        }
        else if (isUseUpdate)
        {
            item.UserName = userName;
            item.Title = title;
            item.About = about;
            item.Count = count;
            if (IsValid(item))
            {
                SqLiteCon.Delete(item);
                SqLiteCon.Insert(item);
            }
        }
    }

    public void AddOrUpdateRecordSourceSetting(long? sourceId, string directory, int firstId, bool isUseUpdate)
    {
        if (sourceId is not { } sid) return;
        TableSourceSettingModel item = GetRecord<TableSourceSettingModel>(null, sourceId);
        if (!IsValid(item))
        {
            item = new(sid, directory, firstId, false);
            if (IsValid(item))
                SqLiteCon.Insert(item);
        }
        else if (isUseUpdate)
        {
            item.FirstId = firstId;
            item.Directory = directory;
            if (IsValid(item))
            {
                SqLiteCon.Update(item);
            }
        }
    }

    public T GetRecord<T>(long? firstId = null, long? secondId = null, long? thirdId = null) where T : TableBase, new()
    {
        InitSqLiteCon();
        List<T>? items = null;
        switch (typeof(T))
        {
            case var cls when cls == typeof(TableAppModel):
                items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.Apps}");
                break;
            case var cls when cls == typeof(TableDocumentModel):
                items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.Documents} WHERE ID = {firstId} AND SOURCE_ID = {secondId} AND MESSAGE_ID = {thirdId}");
                break;
            case var cls when cls == typeof(TableMessageModel):
                items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.Messages} WHERE ID = {firstId} AND SOURCE_ID = {secondId}");
                break;
            case var cls when cls == typeof(TableSourceModel):
                items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.Sources} WHERE ID = {firstId}");
                break;
            case var cls when cls == typeof(TableSourceSettingModel):
                if (firstId is not null)
                    items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.SourcesSettings} WHERE ID = {firstId}");
                else if (secondId is not null)
                    items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.SourcesSettings} WHERE SOURCE_ID = {secondId}");
                break;
        }
        if (items is null || items.Count == 0) return new();
        return items.First();
    }

    public List<T> GetRecords<T>(long? firstId = null, long? secondId = null, long? thirdId = null) where T : TableBase, new()
    {
        InitSqLiteCon();
        List<T>? items = null;
        switch (typeof(T))
        {
            case var cls when cls == typeof(TableAppModel):
                items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.Apps}");
                break;
            case var cls when cls == typeof(TableDocumentModel):
                items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.Documents} WHERE ID = {firstId} AND SOURCE_ID = {secondId} AND MESSAGE_ID = {thirdId}");
                break;
            case var cls when cls == typeof(TableMessageModel):
                items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.Messages} WHERE ID = {firstId} AND SOURCE_ID = {secondId}");
                break;
            case var cls when cls == typeof(TableSourceModel):
                items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.Sources} WHERE ID = {firstId}");
                break;
            case var cls when cls == typeof(TableSourceSettingModel):
                if (firstId is null && secondId is null && thirdId is null)
                    items = SqLiteCon.Query<T>($@"
SELECT * FROM [{TableNamesUtils.SourcesSettings}] [SS]
JOIN [{TableNamesUtils.Sources}] [S] ON [SS].[SOURCE_ID] = [S].[ID]
ORDER BY [S].[USER_NAME]
                    ".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t').Replace(Environment.NewLine, " "));
                else if (firstId is not null)
                    items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.SourcesSettings} WHERE ID = {firstId}");
                else if (secondId is not null)
                    items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.SourcesSettings} WHERE SOURCE_ID = {secondId}");
                break;
        }
        if (items is null || items.Count == 0) return new();
        return items;
    }

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

    public bool IsValid<T>(T item) where T : TableBase, new()
    {
        ValidationResult? validationResult = item switch
        {
            TableAppModel app => new TableAppValidator().Validate(app),
            TableDocumentModel document => new TableDocumentValidator().Validate(document),
            TableMessageModel message => new TableMessageValidator().Validate(message),
            TableSourceModel source => new TableSourceValidator().Validate(source),
            TableSourceSettingModel sourceSetting => new TableSourceSettingValidator().Validate(sourceSetting),
            _ => null
        };
        return validationResult?.IsValid ?? false;
    }

    #endregion
}