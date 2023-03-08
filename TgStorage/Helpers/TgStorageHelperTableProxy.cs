// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgCore.Enums;
using TgStorage.Models.Proxies;

namespace TgStorage.Helpers;

public partial class TgStorageHelper
{
	#region Public and private methods

	public SqlTableProxyModel NewEmptyProxy() =>
		new() { Uid = Guid.Empty, HostName = "Test", Port = 404, Type = ProxyType.Http, UserName = "Test", Password = "Test" };

	private void AddProxy<T>(T item, UnitOfWork uow) where T : ISqlTable, new()
	{
		if (item is not SqlTableProxyModel proxy) return;
		proxy = new(uow)
		{
			DtCreated = DateTime.Now,
			DtChanged = DateTime.Now,
			Type = proxy.Type,
			HostName = proxy.HostName,
			Port = proxy.Port,
			Secret = proxy.Secret,
		};
		if (IsValidXpLite(proxy))
			uow.CommitChanges();
	}

	private void UpdateProxy<T>(T item) where T : ISqlTable, new()
	{
		if (item is not SqlTableProxyModel proxy) return;
		SqlTableProxyModel? itemDb = GetItemNullable<SqlTableProxyModel>(item.Uid);
		if (itemDb is not { }) return;
		itemDb.DtCreated = proxy.DtCreated;
		itemDb.DtChanged = DateTime.Now;
		itemDb.Type = proxy.Type;
		itemDb.HostName = proxy.HostName;
		itemDb.Port = proxy.Port;
		itemDb.UserName = proxy.UserName;
		itemDb.Secret = proxy.Secret;
		if (IsValidXpLite(itemDb))
		{
			itemDb.Session.Save(itemDb);
			itemDb.Session.CommitTransaction();
		}
	}

	public SqlTableProxyModel? GetProxyNullable(ProxyType proxyType, string hostName, ushort port) =>
		new UnitOfWork()
			.Query<SqlTableProxyModel>()
			.Select(item => item)
			.FirstOrDefault(item =>
				Equals(item.Type, proxyType) &&
						   Equals(item.HostName, hostName) &&
				Equals(item.Port, port));

	public SqlTableProxyModel GetProxy(ProxyType proxyType, string hostName, ushort port) =>
		GetProxyNullable(proxyType, hostName, port) ?? new();

	#endregion
}