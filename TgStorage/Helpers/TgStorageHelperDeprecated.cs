// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Models;
using TgStorage.Models.Documents;
using TgStorage.Models.Messages;
using TgStorage.Models.Sources;
using TgStorage.Models.SourcesSettings;
using TgStorage.Utils;

namespace TgStorage.Helpers;

public partial class TgStorageHelper
{
	#region Public and private methods

	[Obsolete(@"Deprecated method")]
	public void AddOrUpdateItemMessageDeprecated(long? id, long? sourceId, DateTime dtCreate, string message, string type, long size, bool isUseUpdate)
	{
		if (id is not { } lid) return;
		if (sourceId is not { } sid) return;
		SqlTableMessageModel item = GetItemDeprecated<SqlTableMessageModel>(id, sid);
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

	[Obsolete(@"Deprecated method")]
	public void AddOrUpdateItemDocumentDeprecated(long? id, long? sourceId, long? messageId, string fileName, long fileSize, long accessHash, bool isUseUpdate)
	{
		if (id is not { } lid) return;
		if (sourceId is not { } sid) return;
		if (messageId is not { } mid) return;
		SqlTableDocumentModel item = GetItemDeprecated<SqlTableDocumentModel>(id, sid, mid);
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

	[Obsolete(@"Deprecated method")]
	public void AddOrUpdateItemSourceDeprecated(long? id, string userName, string title, string about, int count, bool isUseUpdate)
	{
		if (id is not { } lid) return;
		SqlTableSourceModel item = GetItemDeprecated<SqlTableSourceModel>(id);
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

	[Obsolete(@"Deprecated method")]
	public void AddOrUpdateItemSourceSettingDeprecated(long? sourceId, string directory, int firstId, bool isAutoUpdate, bool isUseUpdate)
	{
		if (sourceId is not { } sid) return;
		SqlTableSourceSettingModel item = GetItemDeprecated<SqlTableSourceSettingModel>(null, sourceId);
		if (!IsValid(item))
		{
			item = new(sid, directory, firstId, isAutoUpdate);
			if (IsValid(item))
				SqLiteCon.Insert(item);
		}
		else if (isUseUpdate)
		{
			item.FirstId = firstId;
			item.Directory = directory;
			item.IsAutoUpdate = isAutoUpdate;
			if (IsValid(item))
			{
				SqLiteCon.Update(item);
			}
		}
	}

	[Obsolete(@"Use GetItem")]
	public T GetItemDeprecated<T>(long? firstId = null, long? secondId = null, long? thirdId = null) where T : SqlTableBase, new()
	{
		List<T>? items = null;
		switch (typeof(T))
		{
			case var cls when cls == typeof(SqlTableDocumentModel):
				items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.Documents} WHERE ID = {firstId} AND SOURCE_ID = {secondId} AND MESSAGE_ID = {thirdId}");
				break;
			case var cls when cls == typeof(SqlTableMessageModel):
				items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.Messages} WHERE ID = {firstId} AND SOURCE_ID = {secondId}");
				break;
			case var cls when cls == typeof(SqlTableSourceModel):
				items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.Sources} WHERE ID = {firstId}");
				break;
			case var cls when cls == typeof(SqlTableSourceSettingModel):
				if (firstId is not null)
					items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.SourcesSettings} WHERE ID = {firstId}");
				else if (secondId is not null)
					items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.SourcesSettings} WHERE SOURCE_ID = {secondId}");
				break;
		}
		if (items is null || items.Count == 0) return new();
		return items.First();
	}

	#endregion
}