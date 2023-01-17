// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgCore.Helpers;
using TgCore.Interfaces;
using TgCore.Utils;
using TgLocalization.Helpers;
using TgStorageCore.Models.Apps;
using TgStorageCore.Models.Documents;
using TgStorageCore.Models.Messages;
using TgStorageCore.Models.Sources;
using TgStorageCore.Models.SourcesSettings;

namespace TgStorageCore.Helpers;

public partial class TgStorageHelper : IHelper
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgStorageHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgStorageHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public SQLiteConnection SqLiteCon { get; private set; }
    public TgLogHelper TgLog => TgLogHelper.Instance;
    public TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
    public string FileName { get; set; }

    public bool IsReady => IsReadyFileExists;

    public bool IsReadyFileExists => File.Exists(FileName);

    public TgStorageHelper()
    {
        SqLiteCon = new("");
        FileName = FileNameUtils.Storage;
        // https://github.com/softlion/SQLite.Net-PCL2
        SQLitePCL.Batteries_V2.Init();
    }

    #endregion

    #region Public and private methods

    public void CreateOrConnectDb()
    {
        InitSqLiteCon();
        CreateTables();
    }

    private void InitSqLiteCon()
    {
        if (!string.IsNullOrEmpty(SqLiteCon.DatabasePath)) return;

        SQLiteConnectionString options = new(FileName, false);
        SqLiteCon = new(options);
    }

    public void CreateTables()
    {
        InitSqLiteCon();
        if (!IsReady) return;
        SqLiteCon.CreateTable<SqlTableAppModel>();
        SqLiteCon.CreateTable<SqlTableDocumentModel>();
        SqLiteCon.CreateTable<SqlTableMessageModel>();
        SqLiteCon.CreateTable<SqlTableSourceModel>();
        SqLiteCon.CreateTable<SqlTableSourceSettingModel>();
    }

    public void ClearTables()
    {
        InitSqLiteCon();
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
        File.Delete(FileName);
    }

    public void ViewStatistics()
    {
        InitSqLiteCon();
        TgLog.Info(TgLocale.MenuClientGetInfo);
        List<SQLiteConnection.ColumnInfo>? info = SqLiteCon.GetTableInfo(nameof(SqlTableAppModel));
        if (info is not null)
        {
            foreach (SQLiteConnection.ColumnInfo columnInfo in info)
            {
                TgLog.Info($"{columnInfo.Name}: {columnInfo}");
            }
        }
    }

    public void DropTables()
    {
        InitSqLiteCon();
        SqLiteCon.DropTable<SqlTableAppModel>();
        SqLiteCon.DropTable<SqlTableDocumentModel>();
        SqLiteCon.DropTable<SqlTableMessageModel>();
        SqLiteCon.DropTable<SqlTableSourceSettingModel>();
        SqLiteCon.DropTable<SqlTableSourceModel>();
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
        SqLiteCon = info.GetValue(nameof(SqLiteCon), typeof(SQLiteConnection)) as SQLiteConnection ?? new("");
        FileName = info.GetString(nameof(FileName)) ?? this.GetPropertyDefaultValueAsString(nameof(FileName));
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(Version), SqLiteCon);
        info.AddValue(nameof(FileName), FileName);
    }

    #endregion
}