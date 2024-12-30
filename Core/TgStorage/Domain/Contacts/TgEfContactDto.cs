// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Contacts;

/// <summary> Contact DTO </summary>
public sealed partial class TgEfContactDto : TgDtoBase, ITgDto<TgEfContactDto, TgEfContactEntity>
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private DateTime _dtChanged;
	[ObservableProperty]
	private long _id;
	[ObservableProperty]
	private long _accessHash;
	[ObservableProperty]
	private bool _isContactActive;
	[ObservableProperty]
	private bool _isBot;
	[ObservableProperty]
	private string _firstName = string.Empty;
	[ObservableProperty]
	private string _lastName = string.Empty;
	[ObservableProperty]
	private string _userName = string.Empty;
	[ObservableProperty]
	private string _userNames = string.Empty;
	[ObservableProperty]
	private string _phoneNumber = string.Empty;
	[ObservableProperty]
	private string _status = string.Empty;
	[ObservableProperty]
	private string _restrictionReason = string.Empty;
	[ObservableProperty]
	private string _langCode = string.Empty;
	[ObservableProperty]
	private int _storiesMaxId;
	[ObservableProperty]
	private string _botInfoVersion = string.Empty;
	[ObservableProperty]
	private string _botInlinePlaceholder = string.Empty;
	[ObservableProperty]
	private int _botActiveUsers;

	[ObservableProperty]
	private int _sourceScanCurrent;
	[ObservableProperty]
	private int _sourceScanCount;
	[ObservableProperty]
	private bool _isDownload;
	public bool IsReady => Id > 0;

	#endregion

	#region Private methods

	public string DtChangedString => $"{DtChanged:yyyy-MM-dd HH:mm:ss}";

	public override string ToString() => $"{Id} | {AccessHash}";

	public TgEfContactDto Fill(TgEfContactDto dto, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = dto.Uid;
		DtChanged = dto.DtChanged;
		Id = dto.Id;
		AccessHash = dto.AccessHash;
		IsContactActive = dto.IsContactActive;
		IsBot = dto.IsBot;
		FirstName = dto.FirstName;
		LastName = dto.LastName;
		UserName = dto.UserName;
		UserNames = dto.UserNames;
		PhoneNumber = dto.PhoneNumber;
		Status = dto.Status;
		RestrictionReason = dto.RestrictionReason;
		LangCode = dto.LangCode;
		StoriesMaxId = dto.StoriesMaxId;
		BotInfoVersion = dto.BotInfoVersion;
		BotInlinePlaceholder = dto.BotInlinePlaceholder;
		BotActiveUsers = dto.BotActiveUsers;

		SourceScanCurrent = dto.SourceScanCurrent;
		SourceScanCount = dto.SourceScanCount;
		IsDownload = dto.IsDownload;

		return this;
	}

	public TgEfContactDto Fill(TgEfContactEntity item, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = item.Uid;
		DtChanged = item.DtChanged;
		Id = item.Id;
		AccessHash = item.AccessHash;
		IsContactActive = item.IsActive;
		IsBot = item.IsBot;
		FirstName = item.FirstName ?? string.Empty;
		LastName = item.LastName ?? string.Empty;
		UserName = item.UserName ?? string.Empty;
		UserNames = item.UserNames ?? string.Empty;
		PhoneNumber = item.PhoneNumber ?? string.Empty;
		Status = item.Status ?? string.Empty;
		RestrictionReason = item.RestrictionReason ?? string.Empty;
		LangCode = item.LangCode ?? string.Empty;
		StoriesMaxId = item.StoriesMaxId;
		BotInfoVersion = item.BotInfoVersion ?? string.Empty;
		BotInlinePlaceholder = item.BotInlinePlaceholder;
		BotActiveUsers = item.BotActiveUsers;

		SourceScanCurrent = 1;
		SourceScanCount = 1;
		IsDownload = false;

		return this;
	}

	public TgEfContactDto GetDto(TgEfContactEntity item)
	{
		var dto = new TgEfContactDto();
		dto.Fill(item, isUidCopy: true);
		return dto;
	}

	public TgEfContactEntity GetEntity() => new()
	{
		Uid = Uid,
		DtChanged = DtChanged,
		Id = Id,
		AccessHash = AccessHash,
		IsActive = IsContactActive,
		IsBot = IsBot,
		FirstName = FirstName,
		LastName = LastName,
		UserName = UserName,
		UserNames = UserNames,
		PhoneNumber = PhoneNumber,
		Status = Status,
		RestrictionReason = RestrictionReason,
		LangCode = LangCode,
		StoriesMaxId = StoriesMaxId,
		BotInfoVersion = BotInfoVersion,
		BotInlinePlaceholder = BotInlinePlaceholder,
		BotActiveUsers = BotActiveUsers,
	};

	#endregion
}
