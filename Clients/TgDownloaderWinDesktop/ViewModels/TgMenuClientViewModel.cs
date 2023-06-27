// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Drawing;

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class TgMenuClientViewModel : TgViewBase, INavigationAware
{
	#region Public and private fields, properties, constructor

	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Notifications { get; set; }
	public string Password { get; set; }
	public string VerificationCode { get; set; }
	public Brush BackgroundVerificationCode { get; set; }
	public Brush BackgroundPassword { get; set; }
	public Brush BackgroundFirstName { get; set; }
	public Brush BackgroundLastName { get; set; }
	private bool _isNeedVerificationCode;
	public bool IsNeedVerificationCode { get => _isNeedVerificationCode;
		set
		{
			_isNeedVerificationCode = value;
			BackgroundVerificationCode = value ? new(Color.Yellow) : new SolidBrush(Color.Transparent);
		}
	}
	private bool _isNeedPassword;
	public bool IsNeedPassword
	{
		get => _isNeedPassword;
		set
		{
			_isNeedPassword = value;
			BackgroundPassword = value ? new(Color.Yellow) : new SolidBrush(Color.Transparent);
		}
	}
	private bool _isNeedFirstName;
	public bool IsNeedFirstName
	{
		get => _isNeedFirstName;
		set
		{
			_isNeedFirstName = value;
			BackgroundFirstName = value ? new(Color.Yellow) : new SolidBrush(Color.Transparent);
		}
	}
	private bool _isNeedLastName;
	public bool IsNeedLastName
	{
		get => _isNeedLastName;
		set
		{
			_isNeedLastName = value;
			BackgroundLastName = value ? new(Color.Yellow) : new SolidBrush(Color.Transparent);
		}
	}
	public TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;
	public TgSqlTableAppModel TgSqlApp { get; set; }
	public ObservableCollection<TgMvvmProxyModel> Proxies { get; set; }
	public TgMvvmProxyModel MvvmProxy { get; set; }

	public TgMenuClientViewModel()
	{
		FirstName = string.Empty;
		LastName = string.Empty;
		Notifications = string.Empty;
		Password = string.Empty;
		TgSqlApp = ContextManager.ContextTableApps.GetCurrentItem();
		VerificationCode = string.Empty;
		Proxies = new();
		MvvmProxy = new(new());
		BackgroundVerificationCode = new SolidBrush(Color.Transparent);
		BackgroundPassword = new SolidBrush(Color.Transparent);
		BackgroundFirstName = new SolidBrush(Color.Transparent);
		BackgroundLastName = new SolidBrush(Color.Transparent);
		StateMessage = string.Empty;
	}

	#endregion

	#region Public and private methods

	public void OnNavigatedTo()
	{
		if (!IsInitialized)
			InitializeViewModel();
	}

	public void OnNavigatedFrom()
	{
		//
	}

	private void InitializeViewModel()
	{
		Load();
		if (TgClientUtils.TgClient.IsReady)
		{
			TgClient = TgClientUtils.TgClient;
			IsClientReady = TgClient.IsReady;
		}
		IsFileSession = TgAppSettings.AppXml.IsExistsFileSession;
		IsInitialized = true;
	}

	public void Load()
	{
		Proxies.Clear();
		foreach (TgSqlTableProxyModel proxy in ContextManager.ContextTableProxies.GetList())
		{
			Proxies.Add(new(proxy));
		}
		MvvmProxy.Proxy = ContextManager.ContextTableProxies.GetItem(TgSqlApp.ProxyUid);
	}

	#endregion
}