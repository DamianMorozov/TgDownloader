// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgLocaleCore.Interfaces;

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
        //if (!SqLiteCon.Handle.IsInvalid && !SqLiteCon.Handle.IsClosed) return;
        //if (!SqLiteCon.Handle.IsClosed) return;

        SQLiteConnectionString options = new(FileName, false);
        SqLiteCon = new(options);
    }

    public void CreateTables()
    {
        InitSqLiteCon();
        if (!IsReady) return;
        SqLiteCon.CreateTable<TableAppModel>();
        SqLiteCon.CreateTable<TableSourceModel>();
        SqLiteCon.CreateTable<TableMessageModel>();
        SqLiteCon.CreateTable<TableDocumentModel>();
    }

    public void ClearTables()
    {
        InitSqLiteCon();
        if (!IsReady) return;
        SqLiteCon.DeleteAll<TableAppModel>();
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
        List<SQLiteConnection.ColumnInfo>? info = SqLiteCon.GetTableInfo(nameof(TableAppModel));
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
        SqLiteCon.DropTable<TableAppModel>();
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