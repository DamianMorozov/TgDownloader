// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Versions;

/// <summary> View-model for TgSqlTableSourceModel </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgEfVersionViewModel : TgViewModelBase
{
	#region Public and private fields, properties, constructor

	private TgEfVersionRepository VersionRepository { get; } = new(TgEfUtils.EfContext);
	public TgEfVersionEntity Item { get; set; } = default!;

	[DefaultValue(short.MaxValue)]
	public short Version { get => Item.Version; set => Item.Version = value; }

	[DefaultValue("New version")]
	public string Description { get => Item.Description; set => Item.Description = value; }

	public TgEfVersionViewModel(TgEfVersionEntity item) : base()
	{
		Default(item);
	}

	public TgEfVersionViewModel() : base()
	{
		TgEfVersionEntity item = VersionRepository.GetNew(isReadOnly: false).Item;
		Default(item);
	}

	#endregion

	#region Public and private methods

	private void Default(TgEfVersionEntity item)
	{
		Item = item;
		Description = item.Description;
	}

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