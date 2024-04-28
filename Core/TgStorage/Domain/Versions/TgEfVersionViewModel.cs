// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Versions;

/// <summary> View for TgSqlTableSourceModel </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgEfVersionViewModel : TgViewModelBase
{
	#region Public and private fields, properties, constructor

	public TgEfVersionEntity Item { get; set; }

	public Guid VersionUid
	{
		get => Item.Uid;
		set
		{
			TgEfOperResult<TgEfVersionEntity> operResult =
				EfContext.VersionRepository.Get(new TgEfVersionEntity { Uid = value }, isNoTracking: true);
			Item = operResult.IsExists
				? operResult.Item
				: EfContext.VersionRepository.GetNew(isNoTracking: true).Item;
		}
	}

	[DefaultValue(short.MaxValue)]
	public short Version { get => Item.Version; set => Item.Version = value; }

	[DefaultValue("New version")]
	public string Description { get => Item.Description; set => Item.Description = value; }

	public TgEfVersionViewModel(TgEfVersionEntity item)
	{
		Item = item;
		Description = item.Description;
	}

	public TgEfVersionViewModel(TgEfContext efContext) : this(efContext.VersionRepository.CreateNew().Item) { }

	#endregion

	#region Public and private methods

	public override string ToString() => $"{Version} | {Description}";

	public override string ToDebugString() => $"{base.ToDebugString()} | {Version} | {Description}";

	/// <summary> Set new version </summary>
	/// <param name="version"></param>
	/// <param name="description"></param>
	public void SetSource(short version, string description)
	{
		Version = version;
		Description = description;
	}

	#endregion
}