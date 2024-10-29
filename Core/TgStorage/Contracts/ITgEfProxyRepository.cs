// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Contracts;

public interface ITgEfProxyRepository
{
    TgEfStorageResult<TgEfProxyEntity> GetCurrentProxy(TgEfStorageResult<TgEfAppEntity> storageResult);
    Task<TgEfStorageResult<TgEfProxyEntity>> GetCurrentProxyAsync(TgEfStorageResult<TgEfAppEntity> storageResult);
    Guid GetCurrentProxyUid(TgEfStorageResult<TgEfAppEntity> storageResult);
    Task<Guid> GetCurrentProxyUidAsync(TgEfStorageResult<TgEfAppEntity> storageResult);
}