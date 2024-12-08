// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Stories;

/// <summary> View-model for TgEfStoryEntity </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgEfStoryViewModel : TgViewModelBase
{
	#region Public and private fields, properties, constructor

	public TgEfStoryRepository StoryRepository { get; } = new(TgEfUtils.EfContext);
	public TgEfStoryEntity Item { get; set; } = default!;

	public Guid Uid
	{
		get => Item.Uid;
		set
		{
			TgEfStorageResult<TgEfStoryEntity> storageResult = StoryRepository.Get(
				new() { Uid = value }, isNoTracking: false);
			Item = storageResult.IsExists
				? storageResult.Item
				: StoryRepository.GetNew(isNoTracking: false).Item;
		}
	}

	[DefaultValue(0)]
	public long Id { get => Item.Id; set => Item.Id = value; }
	[DefaultValue("")]
	public DateTime DtChanged { get => Item.DtChanged; set => Item.DtChanged = value; }
	[DefaultValue("")]
	public string DtChangedString => $"{DtChanged:yyyy-mm-dd HH:mm:ss}";
	[DefaultValue(-1)]
	public long FromId { get => Item.FromId ?? -1; set => Item.FromId = value; }
	[DefaultValue("")]
	public string FromName { get => Item.FromName ?? string.Empty; set => Item.FromName = value; }
	[DefaultValue("")]
	public DateTime Date { get => Item.Date ?? DateTime.MinValue; set => Item.Date = value; }
	[DefaultValue("")]
	public DateTime ExpireDate { get => Item.ExpireDate ?? DateTime.MinValue; set => Item.ExpireDate = value; }
	[DefaultValue("")]
	public string Caption { get => Item.Caption ?? string.Empty; set => Item.Caption = value; }
	[DefaultValue("")]
	public string Type { get => Item.Type ?? string.Empty; set => Item.Type = value; }
	[DefaultValue(0)]
	public int Offset { get => Item.Offset; set => Item.Offset = value; }
	[DefaultValue(0)]
	public int Length { get => Item.Length; set => Item.Length = value; }
	[DefaultValue("")]
	public string Message { get => Item.Message ?? string.Empty; set => Item.Message = value; }
	[DefaultValue(1)]
	public int SourceScanCurrent { get; set; }
	[DefaultValue(1)]
	public int SourceScanCount { get; set; }
	public bool IsReady => Id > 0;
	public bool IsDownload { get; private set; }

	public TgEfStoryViewModel(TgEfStoryEntity item) : base()
	{
		Default(item);
	}

	public TgEfStoryViewModel() : base()
	{
		TgEfStoryEntity item = StoryRepository.GetNew(false).Item;
		Default(item);
	}

	#endregion

	#region Public and private methods

	private void Default(TgEfStoryEntity item)
	{
		Item = item;
		Uid = item.Uid;
		DtChanged = item.DtChanged;
		Id = item.Id;
		FromId = item.FromId ?? -1;
		FromName = item.FromName ?? string.Empty;
		Date = item.Date ?? DateTime.MinValue;
		ExpireDate = item.ExpireDate ?? DateTime.MinValue;
		Caption = item.Caption ?? string.Empty;
		Type = item.Type ?? string.Empty;
		Offset = item.Offset;
		Length = item.Length;
		Message = item.Message ?? string.Empty;
		SourceScanCurrent = this.GetDefaultPropertyInt(nameof(SourceScanCurrent));
		SourceScanCount = this.GetDefaultPropertyInt(nameof(SourceScanCount));
	}

	public override string ToString() => $"{Item}";

	public override string ToDebugString() => Item?.ToConsoleString() ?? string.Empty;

	public void SetIsDownload(bool isDownload) => IsDownload = isDownload;

	#endregion
}