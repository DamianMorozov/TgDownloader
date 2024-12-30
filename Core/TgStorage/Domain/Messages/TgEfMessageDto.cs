// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Contacts;

/// <summary> Message DTO </summary>
public sealed partial class TgEfMessageDto : TgDtoBase, ITgDto<TgEfMessageDto, TgEfMessageEntity>
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private DateTime _dtCreated;
	[ObservableProperty]
	private long _sourceId;
	[ObservableProperty]
	private long _id;
	[ObservableProperty]
	private TgEnumMessageType _type;
	[ObservableProperty]
	private long _size;
	[ObservableProperty]
	private string _message = string.Empty;



	#endregion

	#region Public and private methods

	public string DtChangedString => $"{DtCreated:yyyy-MM-dd HH:mm:ss}";

	public override string ToString() => $"{DtCreated} | {SourceId} | {Id} | {Type} | {Size} | {Message}";

	public TgEfMessageDto Fill(TgEfMessageDto dto, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = dto.Uid;
		DtCreated = dto.DtCreated;
		SourceId = dto.SourceId;
		Id = dto.Id;
		Type = dto.Type;
		Size = dto.Size;
		Message = dto.Message;
		return this;
	}

	public TgEfMessageDto Fill(TgEfMessageEntity item, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = item.Uid;
		DtCreated = item.DtCreated;
		SourceId = item.SourceId ?? 0;
		Id = item.Id;
		Type = item.Type;
		Size = item.Size;
		Message = item.Message;
		return this;
	}

	public TgEfMessageDto GetDto(TgEfMessageEntity item)
	{
		var dto = new TgEfMessageDto();
		dto.Fill(item, isUidCopy: true);
		return dto;
	}

	public TgEfMessageEntity GetEntity() => new()
	{
		Uid = Uid,
		DtCreated = DtCreated,
		SourceId = SourceId,
		Id = Id,
		Type = Type,
		Size = Size,
		Message = Message,
	};

	#endregion
}
