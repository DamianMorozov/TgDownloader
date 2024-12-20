// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Contacts;

public sealed partial class TgEfContactDto : TgViewModelBase, ITgDbEntity, ITgDbFillEntity<TgEfContactDto>
{
	[ObservableProperty]
	private Guid _uid;
	[ObservableProperty]
	private long _id;
	[ObservableProperty]
	private string _userName = string.Empty;
	[ObservableProperty]
	private string _dtChanged = string.Empty;
	[ObservableProperty]
	private bool _isContactActive;
	[ObservableProperty]
	private bool _isBot;
	[ObservableProperty]
	private string _firstName = string.Empty;
	[ObservableProperty]
	private string _lastName = string.Empty;
	[ObservableProperty]
	private string _phone = string.Empty;
	[ObservableProperty]
	private string _status = string.Empty;

	public void Default()
	{
		throw new NotImplementedException();
	}

	public TgEfContactDto Fill(TgEfContactDto item, bool isUidCopy)
	{
		throw new NotImplementedException();
	}
}
