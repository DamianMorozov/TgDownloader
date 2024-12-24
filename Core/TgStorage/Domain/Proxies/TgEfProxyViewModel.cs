// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Proxies;

/// <summary> View-model for TgSqlTableSourceModel </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgEfProxyViewModel : TgViewModelBase
{
	#region Public and private fields, properties, constructor

	private TgEfProxyRepository ProxyRepository { get; } = new(TgEfUtils.EfContext);
	public TgEfProxyEntity Item { get; set; } = default!;
    public Guid ProxyUid
    {
	    get => Item.Uid;
	    set
	    {
		    TgEfStorageResult<TgEfProxyEntity> storageResult = ProxyRepository.GetAsync(
			    new() { Uid = value }, isNoTracking: false).GetAwaiter().GetResult();
		    Item = storageResult.IsExists
			    ? storageResult.Item
			    : ProxyRepository.GetNewAsync(isNoTracking: false).GetAwaiter().GetResult().Item;
	    }
    }

    [DefaultValue(0)]
    public TgEnumProxyType ProxyType { get => Item.Type; set => Item.Type = value; }
    [DefaultValue("")]
    public string ProxyHostName { get => Item.HostName; set => Item.HostName = value; }
    [DefaultValue(0)]
    public ushort ProxyPort { get => Item.Port; set => Item.Port = value; }
    [DefaultValue("")]
    public string ProxyUserName { get => Item.UserName; set => Item.UserName = value; }
    [DefaultValue("")]
    public string ProxyPassword { get => Item.Password; set => Item.Password = value; }
    [DefaultValue("")]
    public string ProxySecret { get => Item.Secret; set => Item.Secret = value; }
    [ObservableProperty]
	private string _prettyName = default!;

    public TgEfProxyViewModel(TgEfProxyEntity item) : base()
	{
		Default(item);
    }
    
    public TgEfProxyViewModel() : base()
    {
	    TgEfProxyEntity item = ProxyRepository.GetNewAsync(false).GetAwaiter().GetResult().Item;
	    Default(item);
    }

	#endregion

	#region Public and private methods

	public void Default(TgEfProxyEntity? item = null)
	{
		item ??= new();
		Item = item;
		ProxyUid = item.Uid;
		ProxyType = item.Type;
		ProxyHostName = item.HostName;
		ProxyPort = item.Port;
		ProxyUserName = item.UserName;
		ProxyPassword = item.Password;
		ProxySecret = item.Secret;
		SetPrettyName();
	}

	private void SetPrettyName() => PrettyName = $"{Item.Type} | {TgDataFormatUtils.GetFormatString(Item.HostName, 30)} | {Item.Port} | {Item.UserName}";

	public override string ToString() => PrettyName;

	public override string ToDebugString() => PrettyName;

	#endregion
}