// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo.DB;
using TgCore.Helpers;
using TgCore.Localization;
using TgStorage.Models.Apps;
using TgStorage.Models.Documents;
using TgStorage.Models.Filters;
using TgStorage.Models.Messages;
using TgStorage.Models.Proxies;
using TgStorage.Models.Sources;
using TgStorage.Models.SourcesSettings;
using TgStorage.Models.Versions;

namespace TgStorage.Helpers;

public partial class TgStorageHelper : IHelper
{
	#region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	private static TgStorageHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public static TgStorageHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

	#endregion

	#region Public and private fields, properties, constructor

	public AppSettingsHelper AppSettings => AppSettingsHelper.Instance;
	public SQLite.SQLiteConnection SqLiteCon { get; private set; }
	public TgLogHelper TgLog => TgLogHelper.Instance;
	public TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
	public bool IsReady => AppSettings.AppXml.IsExistsFileStorage;
	public SqlTableAppModel App => GetApp();
	public SqlTableProxyModel Proxy => GetItem<SqlTableProxyModel>(App.ProxyUid);

	public TgStorageHelper()
	{
		SqLiteCon = new("");
	}

	#endregion

	#region Public and private methods

	public void CreateOrConnectDb()
	{
		if (string.IsNullOrEmpty(SqLiteCon.DatabasePath))
		{
			SQLite.SQLiteConnectionString options = new(AppSettings.AppXml.FileStorage, false);
			SqLiteCon = new(options);
		}
		CreateTables();
		// XPO.
		string connectionString = SQLiteConnectionProvider.GetConnectionString(AppSettings.AppXml.FileStorage);
		XpoDefault.DataLayer = XpoDefault.GetDataLayer(connectionString, AutoCreateOption.DatabaseAndSchema);
		// Upgrade tables.
		UpgradeTables();
	}

	public void CreateTables()
	{
		if (!IsReady) return;
		SqLiteCon.CreateTable<SqlTableAppModel>();
		SqLiteCon.CreateTable<SqlTableDocumentModel>();
		SqLiteCon.CreateTable<SqlTableMessageModel>();
		SqLiteCon.CreateTable<SqlTableSourceModel>();
		SqLiteCon.CreateTable<SqlTableSourceSettingModel>();
	}

	public void DeleteExistsDb()
	{
		if (!IsReady) return;
		File.Delete(AppSettings.AppXml.FileStorage);
	}

	public void VersionsView()
	{
		List<SqlTableVersionModel> versions = GetVersionsList();
		foreach (SqlTableVersionModel version in versions)
		{
			TgLog.WriteLine($" {version.Version:00} | {version.Description}");
		}
	}

	public void FiltersView()
	{
		List<SqlTableFilterModel> filters = GetFiltersList();
		foreach (SqlTableFilterModel filter in filters)
		{
			TgLog.WriteLine(filter.ToString());
		}
	}

	public void DropTables()
	{
		SqLiteCon.DropTable<SqlTableAppModel>();
		SqLiteCon.DropTable<SqlTableDocumentModel>();
		SqLiteCon.DropTable<SqlTableMessageModel>();
		SqLiteCon.DropTable<SqlTableSourceSettingModel>();
		SqLiteCon.DropTable<SqlTableSourceModel>();
	}

	private void UpgradeTables()
	{
		// Upgrade table APPS.
		try
		{
			_ = App;
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("Unable to create 'Column' 'UID'"))
			{
				SqlTableAppDeprecatedModel appDeprecated = GetList<SqlTableAppDeprecatedModel>().First();
				SqLiteCon.DropTable<SqlTableAppDeprecatedModel>();
				AddOrUpdateItem<SqlTableAppModel>(new() { ApiHash = appDeprecated.ApiHash, PhoneNumber = appDeprecated.PhoneNumber });
				_ = App;
			}
		}

		// Update app.
		if (App.IsExists)
			UpdateItem(App);

		// Update version.
		UpgradeTableVersions();

		// Update filters.
		UpgradeTableFilters();
	}

	private void UpgradeTableVersions()
	{
		List<SqlTableVersionModel> versions = GetVersionsList();
		if (!versions.Any())
		{
			SqlTableVersionModel version = new() { Version = 11, Description = "Storage version table" };
			AddItem(version);
		}
		else
		{
			SqlTableVersionModel? version11 = versions.Find(item => Equals(item.Version, (ushort)11));
			if (version11 is not null)
			{
				version11.Description = "Added versions table";
				UpdateItem(version11);
			}
			SqlTableVersionModel? version = versions.LastOrDefault();
			if (version is not null)
				switch (version.Version)
				{
					case 11:
						SqlTableVersionModel version12 = new() { Version = 12, Description = "Added filters table" };
						AddItem(version12);
						break;
				}
		}
	}

	private void UpgradeTableFilters()
	{
		List<SqlTableFilterModel> filters = GetFiltersList();
	}

	#endregion

	#region Public and private methods - ISerializable

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	protected TgStorageHelper(SerializationInfo info, StreamingContext context)
	{
		SqLiteCon = info.GetValue(nameof(SqLiteCon), typeof(SQLite.SQLiteConnection)) as SQLite.SQLiteConnection ?? new("");
	}

	/// <summary>
	/// Get object data for serialization info.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(Version), SqLiteCon);
	}

	#endregion
}