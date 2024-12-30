// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Contacts;

/// <summary> App DTO </summary>
public sealed partial class TgEfAppDto : TgDtoBase, ITgDto<TgEfAppDto, TgEfAppEntity>
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private Guid _apiHash;
	[ObservableProperty]
	private int _apiId;
	[ObservableProperty]
	private string _phoneNumber = string.Empty;
	[ObservableProperty]
	private Guid _proxyUid;
	[ObservableProperty]
	private string _firstName = string.Empty;
	[ObservableProperty]
	private string _lastName = string.Empty;

	public string ApiIdString
	{
		get => ApiId.ToString();
		set => ApiId = int.TryParse(value, out int apiId) ? apiId : 0;
	}

	public string ApiHashString
	{
		get => ApiHash.ToString();
		set => ApiHash = Guid.TryParse(value, out Guid apiHash) ? apiHash : Guid.Empty;
	}

	#endregion

	#region Public and private methods

	public override string ToString() => $"{ApiHash} | {ApiId}";
	
	public TgEfAppDto Fill(TgEfAppDto dto, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = dto.Uid;
		ApiHash = dto.ApiHash;
		ApiId = dto.ApiId;
		FirstName = dto.FirstName;
		LastName = dto.LastName;
		PhoneNumber = dto.PhoneNumber;
		ProxyUid = dto.ProxyUid;
		return this;
	}

	public TgEfAppDto Fill(TgEfAppEntity item, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = item.Uid;
		ApiHash = item.ApiHash;
		ApiId = item.ApiId;
		FirstName = item.FirstName;
		LastName = item.LastName;
		PhoneNumber = item.PhoneNumber;
		ProxyUid = item.ProxyUid ?? Guid.Empty;
		return this;
	}

	public TgEfAppDto GetDto(TgEfAppEntity item)
	{
		var dto = new TgEfAppDto();
		dto.Fill(item, isUidCopy: true);
		return dto;
	}

	public TgEfAppEntity GetEntity() => new()
	{
		Uid = Uid,
		ApiHash = ApiHash,
		ApiId = ApiId,
		FirstName = FirstName,
		LastName = LastName,
		PhoneNumber = PhoneNumber,
		ProxyUid = ProxyUid,
	};

	#endregion
}
