// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace TgStorage;

public sealed class TgEfContext : DbContext, IDisposable
{
	#region Public and private fields, properties, constructor

	/// <summary> App queries </summary>
	public DbSet<TgEfAppEntity> Apps { get; set; } = default!;
	/// <summary> Document queries </summary>
	public DbSet<TgEfDocumentEntity> Documents { get; set; } = default!;
	/// <summary> Filter queries </summary>
	public DbSet<TgEfFilterEntity> Filters { get; set; } = default!;
	/// <summary> Message queries </summary>
	public DbSet<TgEfMessageEntity> Messages { get; set; } = default!;
	/// <summary> Proxy queries </summary>
	public DbSet<TgEfProxyEntity> Proxies { get; set; } = default!;
	/// <summary> Source queries </summary>
	public DbSet<TgEfSourceEntity> Sources { get; set; } = default!;
	/// <summary> Version queries </summary>
	public DbSet<TgEfVersionEntity> Versions { get; set; } = default!;

	/// <summary> App repository </summary>
	public ITgEfRepository<TgEfAppEntity> AppRepository { get; set; } = default!;
	/// <summary> Document repository </summary>
	public ITgEfRepository<TgEfDocumentEntity> DocumentRepository { get; set; } = default!;
	/// <summary> Filter repository </summary>
	public ITgEfRepository<TgEfFilterEntity> FilterRepository { get; set; } = default!;
	/// <summary> Message repository </summary>
	public ITgEfRepository<TgEfMessageEntity> MessageRepository { get; set; } = default!;
	/// <summary> Proxy repository </summary>
	public ITgEfRepository<TgEfProxyEntity> ProxyRepository { get; set; } = default!;
	/// <summary> Source repository </summary>
	public ITgEfRepository<TgEfSourceEntity> SourceRepository { get; set; } = default!;
	/// <summary> Version repository </summary>
	public ITgEfRepository<TgEfVersionEntity> VersionRepository { get; set; } = default!;

	public TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;
	public TgLogHelper TgLog => TgLogHelper.Instance;

	// Inject options.
	// options: The DbContextOptions{TgEfContext} for the context.
	public TgEfContext(DbContextOptions<TgEfContext> options) : base(options)
	{
		InitRepositories();
	}

	public TgEfContext() : base()
	{
		InitRepositories();
	}

	#endregion

	#region Public and private methods - IDisposable

	/// <summary> Locker object </summary>
	private readonly object _locker = new();
	/// <summary> To detect redundant calls </summary>
	private bool _disposed;

	/// <summary> Finalizer </summary>
	~TgEfContext() => Dispose(false);

	/// <summary> Throw exception if disposed </summary>
	private void CheckIfDisposed()
	{
		if (_disposed)
			throw new ObjectDisposedException($"{nameof(TgEfContext)}: object has been disposed off!");
	}

	/// <summary> Release managed resources </summary>
	private void ReleaseManagedResources()
	{
		//
	}

	/// <summary> Release unmanaged resources </summary>
	private void ReleaseUnmanagedResources()
	{
		//
	}

	/// <summary> Dispose pattern </summary>
	public override void Dispose()
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
#if DEBUG
			Debug.WriteLine($"{nameof(ContextId)} {ContextId}: context disposed");
#endif
			// Base Dispose.
			base.Dispose();
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
	public override ValueTask DisposeAsync()
	{
#if DEBUG
		Debug.WriteLine($"{nameof(ContextId)} {ContextId}: context disposed async");
#endif
		// Base Dispose.
		var result = base.DisposeAsync();
		// Release unmanaged resources.
		Dispose(false);
		// Suppress finalization.
		GC.SuppressFinalize(this);
		// Result.
		return result;
	}

	#endregion

	#region Public and private methods

	private void InitRepositories()
	{
#if DEBUG
		Debug.WriteLine($"{nameof(ContextId)} {ContextId}: init repositories");
#endif
		AppRepository = new TgEfAppRepository(this);
		DocumentRepository = new TgEfDocumentRepository(this);
		FilterRepository = new TgEfFilterRepository(this);
		MessageRepository = new TgEfMessageRepository(this);
		ProxyRepository = new TgEfProxyRepository(this);
		SourceRepository = new TgEfSourceRepository(this);
		VersionRepository = new TgEfVersionRepository(this);
	}

#if DEBUG
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder
			.LogTo(message => Debug.WriteLine($"{nameof(ContextId)} {ContextId}: {message}"), LogLevel.Information)
			.EnableThreadSafetyChecks()
			.EnableDetailedErrors()
			.EnableSensitiveDataLogging()
		;
	}
