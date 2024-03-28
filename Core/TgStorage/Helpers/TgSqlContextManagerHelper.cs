// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Helpers;

/// <summary>
/// SQL data storage context helper.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgSqlContextManagerHelper : ITgHelper
{
	#region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	private static TgSqlContextManagerHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public static TgSqlContextManagerHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

	#region Public and private fields, properties, constructor

	public TgSqlTableAppRepository AppRepository => TgSqlTableAppRepository.Instance;

    public TgSqlTableDocumentRepository DocumentRepository => TgSqlTableDocumentRepository.Instance;

    public TgSqlTableFilterRepository FilterRepository => TgSqlTableFilterRepository.Instance;

    public TgSqlTableMessageRepository MessageRepository => TgSqlTableMessageRepository.Instance;

    public TgSqlTableProxyRepository ProxyRepository => TgSqlTableProxyRepository.Instance;

    public TgSqlTableSourceRepository SourceRepository => TgSqlTableSourceRepository.Instance;

    public TgSqlTableVersionRepository VersionRepository => TgSqlTableVersionRepository.Instance;

    public TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;

    public TgLogHelper TgLog => TgLogHelper.Instance;

    public bool IsReady =>
        TgAppSettings.AppXml.IsExistsFileStorage &&
        IsTableExists(TgSqlConstants.TableApps) && IsTableExists(TgSqlConstants.TableDocuments) &&
        IsTableExists(TgSqlConstants.TableFilters) && IsTableExists(TgSqlConstants.TableMessages) &&
        IsTableExists(TgSqlConstants.TableProxies) && IsTableExists(TgSqlConstants.TableSources) &&
        IsTableExists(TgSqlConstants.TableVersions);

    public bool IsNotReady => !IsReady;

    #endregion

    #region Public and private methods

    public string ToDebugString() => $"{TgCommonUtils.GetIsReady(IsReady)}";

    public (bool IsSuccess, string FileName) BackupDb()
	{
		if (File.Exists(TgAppSettings.AppXml.FileStorage))
		{
			DateTime dt = DateTime.Now;
			string fileBackup = $"{Path.GetDirectoryName(TgAppSettings.AppXml.FileStorage)}\\{Path.GetFileNameWithoutExtension(TgAppSettings.AppXml.FileStorage)}_{dt:yyyyMMdd}_{dt:HHmmss}.bak";
			File.Copy(TgAppSettings.AppXml.FileStorage, fileBackup);
			return (File.Exists(fileBackup), fileBackup);
		}
		return (false, string.Empty);
	}

	public void CreateOrConnectDb(bool isCheckTables)
	{
        TgSqlUtils.SetXpoDefault();

        if (isCheckTables)
			CheckTables();
	}

	public void DeleteExistsDb()
	{
		if (IsNotReady)
			return;
		File.Delete(TgAppSettings.AppXml.FileStorage);
	}

	public void VersionsView()
	{
		IEnumerable<TgSqlTableVersionModel> versions = VersionRepository.GetEnumerable();
		foreach (TgSqlTableVersionModel version in versions)
		{
			TgLog.WriteLine($" {version.Version:00} | {version.Description}");
		}
	}

	public void FiltersView()
	{
		IEnumerable<TgSqlTableFilterModel> filters = FilterRepository.GetEnumerable();
		foreach (TgSqlTableFilterModel filter in filters)
		{
			TgLog.WriteLine(filter.ToString());
		}
	}

	/// <summary>
	/// Update structures of tables.
	/// </summary>
	private void CheckTables()
	{
		if (!CheckTableApps())
			throw new(TgLocaleHelper.Instance.TablesAppsException);
		if (!CheckTableDocuments())
			throw new(TgLocaleHelper.Instance.TablesDocumentsException);
		if (!CheckTableFilters())
			throw new(TgLocaleHelper.Instance.TablesFiltersException);
		if (!CheckTableMessages())
			throw new(TgLocaleHelper.Instance.TablesMessagesException);
		if (!CheckTableSources())
			throw new(TgLocaleHelper.Instance.TablesSourcesException);
		if (!CheckTableProxies())
			throw new(TgLocaleHelper.Instance.TablesProxiesException);
		if (!CheckTableVersions())
			throw new(TgLocaleHelper.Instance.TablesVersionsException);
		FillTableVersions();
	}

	public bool CheckTableApps()
	{
		bool result = true;
		TgSqlTableAppModel itemNew = AppRepository.GetNewAsync(true).GetAwaiter().GetResult();
		if (!AppRepository.SaveAsync(itemNew).GetAwaiter().GetResult())
			result = false;
		if (!AppRepository.DeleteAsync(itemNew).GetAwaiter().GetResult())
			result = false;
		return result;
	}

	public bool CheckTableDocuments()
	{
		bool result = true;
		TgSqlTableDocumentModel itemNew = DocumentRepository.GetNewAsync(true).Result;
		if (!DocumentRepository.SaveAsync(itemNew).GetAwaiter().GetResult())
			result = false;
		if (!DocumentRepository.DeleteAsync(itemNew).GetAwaiter().GetResult())
			result = false;
		return result;
	}

	public bool CheckTableFilters()
	{
		bool result = true;
		TgSqlTableFilterModel itemNew = FilterRepository.GetNewAsync(false).Result;
		if (!FilterRepository.SaveAsync(itemNew).GetAwaiter().GetResult())
		    result = false;
		if (!FilterRepository.DeleteAsync(itemNew).GetAwaiter().GetResult())
			result = false;
		return result;
	}

	public bool CheckTableMessages()
	{
		bool result = true;
		TgSqlTableMessageModel itemNew = MessageRepository.GetNewAsync(true).Result;
		if (!MessageRepository.SaveAsync(itemNew).GetAwaiter().GetResult())
			result = false;
		if (!MessageRepository.DeleteAsync(itemNew).GetAwaiter().GetResult())
			result = false;
		return result;
	}

	public bool CheckTableProxies()
    {
        bool result = true;
        TgSqlTableProxyModel itemNew = ProxyRepository.GetNewAsync(true).Result;
		if (!ProxyRepository.SaveAsync(itemNew).GetAwaiter().GetResult())
			result = false;
        if (!ProxyRepository.DeleteAsync(itemNew).GetAwaiter().GetResult())
            result = false;
		return result;
	}

	public bool CheckTableSources()
	{
		bool result = true;
		TgSqlTableSourceModel itemNew = SourceRepository.GetNewAsync(true).Result;
		if (!SourceRepository.SaveAsync(itemNew).GetAwaiter().GetResult())
			result = false;
		if (!SourceRepository.DeleteAsync(itemNew).GetAwaiter().GetResult())
			result = false;
		return result;
	}

	public bool CheckTableVersions()
	{
		bool result = true;
		TgSqlTableVersionModel itemNew = VersionRepository.GetNewAsync(true).GetAwaiter().GetResult();
		if (!VersionRepository.SaveAsync(itemNew).GetAwaiter().GetResult())
			result = false;
		if (!VersionRepository.DeleteAsync(itemNew).GetAwaiter().GetResult())
			result = false;
		return result;
	}

	public void FillTableVersions()
	{
		bool isLast = false;
		while (!isLast)
		{
			TgSqlTableVersionModel versionLast = !IsTableExists(TgSqlConstants.TableVersions) 
                ? new() : VersionRepository.GetItemLastAsync().Result;
			if (Equals(versionLast.Version, short.MaxValue))
				versionLast.Version = 0;
			switch (versionLast.Version)
			{
				case 0:
					TgSqlTableVersionModel version1 = new() { Version = 1, Description = "Added versions table" };
					VersionRepository.SaveAsync(version1).GetAwaiter().GetResult();
					break;
				case 1:
					TgSqlTableVersionModel version2 = new() { Version = 2, Description = "Added apps table" };
					VersionRepository.SaveAsync(version2).GetAwaiter().GetResult();
					break;
				case 2:
					TgSqlTableVersionModel version3 = new() { Version = 3, Description = "Added documents table" };
					VersionRepository.SaveAsync(version3).GetAwaiter().GetResult();
					break;
				case 3:
					TgSqlTableVersionModel version4 = new() { Version = 4, Description = "Added filters table" };
					VersionRepository.SaveAsync(version4).GetAwaiter().GetResult();
					break;
				case 4:
					TgSqlTableVersionModel version5 = new() { Version = 5, Description = "Added messages table" };
					VersionRepository.SaveAsync(version5).GetAwaiter().GetResult();
					break;
				case 5:
					TgSqlTableVersionModel version6 = new() { Version = 6, Description = "Added proxies table" };
					VersionRepository.SaveAsync(version6).GetAwaiter().GetResult();
					break;
				case 6:
					TgSqlTableVersionModel version7 = new() { Version = 7, Description = "Added sources table" };
					VersionRepository.SaveAsync(version7).GetAwaiter().GetResult();
					break;
				case 7:
					TgSqlTableVersionModel version8 = new() { Version = 8, Description = "Added source settings table" };
					VersionRepository.SaveAsync(version8).GetAwaiter().GetResult();
					break;
				case 8:
					TgSqlTableVersionModel version9 = new() { Version = 9, Description = "Upgrade versions table" };
					VersionRepository.SaveAsync(version9).GetAwaiter().GetResult();
					break;
				case 9:
					TgSqlTableVersionModel version10 = new() { Version = 10, Description = "Upgrade apps table" };
					VersionRepository.SaveAsync(version10).GetAwaiter().GetResult();
					break;
				case 10:
					TgSqlTableVersionModel version11 = new() { Version = 11, Description = "Upgrade storage on XPO framework" };
					VersionRepository.SaveAsync(version11).GetAwaiter().GetResult();
					break;
				case 11:
					TgSqlTableVersionModel version12 = new() { Version = 12, Description = "Upgrade apps table" };
					VersionRepository.SaveAsync(version12).GetAwaiter().GetResult();
					break;
				case 12:
					TgSqlTableVersionModel version13 = new() { Version = 13, Description = "Upgrade documents table" };
					VersionRepository.SaveAsync(version13).GetAwaiter().GetResult();
					break;
				case 13:
					TgSqlTableVersionModel version14 = new() { Version = 14, Description = "Upgrade filters table" };
					VersionRepository.SaveAsync(version14).GetAwaiter().GetResult();
					break;
				case 14:
					TgSqlTableVersionModel version15 = new() { Version = 15, Description = "Upgrade messages table" };
					VersionRepository.SaveAsync(version15).GetAwaiter().GetResult();
					break;
				case 15:
					TgSqlTableVersionModel version16 = new() { Version = 16, Description = "Upgrade proxies table" };
					VersionRepository.SaveAsync(version16).GetAwaiter().GetResult();
					break;
				case 16:
					TgSqlTableVersionModel version17 = new() { Version = 17, Description = "Upgrade sources table" };
					VersionRepository.SaveAsync(version17).GetAwaiter().GetResult();
					break;
				case 17:
					TgSqlTableVersionModel version18 = new() { Version = 18, Description = "Upgrade sources table" };
					VersionRepository.SaveAsync(version18).GetAwaiter().GetResult();
					break;
			}
			if (versionLast.Version >= VersionRepository.LastVersion)
				isLast = true;
		}
	}

	public async Task DeleteTablesAsync()
	{
        await DeleteTableAsync(TgSqlConstants.TableApps);
        await DeleteTableAsync(TgSqlConstants.TableProxies);
        await DeleteTableAsync(TgSqlConstants.TableFilters);
        await DeleteTableAsync(TgSqlConstants.TableDocuments);
        await DeleteTableAsync(TgSqlConstants.TableMessages);
        await DeleteTableAsync(TgSqlConstants.TableSources);
        await DeleteTableAsync(TgSqlConstants.TableVersions);
        await DeleteTableAsync(TgSqlConstants.XPObjectType);
	}

	/// <summary>
	/// Delete sql table by name.
	/// </summary>
	/// <param name="tableName"></param>
	public async Task<bool> DeleteTableAsync(string tableName)
    {
        return await TgSqlUtils.TryExecuteAsync($"DROP TABLE IF EXISTS {tableName};");
    }

    /// <summary>
	/// Truncate table.
	/// </summary>
	/// <param name="tableName"></param>
	public async Task<bool> TruncateTableAsync(string tableName)
    {
        return await TgSqlUtils.TryExecuteAsync($"TRUNCATE TABLE {tableName};");
    }

    /// <summary>
	/// Check table exists.
	/// </summary>
	/// <param name="tableName"></param>
	/// <returns></returns>
	public bool IsTableExists(string tableName)
	{
		using UnitOfWork uow = TgSqlUtils.CreateUnitOfWork();
		SelectedData data = uow.ExecuteQuery($"SELECT [type], [name] FROM [sqlite_master] WHERE [name]='{tableName}'");
		if (data is not null && data.ResultSet[0].Rows.Any())
		{
			string name = data.ResultSet[0].Rows[0].Values[1].ToString() ?? string.Empty;
			return Equals(name, tableName);
		}
		return false;
	}

	public IEnumerable<ITgSqlTable> GetTableModels()
    {
        yield return new TgSqlTableAppModel();
        yield return new TgSqlTableDocumentModel();
        yield return new TgSqlTableFilterModel();
        yield return new TgSqlTableProxyModel();
        yield return new TgSqlTableSourceModel();
        yield return new TgSqlTableVersionModel();
    }

    public IEnumerable<Type> GetTableTypes()
    {
        yield return typeof(TgSqlTableAppModel);
        yield return typeof(TgSqlTableDocumentModel);
        yield return typeof(TgSqlTableFilterModel);
        yield return typeof(TgSqlTableProxyModel);
        yield return typeof(TgSqlTableSourceModel);
        yield return typeof(TgSqlTableVersionModel);
    }

    #endregion
}