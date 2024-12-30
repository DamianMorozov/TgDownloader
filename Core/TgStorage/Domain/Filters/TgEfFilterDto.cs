// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Contacts;

/// <summary> Filter DTO </summary>
public sealed partial class TgEfFilterDto : TgDtoBase, ITgDto<TgEfFilterDto, TgEfFilterEntity>
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private bool _isEnabled;
	[ObservableProperty]
	private TgEnumFilterType _filterType;
	[ObservableProperty]
	private string _name = string.Empty;
	[ObservableProperty]
	private string _mask = string.Empty;
	[ObservableProperty]
	private long _size;
	[ObservableProperty]
	private TgEnumFileSizeType _sizeType;

	public long SizeAtBytes => SizeType switch
	{
		TgEnumFileSizeType.KBytes => Size * 1024,
		TgEnumFileSizeType.MBytes => Size * 1024 * 1024,
		TgEnumFileSizeType.GBytes => Size * 1024 * 1024 * 1024,
		TgEnumFileSizeType.TBytes => Size * 1024 * 1024 * 1024 * 1024,
		_ => Size,
	};

	#endregion

	#region Public and private methods

	public override string ToString() => $"{IsEnabled} | {FilterType} | {Name} | {Mask} | {Size} | {SizeType}";

	public TgEfFilterDto Fill(TgEfFilterDto dto, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = dto.Uid;
		IsEnabled = dto.IsEnabled;
		FilterType = dto.FilterType;
		Name = dto.Name;
		Mask = dto.Mask;
		Size = dto.Size;
		SizeType = dto.SizeType;
		return this;
	}

	public TgEfFilterDto Fill(TgEfFilterEntity item, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = item.Uid;
		IsEnabled = item.IsEnabled;
		FilterType = item.FilterType;
		Name = item.Name;
		Mask = item.Mask;
		Size = item.Size;
		SizeType = item.SizeType;
		return this;
	}

	public TgEfFilterDto GetDto(TgEfFilterEntity item)
	{
		var dto = new TgEfFilterDto();
		dto.Fill(item, isUidCopy: true);
		return dto;
	}

	public TgEfFilterEntity GetEntity() => new()
	{
		Uid = Uid,
		IsEnabled = IsEnabled,
		FilterType = FilterType,
		Name = Name,
		Mask = Mask,
		Size = Size,
		SizeType = SizeType,
	};

	#endregion
}
