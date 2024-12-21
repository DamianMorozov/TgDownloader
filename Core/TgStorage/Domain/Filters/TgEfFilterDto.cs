// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Contacts;

public sealed partial class TgEfFilterDto : TgViewModelBase, ITgDbEntity, ITgDbFillEntity<TgEfFilterDto>
{
	[ObservableProperty]
	private Guid _uid;
	[ObservableProperty]
	private bool _isEnabled;
	[ObservableProperty]
	private string _filterType = string.Empty;
	[ObservableProperty]
	private string _name = string.Empty;
	[ObservableProperty]
	private string _mask = string.Empty;
	[ObservableProperty]
	private string _size = string.Empty;

	public void Default()
	{
		throw new NotImplementedException();
	}

	public TgEfFilterDto Fill(TgEfFilterDto item, bool isUidCopy)
	{
		throw new NotImplementedException();
	}
}
