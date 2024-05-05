// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Contracts;

public interface ITgEfProxyRepository
{
    TgEfOperResult<TgEfProxyEntity> GetCurrentProxy(TgEfOperResult<TgEfAppEntity> operResult);
    Task<TgEfOperResult<TgEfProxyEntity>> GetCurrentProxyAsync(TgEfOperResult<TgEfAppEntity> operResult);
    Guid GetCurrentProxyUid(TgEfOperResult<TgEfAppEntity> operResult);
    Task<Guid> GetCurrentProxyUidAsync(TgEfOperResult<TgEfAppEntity> operResult);
}