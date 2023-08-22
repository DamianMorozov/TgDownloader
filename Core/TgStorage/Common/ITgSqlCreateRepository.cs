// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

public interface ITgSqlCreateRepository<out T> where T: ITgSqlTable, new()
{
    public T CreateNew(bool isCreateSession);
}