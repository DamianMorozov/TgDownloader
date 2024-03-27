// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Utils;

public static class TgEfUtils
{
    #region Public and private methods

//    public static TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;
//    private static string _connectionString = "";
//    public static string ConnectionString {
//        get
//        {
//            if (string.IsNullOrEmpty(_connectionString))
//                _connectionString = SQLiteConnectionProvider.GetConnectionString(TgAppSettings.AppXml.FileStorage);
//            return _connectionString;
//        }
//    }

//    private static ThreadSafeDataLayer? DataLayer { get; set; }

//    public static async Task<bool> TryExecuteAsync(string cmd)
//    {
//        using UnitOfWork uow = CreateUnitOfWork();
//        try
//        {
//            if (!uow.InTransaction)
//                uow.BeginTransaction();

//            await uow.ExecuteNonQueryAsync(cmd);
//            await uow.CommitChangesAsync();
//            await uow.CommitTransactionAsync();
//            return true;
//        }
//#if DEBUG
//        catch (Exception ex)
//        {
//            Debug.WriteLine(ex);
//#else
//        catch (Exception)
//        {
//#endif
//            uow.RollbackTransaction();
//            throw;
//        }
//    }

//    public static async Task<bool> TryInsertAsync<T>(T item) where T : ITgEf
//    {
//        if (item is not XPLiteObject xpLite) return false;
//        try
//        {
//            if (!xpLite.Session.InTransaction)
//                xpLite.Session.BeginTransaction();

//            await xpLite.Session.SaveAsync(item);
//            await xpLite.Session.CommitTransactionAsync();
//            return true;
//        }
//#if DEBUG
//        catch (Exception ex)
//        {
//            Debug.WriteLine(ex);
//#else
//        catch (Exception)
//        {
//#endif
//            xpLite.Session.RollbackTransaction();
//            throw;
//        }
//    }

//    public static async Task<bool> TryUpdateAsync<T>(T item) where T : ITgEf
//    {
//        if (item is not XPLiteObject xpLite) return false;
//        if (xpLite.Session is not UnitOfWork uow) return false;
//        try
//        {
//            if (!uow.InTransaction)
//                uow.BeginTransaction();
//            await uow.SaveAsync(item);
//            await uow.CommitChangesAsync();
//            await uow.CommitTransactionAsync();
//            return true;
//        }
//#if DEBUG
//        catch (Exception ex)
//        {
//            Debug.WriteLine(ex);
//#else
//        catch (Exception)
//        {
//#endif
//            uow.RollbackTransaction();
//            throw;
//        }
//    }

//    public static async Task<bool> TryDeleteAsync<T>(T item) where T : ITgEf
//    {
//        if (item is not XPLiteObject xpLite) return false;
//        try
//        {
//            if (!xpLite.Session.InTransaction)
//                xpLite.Session.BeginTransaction();
//            await xpLite.Session.DeleteAsync(item);
//            await xpLite.Session.CommitTransactionAsync();
//            return true;
//        }
//#if DEBUG
//        catch (Exception ex)
//        {
//            Debug.WriteLine(ex);
//#else
//        catch (Exception)
//        {
//#endif
//            xpLite.Session.RollbackTransaction();
//            throw;
//        }
//    }

    #endregion

    #region Public and private methods

    //public static void SetXpoDefault()
    //{
    //    if (XpoDefault.DataLayer is null)
    //    {
    //        //XpoDefault.DataLayer = XpoDefault.GetDataLayer(ConnectionString, AutoCreateOption.DatabaseAndSchema);
    //        DataLayer = new(XpoDefault.GetConnectionProvider(ConnectionString, AutoCreateOption.DatabaseAndSchema));
    //        XpoDefault.DataLayer = DataLayer;
    //    }
    //}

    //public static UnitOfWork CreateUnitOfWork()
    //{
    //    SetXpoDefault();
    //    //SimpleDataLayer.SuppressReentrancyAndThreadSafetyCheck = true;
        
    //    //var session = new UnitOfWork(XpoDefault.DataLayer)
    //    var session = new UnitOfWork(DataLayer)
    //    {
    //        //Site = null,
    //        //TrackPropertiesModifications = false,
    //        //CaseSensitive = false,
    //        //IsObjectModifiedOnNonPersistentPropertyChange = null,
    //        //AutoCreateOption = AutoCreateOption.DatabaseAndSchema,
    //        //Connection = null,
    //        //ConnectionString = null,
    //        LockingOption = LockingOption.Optimistic,
    //        //OptimisticLockingReadBehavior = OptimisticLockingReadBehavior.Default,
    //        //IdentityMapBehavior = IdentityMapBehavior.Default
    //    };
    //    return session;
    //}

    #endregion

    #region Public and private methods - Delete

    public static void DeleteNewItems()
    {
        //TgEfAppRepository.Instance.DeleteNewAsync();
        //TgEfDocumentRepository.Instance.DeleteNewAsync();
        //TgEfFilterRepository.Instance.DeleteNewAsync();
        //TgEfMessageRepository.Instance.DeleteNewAsync();
        //TgEfProxyRepository.Instance.DeleteNewAsync();
        //TgEfSourceRepository.Instance.DeleteNewAsync();
        //TgEfVersionRepository.Instance.DeleteNewAsync();
    }

    #endregion

    #region Public and private methods - CreateNew

    //public static TgEfAppEntity CreateNewApp() => new TgEfAppRepository().CreateNew();
    //public static TgEfDocumentModel CreateNewDocument() => TgEfDocumentRepository.Instance.CreateNew(false);
    //public static TgEfFilterModel CreateNewFilter() => TgEfFilterRepository.Instance.CreateNew(false);
    //public static TgEfMessageModel CreateNewMessage() => TgEfMessageRepository.Instance.CreateNew(false);
    //public static TgEfProxyModel CreateNewProxy() => TgEfProxyRepository.Instance.CreateNew(false);
    //public static TgEfSourceModel CreateNewSource() => TgEfSourceRepository.Instance.CreateNew(false);
    //public static TgEfVersionModel CreateNewVersion() => TgEfVersionRepository.Instance.CreateNew(false);

    #endregion

    #region Public and private methods - GetValid

    public static FluentValidation.Results.ValidationResult GetValid(TgEfAppEntity item) => new TgEfAppValidator().Validate(item);
    public static FluentValidation.Results.ValidationResult GetValid(TgEfDocumentEntity item) => new TgEfDocumentValidator().Validate(item);
    public static FluentValidation.Results.ValidationResult GetValid(TgEfFilterEntity item) => new TgEfFilterValidator().Validate(item);
    public static FluentValidation.Results.ValidationResult GetValid(TgEfMessageEntity item) => new TgEfMessageValidator().Validate(item);
    public static FluentValidation.Results.ValidationResult GetValid(TgEfSourceEntity item) => new TgEfSourceValidator().Validate(item);
    public static FluentValidation.Results.ValidationResult GetValid(TgEfProxyEntity item) => new TgEfProxyValidator().Validate(item);
    public static FluentValidation.Results.ValidationResult GetValid(TgEfVersionEntity item) => new TgEfVersionValidator().Validate(item);

    #endregion
}