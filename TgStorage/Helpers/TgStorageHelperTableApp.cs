// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Models.Apps;

namespace TgStorage.Helpers;

public partial class TgStorageHelper
{
	#region Public and private methods

	public SqlTableAppModel NewEmptyApp() =>
		new() { Uid = Guid.Empty, ApiHash = Guid.Empty.ToString().Replace("-", ""), PhoneNumber = "+12345678999" };

	private void AddApp<T>(T item, UnitOfWork uow) where T : ISqlTable, new()
	{
		if (item is not SqlTableAppModel app) return;
		app = new(uow)
		{
			DtCreated = DateTime.Now,
			DtChanged = DateTime.Now,
			ApiHash = app.ApiHash,
			PhoneNumber = app.PhoneNumber,
			ProxyUid = app.ProxyUid,
		};
		if (IsValidXpLite(app))
			uow.CommitChanges();
	}

	private void UpdateApp<T>(T item) where T : ISqlTable, new()
	{
		if (item is not SqlTableAppModel app) return;
		SqlTableAppModel? itemDb = GetItemNullable<SqlTableAppModel>(item.Uid);
		if (itemDb is not { }) return;
		itemDb.DtCreated = app.DtCreated;
		itemDb.DtChanged = DateTime.Now;
		itemDb.ApiHash = app.ApiHash;
		itemDb.PhoneNumber = app.PhoneNumber;
		itemDb.ProxyUid = app.ProxyUid;
		if (IsValidXpLite(itemDb))
		{
			itemDb.Session.Save(itemDb);
			itemDb.Session.CommitTransaction();
		}
	}

	public SqlTableAppModel? GetAppNullable(string apiHash) =>
		new UnitOfWork()
			.Query<SqlTableAppModel>()
			.Select(item => item)
			.FirstOrDefault(item => Equals(item.ApiHash, apiHash));

	public SqlTableAppModel GetApp(string apiHash) => GetAppNullable(apiHash) ?? new();

	public SqlTableAppModel? GetAppNullable() =>
		new UnitOfWork()
			.Query<SqlTableAppModel>()
			.Select(item => item)
			.FirstOrDefault(item => !Equals(item.ApiHash, Guid.Empty.ToString().Replace("-", "")));

	public SqlTableAppModel GetApp() => GetAppNullable() ?? new();

	#endregion
}