// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Contacts;

/// <summary> View-model for TgEfContactEntity </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgEfContactViewModel : TgViewModelBase
{
	#region Public and private fields, properties, constructor

	public TgEfContactRepository ContactRepository { get; } = new(TgEfUtils.EfContext);
	public TgEfContactEntity Item { get; set; } = default!;

	public Guid Uid
	{
		get => Item.Uid;
		set
		{
			TgEfStorageResult<TgEfContactEntity> storageResult = ContactRepository.GetAsync(
				new() { Uid = value }, isNoTracking: false).GetAwaiter().GetResult();
			Item = storageResult.IsExists
				? storageResult.Item
				: ContactRepository.GetNewAsync(isNoTracking: false).GetAwaiter().GetResult().Item;
		}
	}

	[DefaultValue("")]
	public DateTime DtChanged { get => Item.DtChanged; set => Item.DtChanged = value; }
	[DefaultValue(-1)]
	public long Id { get => Item.Id; set => Item.Id = value; }
	[DefaultValue("")]
	public string DtChangedString => $"{DtChanged:yyyy-MM-dd HH:mm:ss}";
	[DefaultValue(-1)]
	public long AccessHash { get => Item.AccessHash; set => Item.AccessHash = value; }
	[DefaultValue(false)]
	public bool IsContactActive { get => Item.IsActive; set => Item.IsActive = value; }
	[DefaultValue(false)]
	public bool IsBot { get => Item.IsBot; set => Item.IsBot = value; }
	[DefaultValue("")]
	public string FirstName { get => Item.FirstName ?? string.Empty; set => Item.FirstName = value; }
	[DefaultValue("")]
	public string LastName { get => Item.LastName ?? string.Empty; set => Item.LastName = value; }
	[DefaultValue("")]
	public string UserName { get => Item.UserName ?? string.Empty; set => Item.UserName = value; }
	[DefaultValue("")]
	public string UserNames { get => Item.UserNames ?? string.Empty; set => Item.UserNames = value; }
	[DefaultValue("")]
	public string PhoneNumber { get => Item.PhoneNumber ?? string.Empty; set => Item.PhoneNumber = value; }
	[DefaultValue("")]
	public string Status { get => Item.Status ?? string.Empty; set => Item.Status = value; }
	[DefaultValue("")]
	public string RestrictionReason { get => Item.RestrictionReason ?? string.Empty; set => Item.RestrictionReason = value; }
	[DefaultValue("")]
	public string LangCode { get => Item.LangCode ?? string.Empty; set => Item.LangCode = value; }
	[DefaultValue(-1)]
	public int StoriesMaxId { get => Item.StoriesMaxId; set => Item.StoriesMaxId = value; }
	[DefaultValue("")]
	public string BotInfoVersion { get => Item.BotInfoVersion ?? string.Empty; set => Item.BotInfoVersion = value; }
	[DefaultValue("")]
	public string BotInlinePlaceholder { get => Item.BotInlinePlaceholder ?? string.Empty; set => Item.BotInlinePlaceholder = value; }
	[DefaultValue(-1)]
	public int BotActiveUsers { get => Item.BotActiveUsers; set => Item.BotActiveUsers = value; }
	[DefaultValue(1)]
	public int SourceScanCurrent { get; set; }
	[DefaultValue(1)]
	public int SourceScanCount { get; set; }
	public bool IsReady => Id > 0;
	public bool IsDownload { get; private set; }

	public TgEfContactViewModel(TgEfContactEntity item) : base()
	{
		Default(item);
	}

	public TgEfContactViewModel() : base()
	{
		TgEfContactEntity item = ContactRepository.GetNewAsync(false).GetAwaiter().GetResult().Item;
		Default(item);
	}

	#endregion

	#region Public and private methods

	private void Default(TgEfContactEntity item)
	{
		Item = item;
		Uid = item.Uid;
		DtChanged = item.DtChanged;
		Id = item.Id;
		AccessHash = item.AccessHash;
		IsActive = item.IsActive;
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
		BotInlinePlaceholder = item.BotInlinePlaceholder ?? string.Empty;
		BotActiveUsers = item.BotActiveUsers;
		SourceScanCurrent = this.GetDefaultPropertyInt(nameof(SourceScanCurrent));
		SourceScanCount = this.GetDefaultPropertyInt(nameof(SourceScanCount));
	}

	public override string ToString() => $"{Item}";

	public override string ToDebugString() => Item?.ToConsoleString() ?? string.Empty;

	/// <summary> Set new contact </summary>
	public void SetContact(long id, string firstName, string lastName, string userName, string phoneNumber)
	{
		Id = id;
		FirstName = firstName;
		LastName = lastName;
		UserName = userName;
		PhoneNumber = phoneNumber;
	}

	public void SetIsDownload(bool isDownload) => IsDownload = isDownload;

	#endregion
}