// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using CommunityToolkit.Mvvm.Input;

namespace TgStorage.Models.Proxies;

/// <summary>
/// View for TgSqlTableSourceModel.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public sealed partial class TgMvvmProxyModel : TgMvvmBase
{
	#region Public and private fields, properties, constructor

	public TgSqlTableProxyModel Proxy { get; set; }
	public string PrettyName => $"{Proxy.Type} | {Proxy.HostName} | {Proxy.Port} | {Proxy.UserName}";
	public Action<TgMvvmProxyModel> ConnectProxy { get; set; }

	public TgMvvmProxyModel(TgSqlTableProxyModel proxy, Action<TgMvvmProxyModel> connectProxy)
	{
		Proxy = proxy;
		ConnectProxy = connectProxy;
	}

	public TgMvvmProxyModel(TgSqlTableProxyModel proxy)
	{
		Proxy = proxy;
		ConnectProxy = _ => { };
	}

	#endregion

	#region Public and private methods

	[RelayCommand]
	public async Task OnConnectProxyAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		ConnectProxy(this);
	}

	#endregion
}