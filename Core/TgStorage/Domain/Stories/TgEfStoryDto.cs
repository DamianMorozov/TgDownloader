// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Contacts;

/// <summary> Proxy DTO </summary>
public sealed partial class TgEfStoryDto : TgDtoBase, ITgDto<TgEfStoryDto, TgEfStoryEntity>
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private DateTime _dtChanged;
	[ObservableProperty]
	private long _id;
	[ObservableProperty]
	private long _fromId;
	[ObservableProperty]
	private string _fromName = string.Empty;
	[ObservableProperty]
	private DateTime _date;
	[ObservableProperty]
	private DateTime _expireDate;
	[ObservableProperty]
	private string _caption = string.Empty;
	[ObservableProperty]
	private string _type = string.Empty;
	[ObservableProperty]
	private int _offset;
	[ObservableProperty]
	private int _length;
	[ObservableProperty]
	private string _message = string.Empty;

	[ObservableProperty]
	private int _sourceScanCurrent;
	[ObservableProperty]
	private int _sourceScanCount;
	[ObservableProperty]
	private bool _isDownload;
	public bool IsReady => Id > 0;



	#endregion

	#region Public and private methods

	public string DtChangedString => $"{DtChanged:yyyy-MM-dd HH:mm:ss}";

	public override string ToString() => $"{DtChanged} | {Id} | {FromId} | {FromName} | {Date} | {Caption}";

	public TgEfStoryDto Fill(TgEfStoryDto dto, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = dto.Uid;
		DtChanged = dto.DtChanged;
		Id = dto.Id;
		FromId = dto.FromId;
		FromName = dto.FromName;
		Date = dto.Date;
		ExpireDate = dto.ExpireDate;
		Caption = dto.Caption;
		Type = dto.Type;
		Offset = dto.Offset;
		Length = dto.Length;
		Message = dto.Message;

		SourceScanCurrent = dto.SourceScanCurrent;
		SourceScanCount = dto.SourceScanCount;
		IsDownload = dto.IsDownload;

		return this;
	}

	public TgEfStoryDto Fill(TgEfStoryEntity item, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = item.Uid;
		DtChanged = item.DtChanged;
		Id = item.Id;
		FromId = item.FromId ?? 0;
		FromName = item.FromName ?? string.Empty;
		Date = item.Date ?? DateTime.MinValue;
		ExpireDate = item.ExpireDate ?? DateTime.MinValue;
		Caption = item.Caption ?? string.Empty;
		Type = item.Type ?? string.Empty;
		Offset = item.Offset;
		Length = item.Length;
		Message = item.Message ?? string.Empty;

		SourceScanCurrent = 1;
		SourceScanCount = 1;
		IsDownload = false;

		return this;
	}

	public TgEfStoryDto GetDto(TgEfStoryEntity item)
	{
		var dto = new TgEfStoryDto();
		dto.Fill(item, isUidCopy: true);
		return dto;
	}

	public TgEfStoryEntity GetEntity() => new()
	{
		Uid = Uid,
		DtChanged = DtChanged,
		Id = Id,
		FromId = FromId,
		FromName = FromName,
		Date = Date,
		ExpireDate = ExpireDate,
		Caption = Caption,
		Type = Type,
		Offset = Offset,
		Message = Message,
	};

	#endregion
}
