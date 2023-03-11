// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Models;
using TgStorage.Models.Documents;
using TgStorage.Models.Messages;
using TgStorage.Models.Sources;
using TgStorage.Models.SourcesSettings;
using TgStorage.Utils;

namespace TgStorage.Helpers;

public partial class TgStorageHelper : IHelper
{
	#region Public and private methods

	public List<T> GetList<T>(long? firstId = null, long? secondId = null, long? thirdId = null) where T : SqlTableBase, new()
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
				if (firstId is null && secondId is null && thirdId is null)
				{
					string query = $@"
SELECT * FROM [{TableNamesUtils.SourcesSettings}] [SS]
JOIN [{TableNamesUtils.Sources}] [S] ON [SS].[SOURCE_ID] = [S].[ID]
ORDER BY [S].[USER_NAME]
                    ".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t').Replace(Environment.NewLine, " ");
					items = SqLiteCon.Query<T>(query);
				}
				else if (firstId is not null)
					items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.SourcesSettings} WHERE ID = {firstId}");
				else if (secondId is not null)
					items = SqLiteCon.Query<T>($"SELECT * FROM {TableNamesUtils.SourcesSettings} WHERE SOURCE_ID = {secondId}");
				break;
		}
		if (items is null || items.Count == 0) return new();
		return items;
	}

	#endregion
}