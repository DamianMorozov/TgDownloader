// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Models.Versions;

namespace TgStorage.Helpers;

public partial class TgStorageHelper
{
	#region Public and private methods

	public SqlTableVersionModel NewEmptyVersion() =>
		new() { Uid = Guid.Empty, Version = 0, Description = string.Empty };

	private void AddVersion<T>(T item, UnitOfWork uow) where T : ISqlTable, new()
	{
		if (item is not SqlTableVersionModel version) return;
		version = new(uow)
		{
			DtCreated = DateTime.Now,
			DtChanged = DateTime.Now,
			Version = version.Version,
			Description = version.Description,
		};
		if (IsValidXpLite(version))
			uow.CommitChanges();
	}

	private void UpdateVersion<T>(T item) where T : ISqlTable, new()
	{
		if (item is not SqlTableVersionModel version) return;
		SqlTableVersionModel? itemDb = GetItemNullable<SqlTableVersionModel>(item.Uid);
		if (itemDb is not { }) return;
		itemDb.DtCreated = version.DtCreated;
		itemDb.DtChanged = DateTime.Now;
		itemDb.Version = version.Version;
		itemDb.Description = version.Description;
		if (IsValidXpLite(itemDb))
		{
			itemDb.Session.Save(itemDb);
			itemDb.Session.CommitTransaction();
		}
	}

	public SqlTableVersionModel? GetVersionNullable(ushort version) =>
		new UnitOfWork()
			.Query<SqlTableVersionModel>()
			.Select(item => item)
			.OrderBy(item => item.Version)
			.FirstOrDefault(item => Equals(item.Version, version));

	public SqlTableVersionModel GetVersion(ushort version) => GetVersionNullable(version) ?? new();

	public SqlTableVersionModel? GetVersionLastNullable() =>
		new UnitOfWork()
			.Query<SqlTableVersionModel>()
			.Select(item => item)
			.OrderBy(item => item.Version)
			.LastOrDefault();

	public SqlTableVersionModel GetVersionLast() => GetVersionLastNullable() ?? new();

	public List<SqlTableVersionModel> GetVersionsList() =>
		new UnitOfWork()
			.Query<SqlTableVersionModel>()
			.Select(item => item)
			.ToList();

	#endregion
}