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

    public bool IsExistsDb => File.Exists(TgAppSettings.AppXml.FileStorage);


    #endregion

    #region Public and private methods

    public string ToDebugString() => $"{TgCommonUtils.GetIsExistsDb(IsExistsDb)} | {TgCommonUtils.GetIsReady(IsReady)}";

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
		if (!IsReady)
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
		if (!CheckTableVersions())
			throw new(TgLocaleHelper.Instance.TablesVersionsException);
		FillTableVersions();
	}

	public bool CheckTableApps()
	{
		bool result = true;
		TgSqlTableAppModel itemNew = AppRepository.GetNew();
		if (!AppRepository.Save(itemNew))
			result = false;
		if (!AppRepository.Delete(itemNew))
			result = false;
		return result;
	}

	public bool CheckTableDocuments()
	{
		bool result = true;
		TgSqlTableDocumentModel itemNew = DocumentRepository.GetNew();
		if (!DocumentRepository.Save(itemNew))
			result = false;
		if (!DocumentRepository.Delete(itemNew))
			result = false;
		return result;
	}

	public bool CheckTableFilters()
	{
		bool result = true;
		TgSqlTableFilterModel itemNew = FilterRepository.GetNew();
		if (!FilterRepository.Save(itemNew))
            result = false;
		if (!FilterRepository.Delete(itemNew))
			result = false;
		return result;
	}

	public bool CheckTableMessages()
	{
		bool result = true;
		TgSqlTableMessageModel itemNew = MessageRepository.GetNew();
		if (!MessageRepository.Save(itemNew))
			result = false;
		if (!MessageRepository.Delete(itemNew))
			result = false;
		return result;
	}

	public bool CheckTableProxies()
    {
        bool result = true;
        TgSqlTableProxyModel itemNew = ProxyRepository.GetNew();
		if (!ProxyRepository.Save(itemNew))
			result = false;
        if (!ProxyRepository.Delete(itemNew))
            result = false;
		return result;
	}

	public bool CheckTableSources()
	{
		bool result = true;
		TgSqlTableSourceModel itemNew = SourceRepository.GetNew();
		if (!SourceRepository.Save(itemNew))
			result = false;
		if (!SourceRepository.Delete(itemNew))
			result = false;
		return result;
	}

	public bool CheckTableVersions()
	{
		bool result = true;
		TgSqlTableVersionModel itemNew = VersionRepository.GetNew();
		if (!VersionRepository.Save(itemNew))
			result = false;
		if (!VersionRepository.Delete(itemNew))
			result = false;
		return result;
	}

	public void FillTableVersions()
	{
		bool isLast = false;
		while (!isLast)
		{
			TgSqlTableVersionModel versionLast = !IsTableExists(TgSqlConstants.TableVersions) ? new() : VersionRepository.GetItemLast();
			if (Equals(versionLast.Version, short.MaxValue))
				versionLast.Version = 0;
			switch (versionLast.Version)
			{
				case 0:
					TgSqlTableVersionModel version1 = new() { Version = 1, Description = "Added versions table" };
					VersionRepository.Save(version1);
					break;
				case 1:
					TgSqlTableVersionModel version2 = new() { Version = 2, Description = "Added apps table" };
					VersionRepository.Save(version2);
					break;
				case 2:
					TgSqlTableVersionModel version3 = new() { Version = 3, Description = "Added documents table" };
					VersionRepository.Save(version3);
					break;
				case 3:
					TgSqlTableVersionModel version4 = new() { Version = 4, Description = "Added filters table" };
					VersionRepository.Save(version4);
					break;
				case 4:
					TgSqlTableVersionModel version5 = new() { Version = 5, Description = "Added messages table" };
					VersionRepository.Save(version5);
					break;
				case 5:
					TgSqlTableVersionModel version6 = new() { Version = 6, Description = "Added proxies table" };
					VersionRepository.Save(version6);
					break;
				case 6:
					TgSqlTableVersionModel version7 = new() { Version = 7, Description = "Added sources table" };
					VersionRepository.Save(version7);
					break;
				case 7:
					TgSqlTableVersionModel version8 = new() { Version = 8, Description = "Added source settings table" };
					VersionRepository.Save(version8);
					break;
				case 8:
					TgSqlTableVersionModel version9 = new() { Version = 9, Description = "Upgrade versions table" };
					VersionRepository.Save(version9);
					break;
				case 9:
					TgSqlTableVersionModel version10 = new() { Version = 10, Description = "Upgrade apps table" };
					VersionRepository.Save(version10);
					break;
				case 10:
					TgSqlTableVersionModel version11 = new() { Version = 11, Description = "Upgrade storage on XPO framework" };
					VersionRepository.Save(version11);
					break;
				case 11:
					TgSqlTableVersionModel version12 = new() { Version = 12, Description = "Upgrade apps table" };
					VersionRepository.Save(version12);
					break;
				case 12:
					TgSqlTableVersionModel version13 = new() { Version = 13, Description = "Upgrade documents table" };
					VersionRepository.Save(version13);
					break;
				case 13:
					TgSqlTableVersionModel version14 = new() { Version = 14, Description = "Upgrade filters table" };
					VersionRepository.Save(version14);
					break;
				case 14:
					TgSqlTableVersionModel version15 = new() { Version = 15, Description = "Upgrade messages table" };
					VersionRepository.Save(version15);
					break;
				case 15:
					TgSqlTableVersionModel version16 = new() { Version = 16, Description = "Upgrade proxies table" };
					VersionRepository.Save(version16);
					break;
				case 16:
					TgSqlTableVersionModel version17 = new() { Version = 17, Description = "Upgrade sources table" };
					VersionRepository.Save(version17);
					break;
				case 17:
					TgSqlTableVersionModel version18 = new() { Version = 18, Description = "Upgrade sources table" };
					VersionRepository.Save(version18);
					break;
			}
			if (versionLast.Version >= VersionRepository.LastVersion)
				isLast = true;
		}
	}

	public void DeleteTables()
	{
		DeleteTable(TgSqlConstants.TableApps);
		DeleteTable(TgSqlConstants.TableProxies);
		DeleteTable(TgSqlConstants.TableFilters);
		DeleteTable(TgSqlConstants.TableDocuments);
		DeleteTable(TgSqlConstants.TableMessages);
		DeleteTable(TgSqlConstants.TableSources);
		DeleteTable(TgSqlConstants.TableVersions);
		DeleteTable(TgSqlConstants.XPObjectType);
	}

	/// <summary>
	/// Delete sql table by name.
	/// </summary>
	/// <param name="tableName"></param>
	public bool DeleteTable(string tableName)
    {
        return TgSqlUtils.TryExecute($"DROP TABLE IF EXISTS {tableName};");
    }

    /// <summary>
	/// Truncate table.
	/// </summary>
	/// <param name="tableName"></param>
	public bool TruncateTable(string tableName)
    {
        return TgSqlUtils.TryExecute($"TRUNCATE TABLE {tableName};");
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