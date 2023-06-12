// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Helpers;

/// <summary>
/// SQL data storage context helper.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public sealed class TgSqlContextManagerHelper : ITgHelper
{
	#region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	private static TgSqlContextManagerHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public static TgSqlContextManagerHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

	#endregion

	#region Public and private fields, properties, constructor

	public TgSqlContextCacheHelper ContextCache => TgSqlContextCacheHelper.Instance;
	public TgSqlTableAppController ContextTableApps => TgSqlTableAppController.Instance;
	public TgSqlTableDocumentController ContextTableDocuments => TgSqlTableDocumentController.Instance;
	public TgSqlTableFilterController ContextTableFilters => TgSqlTableFilterController.Instance;
	public TgSqlTableMessageController ContextTableMessages => TgSqlTableMessageController.Instance;
	public TgSqlTableProxyController ContextTableProxies => TgSqlTableProxyController.Instance;
	public TgSqlTableSourceController ContextTableSources => TgSqlTableSourceController.Instance;
	public TgSqlTableVersionController ContextTableVersions => TgSqlTableVersionController.Instance;

	public TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;
	public TgLogHelper TgLog => TgLogHelper.Instance;
	public bool IsReady => TgAppSettings.AppXml.IsExistsFileStorage &&
			IsTableExists(TgSqlConstants.TableApps) && IsTableExists(TgSqlConstants.TableDocuments) && 
		IsTableExists(TgSqlConstants.TableFilters) && IsTableExists(TgSqlConstants.TableMessages) &&
		IsTableExists(TgSqlConstants.TableProxies) && IsTableExists(TgSqlConstants.TableSources) &&
		IsTableExists(TgSqlConstants.TableVersions);

	#endregion

	#region Public and private methods

	public bool IsExistsDb() =>
		File.Exists(TgAppSettings.AppXml.FileStorage);

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
		// Setup XPO.
		string connectionString = SQLiteConnectionProvider.GetConnectionString(TgAppSettings.AppXml.FileStorage);
		XpoDefault.DataLayer = XpoDefault.GetDataLayer(connectionString, AutoCreateOption.DatabaseAndSchema);
		
		if (isCheckTables)
			CheckTables();
	}

	public void DeleteExistsDb()
	{
		if (!IsReady) return;
		File.Delete(TgAppSettings.AppXml.FileStorage);
	}

	public void VersionsView()
	{
		List<TgSqlTableVersionModel> versions = ContextTableVersions.GetList();
		foreach (TgSqlTableVersionModel version in versions)
		{
			TgLog.WriteLine($" {version.Version:00} | {version.Description}");
		}
	}

	public void FiltersView()
	{
		List<TgSqlTableFilterModel> filters = ContextTableFilters.GetList();
		foreach (TgSqlTableFilterModel filter in filters)
		{
			TgLog.WriteLine(filter.ToString());
		}
	}

	/// <summary>
	/// Update structures of tables.
	/// </summary>
	private bool CheckTables()
	{
		bool result = CheckTableProxies();
		if (!CheckTableApps()) result = false;
		if (!CheckTableDocuments()) result = false;
		if (!CheckTableFilters()) result = false;
		if (!CheckTableMessages()) result = false;
		if (!CheckTableSources()) result = false;
		if (!CheckTableVersions()) result = false;
		FillTableVersions();
		return result;
	}

	public bool CheckTableApps()
	{
		bool result = true;
		TgSqlTableAppModel app = ContextTableApps.GetNewItem();
		if (!ContextTableApps.AddOrUpdateItem(app, ContextTableProxies.GetNewItem().Uid)) result = false;
		if (!ContextTableApps.DeleteItem(app)) result = false;
		return result;
	}

	public bool CheckTableDocuments()
	{
		bool result = true;
		TgSqlTableDocumentModel document = ContextTableDocuments.GetNewItem();
		if (!ContextTableDocuments.AddOrUpdateItem(document)) result = false;
		if (!ContextTableDocuments.DeleteItem(document)) result = false;
		return result;
	}

	public bool CheckTableFilters()
	{
		bool result = true;
		TgSqlTableFilterModel filter = ContextTableFilters.GetNewItem();
		if (!ContextTableFilters.AddOrUpdateItem(filter)) result = false;
		//Filters.DeleteItem(filter);
		return result;
	}

	public bool CheckTableMessages()
	{
		bool result = true;
		TgSqlTableMessageModel message = ContextTableMessages.GetNewItem();
		if (!ContextTableMessages.AddOrUpdateItem(message)) result = false;
		if (!ContextTableMessages.DeleteItem(message)) result = false;
		return result;
	}

	public bool CheckTableProxies()
	{
		bool result = true;
		TgSqlTableProxyModel proxy = ContextTableProxies.GetNewItem();
		if (!ContextTableProxies.AddOrUpdateItem(proxy)) result = false;
		//Proxies.DeleteItem(proxy);
		return result;
	}

	public bool CheckTableSources()
	{
		bool result = true;
		TgSqlTableSourceModel source = ContextTableSources.GetNewItem();
		if (!ContextTableSources.AddOrUpdateItem(source)) result = false;
		if (!ContextTableSources.DeleteItem(source)) result = false;
		return result;
	}

	public bool CheckTableVersions()
	{
		bool result = true;
		TgSqlTableVersionModel version = ContextTableVersions.GetNewItem();
		if (!ContextTableVersions.AddOrUpdateItem(version)) result = false;
		if (!ContextTableVersions.DeleteItem(version)) result = false;
		return result;
	}

	public void FillTableVersions()
	{
		bool isLast = false;
		while (!isLast)
		{
			TgSqlTableVersionModel versionLast = !IsTableExists(TgSqlConstants.TableVersions) ? new() : ContextTableVersions.GetItemLast();
			if (Equals(versionLast.Version, short.MaxValue)) versionLast.Version = 0;
			switch (versionLast.Version)
			{
				case 0:
					TgSqlTableVersionModel version1 = new() { Version = 1, Description = "Added versions table" };
					ContextTableVersions.AddItem(version1);
					break;
				case 1:
					TgSqlTableVersionModel version2 = new() { Version = 2, Description = "Added apps table" };
					ContextTableVersions.AddItem(version2);
					break;
				case 2:
					TgSqlTableVersionModel version3 = new() { Version = 3, Description = "Added documents table" };
					ContextTableVersions.AddItem(version3);
					break;
				case 3:
					TgSqlTableVersionModel version4 = new() { Version = 4, Description = "Added filters table" };
					ContextTableVersions.AddItem(version4);
					break;
				case 4:
					TgSqlTableVersionModel version5 = new() { Version = 5, Description = "Added messages table" };
					ContextTableVersions.AddItem(version5);
					break;
				case 5:
					TgSqlTableVersionModel version6 = new() { Version = 6, Description = "Added proxies table" };
					ContextTableVersions.AddItem(version6);
					break;
				case 6:
					TgSqlTableVersionModel version7 = new() { Version = 7, Description = "Added sources table" };
					ContextTableVersions.AddItem(version7);
					break;
				case 7:
					TgSqlTableVersionModel version8 = new() { Version = 8, Description = "Added source settings table" };
					ContextTableVersions.AddItem(version8);
					break;
				case 8:
					TgSqlTableVersionModel version9 = new() { Version = 9, Description = "Upgrade versions table" };
					ContextTableVersions.AddItem(version9);
					break;
				case 9:
					TgSqlTableVersionModel version10 = new() { Version = 10, Description = "Upgrade apps table" };
					ContextTableVersions.AddItem(version10);
					break;
				case 10:
					TgSqlTableVersionModel version11 = new() { Version = 11, Description = "Upgrade storage on XPO framework" };
					ContextTableVersions.AddItem(version11);
					break;
				case 11:
					TgSqlTableVersionModel version12 = new() { Version = 12, Description = "Upgrade apps table" };
					ContextTableVersions.AddItem(version12);
					break;
				case 12:
					TgSqlTableVersionModel version13 = new() { Version = 13, Description = "Upgrade documents table" };
					ContextTableVersions.AddItem(version13);
					break;
				case 13:
					TgSqlTableVersionModel version14 = new() { Version = 14, Description = "Upgrade filters table" };
					ContextTableVersions.AddItem(version14);
					break;
				case 14:
					TgSqlTableVersionModel version15 = new() { Version = 15, Description = "Upgrade messages table" };
					ContextTableVersions.AddItem(version15);
					break;
				case 15:
					TgSqlTableVersionModel version16 = new() { Version = 16, Description = "Upgrade proxies table" };
					ContextTableVersions.AddItem(version16);
					break;
				case 16:
					TgSqlTableVersionModel version17 = new() { Version = 17, Description = "Upgrade sources table" };
					ContextTableVersions.AddItem(version17);
					break;
				case 17:
					TgSqlTableVersionModel version18 = new() { Version = 18, Description = "Upgrade sources table" };
					ContextTableVersions.AddItem(version18);
					break;
			}
			if (versionLast.Version >= ContextTableVersions.VersionLast)
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
		DeleteTable(nameof(TgSqlTableBase));
		DeleteTable(TgSqlConstants.XPObjectType);
	}

	/// <summary>
	/// Delete sql table by name.
	/// </summary>
	/// <param name="tableName"></param>
	public void DeleteTable(string tableName)
	{
		using UnitOfWork uow = new();
		uow.ExecuteNonQuery($"DROP TABLE IF EXISTS {tableName};");
		uow.CommitChanges();
	}

	/// <summary>
	/// Check table exists.
	/// </summary>
	/// <param name="tableName"></param>
	/// <returns></returns>
	public bool IsTableExists(string tableName)
	{
		using UnitOfWork uow = new();
		SelectedData data = uow.ExecuteQuery($"SELECT [type], [name] FROM [sqlite_master] WHERE [name]='{tableName}'");
		if (data is not null && data.ResultSet[0].Rows.Any())
		{
			string name = data.ResultSet[0].Rows[0].Values[1].ToString() ?? string.Empty;
			return Equals(name, tableName);
		}
		return false;
	}

	public List<TgSqlTableBase> GetTableModels() => new()
	{
		new TgSqlTableAppModel(),
		new TgSqlTableDocumentModel(),
		new TgSqlTableFilterModel(),
		new TgSqlTableProxyModel(),
		new TgSqlTableSourceModel(),
		new TgSqlTableVersionModel(),
	};

	public List<Type> GetTableTypes() => new()
	{
		typeof(TgSqlTableAppModel),
		typeof(TgSqlTableDocumentModel),
		typeof(TgSqlTableFilterModel),
		typeof(TgSqlTableProxyModel),
		typeof(TgSqlTableSourceModel),
		typeof(TgSqlTableVersionModel),
	};

	#endregion
}