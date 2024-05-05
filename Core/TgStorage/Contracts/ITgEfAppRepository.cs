namespace TgStorage.Contracts;

public interface ITgEfAppRepository
{
	TgEfOperResult<TgEfAppEntity> GetCurrentApp();
	Task<TgEfOperResult<TgEfAppEntity>> GetCurrentAppAsync();
	Guid GetCurrentAppUid();
	Task<Guid> GetCurrentAppUidAsync();
}