#endif

	//// Magic string.
	//// Define the model.
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Concurrency tokens
		// https://learn.microsoft.com/en-us/ef/core/modeling/table-splitting
		// This property isn't on the C# class, so we set it up as a "shadow" property and use it for concurrency.
		modelBuilder.Entity<TgEfAppEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgStorageConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfDocumentEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgStorageConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfFilterEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgStorageConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfMessageEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgStorageConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfProxyEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgStorageConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfSourceEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgStorageConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfVersionEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgStorageConstants.ColumnRowVersion);
		// Ingore.
		modelBuilder.Ignore<TgEfEntityBase>();
		modelBuilder.Entity<TgEfAppEntity>().Ignore(x => x.RowVersion);
		modelBuilder.Entity<TgEfDocumentEntity>().Ignore(x => x.RowVersion);
		modelBuilder.Entity<TgEfFilterEntity>().Ignore(x => x.RowVersion);
		modelBuilder.Entity<TgEfMessageEntity>().Ignore(x => x.RowVersion);
		modelBuilder.Entity<TgEfProxyEntity>().Ignore(x => x.RowVersion);
		modelBuilder.Entity<TgEfSourceEntity>().Ignore(x => x.RowVersion);
		modelBuilder.Entity<TgEfVersionEntity>().Ignore(x => x.RowVersion);
		// FK.
		modelBuilder.Entity<TgEfAppEntity>()
			.HasOne(app => app.Proxy)
			.WithMany(proxy => proxy.Apps)
			.HasForeignKey(app => app.ProxyUid)
			.HasPrincipalKey(proxy => proxy.Uid);
		modelBuilder.Entity<TgEfDocumentEntity>()
			.HasOne(document => document.Source)
			.WithMany(source => source.Documents)
			.HasForeignKey(document => document.SourceId)
			.HasPrincipalKey(source => source.Id)
			.IsRequired();
		modelBuilder.Entity<TgEfMessageEntity>()
			.HasOne(message => message.Source)
			.WithMany(source => source.Messages)
			.HasForeignKey(message => message.SourceId)
			.HasPrincipalKey(source => source.Id)
			.IsRequired();
		// Result.
		base.OnModelCreating(modelBuilder);
	}

	public IEnumerable<ITgDbEntity> GetTableModels()
	{
		yield return new TgEfAppEntity();
		yield return new TgEfDocumentEntity();
		yield return new TgEfFilterEntity();
		yield return new TgEfProxyEntity();
		yield return new TgEfSourceEntity();
		yield return new TgEfVersionEntity();
	}

	public IEnumerable<Type> GetTableTypes()
	{
		yield return typeof(TgEfAppEntity);
		yield return typeof(TgEfDocumentEntity);
		yield return typeof(TgEfFilterEntity);
		yield return typeof(TgEfProxyEntity);
		yield return typeof(TgEfSourceEntity);
		yield return typeof(TgEfVersionEntity);
	}

	public TgEfOperResult<TgEfProxyEntity> GetCurrentProxy()
	{
		TgEfOperResult<TgEfAppEntity> operResultApp = AppRepository.GetFirst(isNoTracking: true);
		if (operResultApp.NotExist)
			return new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExist);
		TgEfOperResult<TgEfProxyEntity> operResultProxy = ProxyRepository.Get(
			new TgEfProxyEntity { Uid = operResultApp.Item.ProxyUid ?? Guid.Empty }, isNoTracking: true);
		return operResultProxy.IsExist ? operResultProxy : new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExist);
	}

	public async Task<TgEfOperResult<TgEfProxyEntity>> GetCurrentProxyAsync()
	{
		TgEfOperResult<TgEfAppEntity> operResultApp = await AppRepository.GetFirstAsync(isNoTracking: true);
		if (operResultApp.NotExist)
			return new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExist);
		TgEfOperResult<TgEfProxyEntity> operResultProxy = await ProxyRepository.GetAsync(
			new TgEfProxyEntity { Uid = operResultApp.Item.ProxyUid ?? Guid.Empty }, isNoTracking: true);
		return operResultProxy.IsExist ? operResultProxy : new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExist);
	}

	public Guid GetCurrentProxyUid() => GetCurrentProxy().Item.Uid;

	public async Task<Guid> GetCurrentProxyUidAsync() => (await GetCurrentProxyAsync()).Item.Uid;

	public string ToConsoleStringShort(TgEfSourceEntity source) =>
		$"{GetPercentCountString(source)} | {(source.IsAutoUpdate ? "a | " : "")} | {source.Id} | " +
		$"{(string.IsNullOrEmpty(source.UserName) ? "" : TgDataFormatUtils.GetFormatString(source.UserName, 30))} | " +
		$"{(string.IsNullOrEmpty(source.Title) ? "" : TgDataFormatUtils.GetFormatString(source.Title, 30))} | " +
		$"{source.FirstId} {TgLocaleHelper.Instance.From} {source.Count} {TgLocaleHelper.Instance.Messages}";

	public string ToConsoleString(TgEfSourceEntity source) =>
		$"{GetPercentCountString(source)} | {(source.IsAutoUpdate ? "a" : " ")} | {source.Id} | " +
		$"{TgDataFormatUtils.GetFormatString(source.UserName, 30)} | " +
		$"{TgDataFormatUtils.GetFormatString(source.Title, 30)} | " +
		$"{source.FirstId} {TgLocaleHelper.Instance.From} {source.Count} {TgLocaleHelper.Instance.Messages}";

	public string ToConsoleString(TgEfVersionEntity version) =>
		$"{version.Version}	{version.Description}";

	public string GetPercentCountString(TgEfSourceEntity source)
	{
		float percent = source.Count <= source.FirstId ? 100 : source.FirstId > 1 ? (float)source.FirstId * 100 / source.Count : 0;
		if (IsPercentCountAll(source))
			return "100.00 %";
		return percent > 9 ? $" {percent:00.00} %" : $"  {percent:0.00} %";
	}

	public bool IsPercentCountAll(TgEfSourceEntity source) => source.Count <= source.FirstId;

	/// <summary>
	/// Check table exists.
	/// </summary>
	/// <param name="tableName"></param>
	/// <returns></returns>
	public bool IsTableExists(string tableName)
	{
		string? result = Database.SqlQuery<string>(
			$"SELECT [name] AS [Value] FROM [sqlite_master]").SingleOrDefault(x => x == tableName);
		return tableName == result;
	}

	public static short LastVersion => 19;

	public void VersionsView()
	{
		TgEfOperResult<TgEfVersionEntity> operResult = VersionRepository.GetEnumerable(TgEnumTableTopRecords.All, isNoTracking: true);
		if (operResult.IsExist)
		{
			foreach (TgEfVersionEntity version in operResult.Items)
			{
				TgLog.WriteLine($" {version.Version:00} | {version.Description}");
			}
		}
	}

	public void FiltersView()
	{
		TgEfOperResult<TgEfFilterEntity> operResult = FilterRepository.GetEnumerable(TgEnumTableTopRecords.All, isNoTracking: true);
		if (operResult.IsExist)
		{
			foreach (TgEfFilterEntity filter in operResult.Items)
			{
				TgLog.WriteLine($"{filter}");
			}
		}
	}

	public (bool IsSuccess, string FileName) BackupDb()
	{
		if (File.Exists(TgAppSettings.AppXml.FileStorage))
		{
			DateTime dt = DateTime.Now;
			string fileBackup =
				$"{Path.GetDirectoryName(TgAppSettings.AppXml.FileStorage)}\\" +
				$"{Path.GetFileNameWithoutExtension(TgAppSettings.AppXml.FileStorage)}_{dt:yyyyMMdd}_{dt:HHmmss}.bak";
			File.Copy(TgAppSettings.AppXml.FileStorage, fileBackup);
			return (File.Exists(fileBackup), fileBackup);
		}
		return (false, string.Empty);
	}

	#endregion
}