// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo;
using DevExpress.Xpo.DB;

namespace TgStorage;

/// <summary>
/// SQL data storage context helper.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgXpoContext : IDisposable
{
	#region Public and private fields, properties, constructor

    public TgEnumStorageType StorageType { get; }
    public string ConnectionStringLowerCase { get; }
    public string ConnectionStringUpperCase { get; }
    public ThreadSafeDataLayer? DataLayerUpperCase { get; private set; }
    public ThreadSafeDataLayer? DataLayerLowerCase { get; private set; }

	public TgXpoAppRepository AppRepository { get; }

    public TgXpoDocumentRepository DocumentRepository { get; }

	public TgXpoFilterRepository FilterRepository { get; }

	public TgXpoMessageRepository MessageRepository { get; }

	public TgXpoProxyRepository ProxyRepository { get; }

	public TgXpoSourceRepository SourceRepository { get; }

	public TgXpoVersionRepository VersionRepository { get; }

	public TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;

    public TgLogHelper TgLog => TgLogHelper.Instance;
    
    public TgLocaleHelper TgLocale => TgLocaleHelper.Instance;

    public bool IsReady =>
        TgAppSettings.AppXml.IsExistsFileStorage &&
        IsTableExists(TgStorageConstants.TableApps) && IsTableExists(TgStorageConstants.TableDocuments) &&
        IsTableExists(TgStorageConstants.TableFilters) && IsTableExists(TgStorageConstants.TableMessages) &&
        IsTableExists(TgStorageConstants.TableProxies) && IsTableExists(TgStorageConstants.TableSources) &&
        IsTableExists(TgStorageConstants.TableVersions);

    public bool IsNotReady => !IsReady;

	public TgXpoContext(TgEnumStorageType storageType)
	{
		StorageType = storageType;
		switch (StorageType)
		{
			case TgEnumStorageType.Test:
				ConnectionStringLowerCase = TgXpoSqLiteLowerProvider.GetConnectionStringLower(TgAppSettings.AppXml.TestStorage);
				ConnectionStringUpperCase = TgXpoSqLiteUpperProvider.GetConnectionStringUpper(TgAppSettings.AppXml.TestStorage);
				break;
			case TgEnumStorageType.Prod:
				ConnectionStringLowerCase = TgXpoSqLiteLowerProvider.GetConnectionStringLower(TgAppSettings.AppXml.FileStorage);
				ConnectionStringUpperCase = TgXpoSqLiteUpperProvider.GetConnectionStringUpper(TgAppSettings.AppXml.FileStorage);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(storageType));
		}

		AppRepository = new(this);
		DocumentRepository = new(this);
		FilterRepository = new(this);
		MessageRepository = new(this);
		ProxyRepository = new(this);
		SourceRepository = new(this);
		VersionRepository = new(this);

		CreateOrConnectDb();
	}

	#endregion

	#region Public and private methods - IDisposable

	/// <summary> Locker object </summary>
	private readonly object _locker = new();
	/// <summary> To detect redundant calls </summary>
	private bool _disposed;

	/// <summary> Finalizer </summary>
	~TgXpoContext() => Dispose(false);

	/// <summary> Throw exception if disposed </summary>
	private void CheckIfDisposed()
	{
		if (_disposed)
			throw new ObjectDisposedException($"{nameof(TgEfContext)}: object has been disposed off!");
	}

	/// <summary> Release managed resources </summary>
	private void ReleaseManagedResources()
	{
		DataLayerLowerCase?.Dispose();
		DataLayerLowerCase = null;
		DataLayerUpperCase?.Dispose();
		DataLayerUpperCase = null;
	}

	/// <summary> Release unmanaged resources </summary>
	private void ReleaseUnmanagedResources()
	{
		//
	}

	/// <summary> Dispose pattern </summary>
	public void Dispose()
	{
		Dispose(true);
		// Suppress finalization.
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (_disposed)
			return;
		lock (_locker)
		{
			// Release managed resources.
			if (disposing)
				ReleaseManagedResources();
			// Release unmanaged resources.
			ReleaseUnmanagedResources();
			// Flag.
			_disposed = true;
		}
	}

	/// <summary> Dispose async pattern | await using </summary>
	public ValueTask DisposeAsync()
	{
		// Release unmanaged resources.
		Dispose(false);
		// Suppress finalization.
		GC.SuppressFinalize(this);
		// Result.
		return ValueTask.CompletedTask;
	}

	#endregion

	#region Public and private methods

	public string ToDebugString() => $"{TgCommonUtils.GetIsReady(IsReady)}";

    public void CreateOrConnectDb()
    {
	    CreateOrConnectDbAsync().GetAwaiter().GetResult();
    }

    public async Task CreateOrConnectDbAsync()
    {
	    SetDataLayer();
        await CheckTablesAsync();
    }

    public void SetDataLayer()
    {
	    if (DataLayerLowerCase is null)
	    {
		    DataLayerLowerCase = new ThreadSafeDataLayer(XpoDefault.GetConnectionProvider(
			    ConnectionStringLowerCase, AutoCreateOption.DatabaseAndSchema));
	    }
	    if (DataLayerUpperCase is null)
	    {
		    DataLayerUpperCase = new ThreadSafeDataLayer(XpoDefault.GetConnectionProvider(
			    ConnectionStringUpperCase, AutoCreateOption.DatabaseAndSchema));
	    }
    }

    public UnitOfWork CreateUnitOfWork(bool isLowerCase = false)
    {
	    UnitOfWork uow = new(isLowerCase ? DataLayerLowerCase : DataLayerUpperCase)
	    {
		    //Site = null,
		    //TrackPropertiesModifications = false,
		    //CaseSensitive = false,
		    //IsObjectModifiedOnNonPersistentPropertyChange = null,
		    //AutoCreateOption = AutoCreateOption.DatabaseAndSchema,
		    //Connection = null,
		    LockingOption = LockingOption.Optimistic,
		    //OptimisticLockingReadBehavior = OptimisticLockingReadBehavior.Default,
		    //IdentityMapBehavior = IdentityMapBehavior.Default
	    };
	    return uow;
    }


    /// <summary>
    /// Update structures of tables.
    /// </summary>
    private async Task CheckTablesAsync()
    {
        if (!await CheckTableAppsAsync())
            throw new(TgLocale.TablesAppsException);
        if (!await CheckTableDocumentsAsync())
            throw new(TgLocale.TablesDocumentsException);
        if (!await CheckTableFiltersAsync())
            throw new(TgLocale.TablesFiltersException);
        if (!await CheckTableMessagesAsync())
            throw new(TgLocale.TablesMessagesException);
        if (!await CheckTableSourcesAsync())
            throw new(TgLocale.TablesSourcesException);
        if (!await CheckTableProxiesAsync())
            throw new(TgLocale.TablesProxiesException);
        if (!await CheckTableVersionsAsync())
            throw new(TgLocale.TablesVersionsException);
        FillTableVersions();
    }

    private async Task<bool> CheckTableAsync<T>(TgXpoRepositoryBase<T> repository) where T : XPLiteObject, ITgDbEntity, new()
    {
        var operResult = await repository.CreateNewAsync();
        if (operResult.NotExist) return false;
        operResult = await repository.GetNewAsync();
        if (operResult.NotExist) return false;
        operResult = await repository.SaveAsync(operResult.Item);
        if (operResult.State != TgEnumEntityState.IsSaved) return false;
        operResult = await repository.DeleteAsync(operResult.Item, isSkipFind: false);
        return operResult.State == TgEnumEntityState.IsDeleted;
    }

    public Task<bool> CheckTableAppsAsync() => CheckTableAsync(AppRepository);

    public Task<bool> CheckTableDocumentsAsync() => CheckTableAsync(DocumentRepository);

    public Task<bool> CheckTableFiltersAsync() => CheckTableAsync(FilterRepository);

    public Task<bool> CheckTableMessagesAsync() => CheckTableAsync(MessageRepository);

    public Task<bool> CheckTableProxiesAsync() => CheckTableAsync(ProxyRepository);

    public Task<bool> CheckTableSourcesAsync() => CheckTableAsync(SourceRepository);

    public Task<bool> CheckTableVersionsAsync() => CheckTableAsync(VersionRepository);

    public void FillTableVersions()
    {
        bool isLast = false;
        while (!isLast)
        {
            TgXpoVersionEntity versionLast = !IsTableExists(TgStorageConstants.TableVersions)
                ? new() : VersionRepository.GetItemLastAsync().GetAwaiter().GetResult().Item;
            if (Equals(versionLast.Version, short.MaxValue))
                versionLast.Version = 0;
            switch (versionLast.Version)
            {
                case 0:
                    TgXpoVersionEntity version1 = new() { Version = 1, Description = "Added versions table" };
                    VersionRepository.SaveAsync(version1).GetAwaiter().GetResult();
                    break;
                case 1:
                    TgXpoVersionEntity version2 = new() { Version = 2, Description = "Added apps table" };
                    VersionRepository.SaveAsync(version2).GetAwaiter().GetResult();
                    break;
                case 2:
                    TgXpoVersionEntity version3 = new() { Version = 3, Description = "Added documents table" };
                    VersionRepository.SaveAsync(version3).GetAwaiter().GetResult();
                    break;
                case 3:
                    TgXpoVersionEntity version4 = new() { Version = 4, Description = "Added filters table" };
                    VersionRepository.SaveAsync(version4).GetAwaiter().GetResult();
                    break;
                case 4:
                    TgXpoVersionEntity version5 = new() { Version = 5, Description = "Added messages table" };
                    VersionRepository.SaveAsync(version5).GetAwaiter().GetResult();
                    break;
                case 5:
                    TgXpoVersionEntity version6 = new() { Version = 6, Description = "Added proxies table" };
                    VersionRepository.SaveAsync(version6).GetAwaiter().GetResult();
                    break;
                case 6:
                    TgXpoVersionEntity version7 = new() { Version = 7, Description = "Added sources table" };
                    VersionRepository.SaveAsync(version7).GetAwaiter().GetResult();
                    break;
                case 7:
                    TgXpoVersionEntity version8 = new() { Version = 8, Description = "Added source settings table" };
                    VersionRepository.SaveAsync(version8).GetAwaiter().GetResult();
                    break;
                case 8:
                    TgXpoVersionEntity version9 = new() { Version = 9, Description = "Upgrade versions table" };
                    VersionRepository.SaveAsync(version9).GetAwaiter().GetResult();
                    break;
                case 9:
                    TgXpoVersionEntity version10 = new() { Version = 10, Description = "Upgrade apps table" };
                    VersionRepository.SaveAsync(version10).GetAwaiter().GetResult();
                    break;
                case 10:
                    TgXpoVersionEntity version11 = new() { Version = 11, Description = "Upgrade storage on XPO framework" };
                    VersionRepository.SaveAsync(version11).GetAwaiter().GetResult();
                    break;
                case 11:
                    TgXpoVersionEntity version12 = new() { Version = 12, Description = "Upgrade apps table" };
                    VersionRepository.SaveAsync(version12).GetAwaiter().GetResult();
                    break;
                case 12:
                    TgXpoVersionEntity version13 = new() { Version = 13, Description = "Upgrade documents table" };
                    VersionRepository.SaveAsync(version13).GetAwaiter().GetResult();
                    break;
                case 13:
                    TgXpoVersionEntity version14 = new() { Version = 14, Description = "Upgrade filters table" };
                    VersionRepository.SaveAsync(version14).GetAwaiter().GetResult();
                    break;
                case 14:
                    TgXpoVersionEntity version15 = new() { Version = 15, Description = "Upgrade messages table" };
                    VersionRepository.SaveAsync(version15).GetAwaiter().GetResult();
                    break;
                case 15:
                    TgXpoVersionEntity version16 = new() { Version = 16, Description = "Upgrade proxies table" };
                    VersionRepository.SaveAsync(version16).GetAwaiter().GetResult();
                    break;
                case 16:
                    TgXpoVersionEntity version17 = new() { Version = 17, Description = "Upgrade sources table" };
                    VersionRepository.SaveAsync(version17).GetAwaiter().GetResult();
                    break;
                case 17:
                    TgXpoVersionEntity version18 = new() { Version = 18, Description = "Upgrade sources table" };
                    VersionRepository.SaveAsync(version18).GetAwaiter().GetResult();
                    break;
                case 18:
                    TgXpoVersionEntity version19 = new() { Version = 19, Description = "Upgrade UID in all tables" };
                    VersionRepository.SaveAsync(version19).GetAwaiter().GetResult();
                    break;
            }
            if (versionLast.Version >= TgEfContext.LastVersion)
                isLast = true;
        }
    }

    public async Task DeleteTablesAsync()
    {
        await DeleteTableAsync<TgXpoAppEntity>(TgStorageConstants.TableApps);
        await DeleteTableAsync<TgXpoProxyEntity>(TgStorageConstants.TableProxies);
        await DeleteTableAsync<TgXpoFilterEntity>(TgStorageConstants.TableFilters);
        await DeleteTableAsync<TgXpoDocumentEntity>(TgStorageConstants.TableDocuments);
        await DeleteTableAsync<TgXpoMessageEntity>(TgStorageConstants.TableMessages);
        await DeleteTableAsync<TgXpoSourceEntity>(TgStorageConstants.TableSources);
        await DeleteTableAsync<TgXpoVersionEntity>(TgStorageConstants.TableVersions);
        await DeleteTableAsync<TgXpoTableEmpty>(TgStorageConstants.XPObjectType);
    }

    /// <summary>
    /// Truncate sql table by name.
    /// </summary>
    /// <param name="tableName"></param>
    public async Task<TgXpoOperResult<T>> TruncateTableAsync<T>(string tableName) where T : XPLiteObject, ITgDbEntity, new() =>
        await TryExecuteAsync<T>(TgStorageConstants.UidTruncate, $"TRUNCATE TABLE {tableName};");

    /// <summary>
    /// Delete sql table by name.
    /// </summary>
    /// <param name="tableName"></param>
    public async Task<TgXpoOperResult<T>> DeleteTableAsync<T>(string tableName) where T : XPLiteObject, ITgDbEntity, new() =>
        await TryExecuteAsync<T>(TgStorageConstants.UidDrop, $"DROP TABLE IF EXISTS {tableName};");

    /// <summary>
	/// Check table exists.
	/// </summary>
	/// <param name="tableName"></param>
	/// <returns></returns>
	public bool IsTableExists(string tableName)
    {
        using UnitOfWork uow = CreateUnitOfWork();
        SelectedData data = uow.ExecuteQuery($"SELECT [type], [name] FROM [sqlite_master] WHERE [name]='{tableName}'");
        if (data is not null && data.ResultSet[0].Rows.Any())
        {
            string name = data.ResultSet[0].Rows[0].Values[1].ToString() ?? string.Empty;
            return Equals(name, tableName);
        }
        return false;
    }

    public IEnumerable<ITgDbEntity> GetTableModels()
    {
        yield return new TgXpoAppEntity();
        yield return new TgXpoDocumentEntity();
        yield return new TgXpoFilterEntity();
        yield return new TgXpoProxyEntity();
        yield return new TgXpoSourceEntity();
        yield return new TgXpoVersionEntity();
    }

    public IEnumerable<Type> GetTableTypes()
    {
        yield return typeof(TgXpoAppEntity);
        yield return typeof(TgXpoDocumentEntity);
        yield return typeof(TgXpoFilterEntity);
        yield return typeof(TgXpoProxyEntity);
        yield return typeof(TgXpoSourceEntity);
        yield return typeof(TgXpoVersionEntity);
    }

    private bool _isPatchingUid = false;

	private async Task<TgXpoOperResult<T>> GetOperResultBeforeChangeAsync<T>(T item, Session uow)
		where T : XPLiteObject, ITgDbEntity, new()
	{
		TgXpoOperResult<T> operResult = new TgXpoOperResult<T>();
		switch (item)
		{
			case TgXpoAppEntity:
				TgXpoOperResult<TgXpoAppEntity> operResultApp = await AppRepository.GetAsync(item.Uid, uow);
				// Patch UpperCase in UID.
				//if (!_isPatchingUid && operResultApp is { IsExist: true, Item.LetterCase: TgEnumLetterCase.LowerCase })
				//{
				//	try
				//	{
				//		//operResultApp = await AppRepository.GetAsync(item.Uid, uow);
				//		_isPatchingUid = true;
				//		await AppRepository.DeleteNewAsync();
				//		TgXpoAppEntity app = new();
				//		app.Fill(operResultApp.Item);
				//		await AppRepository.DeleteAsync(operResultApp.Item, isSkipFind: true);
				//		//operResultApp.Item.Delete();
				//		await AppRepository.SaveAsync(app);
				//		//app.Save();
				//		operResultApp = await AppRepository.GetAsync(app.Uid, uow);
				//	}
				//	finally
				//	{
				//		_isPatchingUid = false;
				//	}
				//}
				operResult.State = operResultApp.State;
				operResult.Item = operResultApp.Item as T ?? new();
				break;
			case TgXpoDocumentEntity:
				TgXpoOperResult<TgXpoDocumentEntity> operResultDocument = await DocumentRepository.GetAsync(item.Uid, uow);
				operResult.State = operResultDocument.State;
				operResult.Item = operResultDocument.Item as T ?? new();
				break;
			case TgXpoFilterEntity:
				TgXpoOperResult<TgXpoFilterEntity> operResultFilter = await FilterRepository.GetAsync(item.Uid, uow);
				operResult.State = operResultFilter.State;
				operResult.Item = operResultFilter.Item as T ?? new();
				break;
			case TgXpoMessageEntity:
				TgXpoOperResult<TgXpoMessageEntity> operResultMessage = await MessageRepository.GetAsync(item.Uid, uow);
				operResult.State = operResultMessage.State;
				operResult.Item = operResultMessage.Item as T ?? new();
				break;
			case TgXpoProxyEntity:
				TgXpoOperResult<TgXpoProxyEntity> operResultProxy = await ProxyRepository.GetAsync(item.Uid, uow);
				operResult.State = operResultProxy.State;
				operResult.Item = operResultProxy.Item as T ?? new();
				break;
			case TgXpoSourceEntity:
				TgXpoOperResult<TgXpoSourceEntity> operResultSource = await SourceRepository.GetAsync(item.Uid, uow);
				operResult.State = operResultSource.State;
				operResult.Item = operResultSource.Item as T ?? new();
				break;
			case TgXpoVersionEntity:
				TgXpoOperResult<TgXpoVersionEntity> operResultVersion = await VersionRepository.GetAsync(item.Uid, uow);
				operResult.State = operResultVersion.State;
				operResult.Item = operResultVersion.Item as T ?? new();
				break;
			default:
				throw new ArgumentException(nameof(item));
		}
        return operResult;
	}

	public async Task<TgXpoOperResult<T>> TryLockReaderWriterLockSlimAsync<T>(Guid uid, Func<Task<TgXpoOperResult<T>>> func)
		where T : XPLiteObject, ITgDbEntity, new()
	{
		try
		{
			TgStorageUtils.ReaderWriterLockSlimItems.EnterUpgradeableReadLock();
			string key = GetKey<T>(uid);
			if (!TgStorageUtils.ListItems.Contains(key))
			{
				// Vocabulary filling and blocking.
				TgStorageUtils.ReaderWriterLockSlimItems.EnterWriteLock();
				try
				{
					TgStorageUtils.ListItems.Add(key);
					// Asynchronous operation.
					return await func();
				}
				finally
				{
					// Dictionary cleaning and unlocking.
					TgStorageUtils.ListItems.Remove(key);
					TgStorageUtils.ReaderWriterLockSlimItems.ExitWriteLock();
				}
			}
		}
		finally
		{
			TgStorageUtils.ReaderWriterLockSlimItems.ExitUpgradeableReadLock();
		}
		return new TgXpoOperResult<T>(TgEnumEntityState.Unknown, new T());
	}

	public async Task<TgXpoOperResult<T>> TryLockSemaphoreSlimAsync<T>(Guid uid, Func<Task<TgXpoOperResult<T>>> func) where T : XPLiteObject, ITgDbEntity, new()
	{
		try
		{
			await TgStorageUtils.SemaphoreSlimItems.WaitAsync();
			string key = GetKey<T>(uid);
			if (!TgStorageUtils.ListItems.Contains(key))
			{
				// Vocabulary filling and blocking.
				try
				{
					TgStorageUtils.ListItems.Add(key);
					// Asynchronous operation.
					return await func();
				}
				finally
				{
					// Dictionary cleaning and unlocking.
					TgStorageUtils.ListItems.Remove(key);
				}
			}
		}
		finally
		{
			TgStorageUtils.SemaphoreSlimItems.Release();
		}
		return new TgXpoOperResult<T>(TgEnumEntityState.Unknown, new T());
	}

	private string GetKey<T>(Guid uid) => $"{typeof(T).ToString().Split('.').LastOrDefault() ?? string.Empty}_{uid.ToString().ToUpper()}";

	public async Task<TgXpoOperResult<T>> TrySaveAsync<T>(T item) where T : XPLiteObject, ITgDbEntity, new()
	{
		using UnitOfWork uow = CreateUnitOfWork();
		try
		{
			await using TgXpoOperResult<T> operResult = await GetOperResultBeforeChangeAsync(item, uow);
			operResult.Item.Fill(item);
			FluentValidation.Results.ValidationResult validationResult = TgStorageUtils.GetXpValid(operResult.Item);
			if (!validationResult.IsValid)
				return new TgXpoOperResult<T>(TgEnumEntityState.NotSaved, operResult.Item);
			if (!uow.InTransaction)
				uow.BeginTransaction();
			await uow.SaveAsync(operResult.Item);
			await uow.CommitTransactionAsync();
			return new TgXpoOperResult<T>(TgEnumEntityState.IsSaved, operResult.Item);
		}
		catch (Exception ex)
		{
#if DEBUG
			Debug.WriteLine(ex);
			Console.WriteLine(ex);
#endif
			uow.RollbackTransaction();
#if DEBUG
			throw;
#endif
		}
		finally
		{
			uow.Disconnect();
		}
	}

	public async Task<TgXpoOperResult<T>> TryDeleteAsync<T>(T item, bool isSkipFind) where T : XPLiteObject, ITgDbEntity, new()
	{
		using Session uow = !isSkipFind ? CreateUnitOfWork() : item.Session;
		TgXpoOperResult<T> operResult = default!;
		try
		{
			if (!isSkipFind)
			{
				operResult = await GetOperResultBeforeChangeAsync(item, uow);
				if (operResult.NotExist)
					return operResult;
			}
			else
			{
				operResult = new TgXpoOperResult<T>(TgEnumEntityState.IsExist, item);
			}
			if (!uow.InTransaction)
				uow.BeginTransaction();
			await uow.DeleteAsync(operResult.Item);
			await uow.CommitTransactionAsync();
			return new TgXpoOperResult<T>(TgEnumEntityState.IsDeleted);
		}
		catch (Exception ex)
		{
#if DEBUG
			Debug.WriteLine(ex);
			Console.WriteLine(ex);
#endif
			uow.RollbackTransaction();
#if DEBUG
			throw;
#endif
		}
		finally
		{
			await operResult.DisposeAsync();
			uow.Disconnect();
		}
	}

	public async Task<TgXpoOperResult<T>> TryExecuteAsync<T>(Guid uid, string cmd) where T : XPLiteObject, ITgDbEntity, new()
	{
		using UnitOfWork uow = CreateUnitOfWork();
		try
		{
			if (!uow.InTransaction)
				uow.BeginTransaction();
			await uow.ExecuteNonQueryAsync(cmd);
			await uow.CommitChangesAsync();
			await uow.CommitTransactionAsync();
			return new TgXpoOperResult<T>(TgEnumEntityState.IsExecuted);
		}
		catch (Exception ex)
		{
#if DEBUG
			Debug.WriteLine(ex);
			Console.WriteLine(ex);
#endif
			uow.RollbackTransaction();
#if DEBUG
			throw;
#endif
		}
		finally
		{
			uow.Disconnect();
		}
	}
	
	#endregion
}