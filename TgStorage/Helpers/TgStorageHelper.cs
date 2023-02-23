// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo.DB;
using TgCore.Helpers;
using TgCore.Localization;
using TgStorage.Models.Apps;
using TgStorage.Models.Documents;
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
    public SqlTableAppModel App => GetItemApp();
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

    public void ClearTables()
    {
        if (!IsReady) return;
        SqLiteCon.DeleteAll<SqlTableAppModel>();
        SqLiteCon.DeleteAll<SqlTableDocumentModel>();
        SqLiteCon.DeleteAll<SqlTableMessageModel>();
        SqLiteCon.DeleteAll<SqlTableSourceSettingModel>();
        SqLiteCon.DeleteAll<SqlTableSourceModel>();
    }

    public void DeleteExistsDb()
    {
        if (!IsReady) return;
        File.Delete(AppSettings.AppXml.FileStorage);
    }

    public void ViewStatistics()
    {
        TgLog.Info(TgLocale.MenuClientGetInfo);
        List<SQLite.SQLiteConnection.ColumnInfo>? info = SqLiteCon.GetTableInfo(nameof(SqlTableAppModel));
        if (info is not null)
        {
            foreach (SQLite.SQLiteConnection.ColumnInfo columnInfo in info)
            {
                TgLog.Info($"{columnInfo.Name}: {columnInfo}");
            }
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
                AddOrUpdateItem<SqlTableAppModel>(new () { ApiHash = appDeprecated.ApiHash, PhoneNumber = appDeprecated.PhoneNumber });
                _ = App;
            }
        }
        
        // Update app.
        if (App.IsExists)
            UpdateItem(App);
        
        // Update version.
        List<SqlTableVersionModel> versions = GetVersionsList();
        if (!versions.Any())
        {
            SqlTableVersionModel version = new() { Version = 11, Description = "Storage version table" };
            AddItem(version);
        }
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