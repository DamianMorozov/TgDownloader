// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Utils;

public static class TgSqlUtils
{
    #region Public and private methods

    public static TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;
    private static string _connectionString = "";
    public static string ConnectionString {
        get
        {
            if (string.IsNullOrEmpty(_connectionString))
                _connectionString = SQLiteConnectionProvider.GetConnectionString(TgAppSettings.AppXml.FileStorage);
            return _connectionString;
        }
    }

    private static ThreadSafeDataLayer? DataLayer { get; set; }

    public static async Task<bool> TryExecuteAsync(string cmd)
    {
        using UnitOfWork uow = CreateUnitOfWork();
        try
        {
            if (!uow.InTransaction)
                uow.BeginTransaction();

            await uow.ExecuteNonQueryAsync(cmd);
            await uow.CommitChangesAsync();
            await uow.CommitTransactionAsync();
            return true;
        }
#if DEBUG
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
#else
        catch (Exception)
        {
#endif
            uow.RollbackTransaction();
            throw;
        }
    }

    public static async Task<bool> TryInsertAsync<T>(T item) where T : ITgSqlTable
    {
        if (item is not XPLiteObject xpLite) return false;
        try
        {
            if (!xpLite.Session.InTransaction)
                xpLite.Session.BeginTransaction();

            await xpLite.Session.SaveAsync(item);
            await xpLite.Session.CommitTransactionAsync();
            return true;
        }
#if DEBUG
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
#else
        catch (Exception)
        {
#endif
            xpLite.Session.RollbackTransaction();
            throw;
        }
    }

    public static async Task<bool> TryUpdateAsync<T>(T item) where T : ITgSqlTable
    {
        if (item is not XPLiteObject xpLite) return false;
        if (xpLite.Session is not UnitOfWork uow) return false;
        try
        {
            if (!uow.InTransaction)
                uow.BeginTransaction();
            await uow.SaveAsync(item);
            await uow.CommitChangesAsync();
            await uow.CommitTransactionAsync();
            return true;
        }
#if DEBUG
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
#else
        catch (Exception)
        {
#endif
            uow.RollbackTransaction();
            throw;
        }
    }

    public static async Task<bool> TryDeleteAsync<T>(T item) where T : ITgSqlTable
    {
        if (item is not XPLiteObject xpLite) return false;
        try
        {
            if (!xpLite.Session.InTransaction)
                xpLite.Session.BeginTransaction();
            await xpLite.Session.DeleteAsync(item);
            await xpLite.Session.CommitTransactionAsync();
            return true;
        }
#if DEBUG
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
#else
        catch (Exception)
        {
#endif
            xpLite.Session.RollbackTransaction();
            throw;
        }
    }

    #endregion

    #region Public and private methods

    public static void SetXpoDefault()
    {
        if (XpoDefault.DataLayer is null)
        {
            //XpoDefault.DataLayer = XpoDefault.GetDataLayer(ConnectionString, AutoCreateOption.DatabaseAndSchema);
            DataLayer = new(XpoDefault.GetConnectionProvider(ConnectionString, AutoCreateOption.DatabaseAndSchema));
            XpoDefault.DataLayer = DataLayer;
        }
    }

    public static UnitOfWork CreateUnitOfWork()
    {
        SetXpoDefault();
        //SimpleDataLayer.SuppressReentrancyAndThreadSafetyCheck = true;
        
        //var session = new UnitOfWork(XpoDefault.DataLayer)
        var session = new UnitOfWork(DataLayer)
        {
            //Site = null,
            //TrackPropertiesModifications = false,
            //CaseSensitive = false,
            //IsObjectModifiedOnNonPersistentPropertyChange = null,
            //AutoCreateOption = AutoCreateOption.DatabaseAndSchema,
            //Connection = null,
            //ConnectionString = null,
            LockingOption = LockingOption.Optimistic,
            //OptimisticLockingReadBehavior = OptimisticLockingReadBehavior.Default,
            //IdentityMapBehavior = IdentityMapBehavior.Default
        };
        return session;
    }

    #endregion

    #region Public and private methods - Delete

    public static void DeleteNewItems()
    {
        TgSqlTableAppRepository.Instance.DeleteNewAsync();
        TgSqlTableDocumentRepository.Instance.DeleteNewAsync();
        TgSqlTableFilterRepository.Instance.DeleteNewAsync();
        TgSqlTableMessageRepository.Instance.DeleteNewAsync();
        TgSqlTableProxyRepository.Instance.DeleteNewAsync();
        TgSqlTableSourceRepository.Instance.DeleteNewAsync();
        TgSqlTableVersionRepository.Instance.DeleteNewAsync();
    }

    #endregion

    #region Public and private methods - CreateNew

    public static TgSqlTableAppModel CreateNewApp() => TgSqlTableAppRepository.Instance.CreateNew(false);
    public static TgSqlTableDocumentModel CreateNewDocument() => TgSqlTableDocumentRepository.Instance.CreateNew(false);
    public static TgSqlTableFilterModel CreateNewFilter() => TgSqlTableFilterRepository.Instance.CreateNew(false);
    public static TgSqlTableMessageModel CreateNewMessage() => TgSqlTableMessageRepository.Instance.CreateNew(false);
    public static TgSqlTableProxyModel CreateNewProxy() => TgSqlTableProxyRepository.Instance.CreateNew(false);
    public static TgSqlTableSourceModel CreateNewSource() => TgSqlTableSourceRepository.Instance.CreateNew(false);
    public static TgSqlTableVersionModel CreateNewVersion() => TgSqlTableVersionRepository.Instance.CreateNew(false);

    #endregion

    #region Public and private methods - GetValidXpLite

    public static ValidationResult GetValidXpLite(TgSqlTableAppModel item) => new TgSqlTableAppValidator().Validate(item);
    public static ValidationResult GetValidXpLite(TgSqlTableDocumentModel item) => new TgSqlTableDocumentValidator().Validate(item);
    public static ValidationResult GetValidXpLite(TgSqlTableFilterModel item) => new TgSqlTableFilterValidator().Validate(item);
    public static ValidationResult GetValidXpLite(TgSqlTableMessageModel item) => new TgSqlTableMessageValidator().Validate(item);
    public static ValidationResult GetValidXpLite(TgSqlTableSourceModel item) => new TgSqlTableSourceValidator().Validate(item);
    public static ValidationResult GetValidXpLite(TgSqlTableProxyModel item) => new TgSqlTableProxyValidator().Validate(item);
    public static ValidationResult GetValidXpLite(TgSqlTableVersionModel item) => new TgSqlTableVersionValidator().Validate(item);

    #endregion
}