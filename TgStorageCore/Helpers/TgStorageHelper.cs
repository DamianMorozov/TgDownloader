// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageCore.Helpers;

public class TgStorageHelper
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
    public string FileName => "TgDownloader.sqlite3";

    public bool IsReady
    {
        get
        {
            if (!IsReadyFileExists)
                return false;
            return true;
        }
    }

    public bool IsReadyFileExists => File.Exists(FileName);

    public TgStorageHelper()
    {
        SqLiteCon = new("");
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

    public void AddRecordApp(string apiHash, string phoneNumber)
    {
        if (string.IsNullOrEmpty(apiHash) && string.IsNullOrEmpty(phoneNumber)) return;
        TableAppModel item = GetRecordApp();
        if (!IsValid(item))
        {
            item = new(apiHash, phoneNumber);
            if (IsValid(item))
                SqLiteCon.Insert(item);
        }
    }

    public TableAppModel GetRecordApp()
    {
        InitSqLiteCon();
        List<TableAppModel>? items = SqLiteCon.Query<TableAppModel>("SELECT * FROM APPS");
        if (items is null || items.Count == 0) return new();
        return items.First();
    }

    public bool IsValid(TableAppModel app)
    {
        ValidationResult validationResult = new TableAppValidator().Validate(app);
        return validationResult.IsValid;
    }

    public void AddRecordSource(long? id, string userName)
    {
        if (id is not { } lid) return;
        if (string.IsNullOrEmpty(userName)) return;
        TableSourceModel item = GetRecordSource(id);
        if (!IsValid(item))
        {
            item = new(lid, userName);
            if (IsValid(item))
                SqLiteCon.Insert(item);
        }
    }

    public TableSourceModel GetRecordSource(long? id)
    {
        InitSqLiteCon();
        List<TableSourceModel>? items = SqLiteCon.Query<TableSourceModel>(
            $"SELECT * FROM SOURCES WHERE ID = {id}");
        if (items is null || items.Count == 0) return new();
        return items.First();
    }

    public bool IsValid(TableSourceModel source)
    {
        ValidationResult validationResult = new TableSourceValidator().Validate(source);
        return validationResult.IsValid;
    }

    public void AddRecordMessage(long? id, long? sourceId, string message, string fileName, long fileSize, long accessHash)
    {
        if (id is not { } lid) return;
        if (sourceId is not { } sid) return;
        TableMessageModel item = GetRecordMessage(id, sourceId);
        if (!IsValid(item))
        {
            item = new(lid, sid, message, fileName, fileSize, accessHash);
            if (IsValid(item))
                SqLiteCon.Insert(item);
        }
    }

    public TableMessageModel GetRecordMessage(long? id, long? sourceId)
    {
        InitSqLiteCon();
        List<TableMessageModel>? items = SqLiteCon.Query<TableMessageModel>(
            $"SELECT * FROM MESSAGES WHERE ID = {id} AND SOURCE_ID = {sourceId}");
        if (items is null || items.Count == 0) return new();
        return items.First();
    }

    public bool IsValid(TableMessageModel message)
    {
        ValidationResult validationResult = new TableMessageValidator().Validate(message);
        return validationResult.IsValid;
    }

    #endregion
}