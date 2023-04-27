// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Helpers;

public sealed class TgSqlContextManagerHelper : ITgHelper
{
	#region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	private static TgSqlContextManagerHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public static TgSqlContextManagerHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

	#endregion

	#region Public and private fields, properties, constructor

	public TgSqlAppHelper Apps => TgSqlAppHelper.Instance;
	public TgSqlDocumentHelper Documents => TgSqlDocumentHelper.Instance;
	public TgSqlFilterHelper Filters => TgSqlFilterHelper.Instance;
	public TgSqlMessageHelper Messages => TgSqlMessageHelper.Instance;
	public TgSqlProxyHelper Proxies => TgSqlProxyHelper.Instance;
	public TgSqlSourceHelper Sources => TgSqlSourceHelper.Instance;
	public TgSqlVersionHelper Versions => TgSqlVersionHelper.Instance;
	public TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;
	public TgLogHelper TgLog => TgLogHelper.Instance;
	public TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
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
		List<TgSqlTableVersionModel> versions = Versions.GetList(false);
		foreach (TgSqlTableVersionModel version in versions)
		{
			TgLog.WriteLine($" {version.Version:00} | {version.Description}");
		}
	}

	public void FiltersView()
	{
		List<TgSqlTableFilterModel> filters = Filters.GetList(false);
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
		TgSqlTableAppModel app = Apps.GetNewItem();
		if (!Apps.AddOrUpdateItem(app, Proxies.GetNewItem().Uid)) result = false;
		if (!Apps.DeleteItem(app)) result = false;
		return result;
	}

	public bool CheckTableDocuments()
	{
		bool result = true;
		TgSqlTableDocumentModel document = Documents.GetNewItem();
		if (!Documents.AddOrUpdateItem(document)) result = false;
		if (!Documents.DeleteItem(document)) result = false;
		return result;
	}

	public bool CheckTableFilters()
	{
		bool result = true;
		TgSqlTableFilterModel filter = Filters.GetNewItem();
		if (!Filters.AddOrUpdateItem(filter)) result = false;
		//Filters.DeleteItem(filter);
		return result;
	}

	public bool CheckTableMessages()
	{
		bool result = true;
		TgSqlTableMessageModel message = Messages.GetNewItem();
		if (!Messages.AddOrUpdateItem(message)) result = false;
		if (!Messages.DeleteItem(message)) result = false;
		return result;
	}

	public bool CheckTableProxies()
	{
		bool result = true;
		TgSqlTableProxyModel proxy = Proxies.GetNewItem();
		if (!Proxies.AddOrUpdateItem(proxy)) result = false;
		//Proxies.DeleteItem(proxy);
		return result;
	}

	public bool CheckTableSources()
	{
		bool result = true;
		TgSqlTableSourceModel source = Sources.GetNewItem();
		if (!Sources.AddOrUpdateItem(source)) result = false;
		if (!Sources.DeleteItem(source)) result = false;
		return result;
	}

	public bool CheckTableVersions()
	{
		bool result = true;
		TgSqlTableVersionModel version = Versions.GetNewItem();
		if (!Versions.AddOrUpdateItem(version)) result = false;
		if (!Versions.DeleteItem(version)) result = false;
		return result;
	}

	public void FillTableVersions()
	{
		bool isLast = false;
		while (!isLast)
		{
			TgSqlTableVersionModel versionLast = !IsTableExists(TgSqlConstants.TableVersions) ? new() : Versions.GetItemLast();
			if (Equals(versionLast.Version, short.MaxValue)) versionLast.Version = 0;
			switch (versionLast.Version)
			{
				case 0:
					TgSqlTableVersionModel version1 = new() { Version = 1, Description = "Added versions table" };
					Versions.AddItem(version1);
					break;
				case 1:
					TgSqlTableVersionModel version2 = new() { Version = 2, Description = "Added apps table" };
					Versions.AddItem(version2);
					break;
				case 2:
					TgSqlTableVersionModel version3 = new() { Version = 3, Description = "Added documents table" };
					Versions.AddItem(version3);
					break;
				case 3:
					TgSqlTableVersionModel version4 = new() { Version = 4, Description = "Added filters table" };
					Versions.AddItem(version4);
					break;
				case 4:
					TgSqlTableVersionModel version5 = new() { Version = 5, Description = "Added messages table" };
					Versions.AddItem(version5);
					break;
				case 5:
					TgSqlTableVersionModel version6 = new() { Version = 6, Description = "Added proxies table" };
					Versions.AddItem(version6);
					break;
				case 6:
					TgSqlTableVersionModel version7 = new() { Version = 7, Description = "Added sources table" };
					Versions.AddItem(version7);
					break;
				case 7:
					TgSqlTableVersionModel version8 = new() { Version = 8, Description = "Added source settings table" };
					Versions.AddItem(version8);
					break;
				case 8:
					TgSqlTableVersionModel version9 = new() { Version = 9, Description = "Upgrade versions table" };
					Versions.AddItem(version9);
					break;
				case 9:
					TgSqlTableVersionModel version10 = new() { Version = 10, Description = "Upgrade apps table" };
					Versions.AddItem(version10);
					break;
				case 10:
					TgSqlTableVersionModel version11 = new() { Version = 11, Description = "Upgrade storage on XPO framework" };
					Versions.AddItem(version11);
					break;
				case 11:
					TgSqlTableVersionModel version12 = new() { Version = 12, Description = "Upgrade apps table" };
					Versions.AddItem(version12);
					break;
				case 12:
					TgSqlTableVersionModel version13 = new() { Version = 13, Description = "Upgrade documents table" };
					Versions.AddItem(version13);
					break;
				case 13:
					TgSqlTableVersionModel version14 = new() { Version = 14, Description = "Upgrade filters table" };
					Versions.AddItem(version14);
					break;
				case 14:
					TgSqlTableVersionModel version15 = new() { Version = 15, Description = "Upgrade messages table" };
					Versions.AddItem(version15);
					break;
				case 15:
					TgSqlTableVersionModel version16 = new() { Version = 16, Description = "Upgrade proxies table" };
					Versions.AddItem(version16);
					break;
				case 16:
					TgSqlTableVersionModel version17 = new() { Version = 17, Description = "Upgrade sources table" };
					Versions.AddItem(version17);
					break;
				case 17:
					TgSqlTableVersionModel version18 = new() { Version = 18, Description = "Upgrade sources table" };
					Versions.AddItem(version18);
					break;
			}
			if (versionLast.Version >= Versions.VersionLast)
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