// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgClientView.xaml
/// </summary>
public partial class TgMenuClientPage : TgPageBase
{
	#region Public and private fields, properties, constructor

	public TgMenuClientViewModel ViewModel { get; set; }

	public TgMenuClientPage(TgMenuClientViewModel viewModel)
	{
		ViewModel = viewModel;
		ViewModel.OnNavigatedTo();
		InitializeComponent();
	}

	#endregion

	#region Public and private methods

	private void ButtonClientClearSettings_OnClick(object sender, RoutedEventArgs e)
	{
		ViewModel.TgSqlApp.Clear();
	}

	private void ButtonClientConnect_OnClick(object sender, RoutedEventArgs e)
	{
		try
		{
			_ = Task.Run(async () =>
						{
							await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
				Application.Current.Dispatcher.Invoke(() =>
				{
					ViewModel.IsLoad = true;
					
					ViewModel.Exception.Clear();
					if (!ViewModel.ContextManager.ContextTableApps.GetValidXpLite(ViewModel.TgSqlApp).IsValid)
						return;
					ViewModel.TgClient.ConnectSessionDesktop(GetDesktopConfig, 
						ViewModel.ContextManager.ContextTableApps.GetCurrentProxy(), AfterConnect);
				});
						}).ConfigureAwait(true);
		}
		catch (Exception ex)
		{
			ViewModel.Exception.Set(ex);
		}
	}

	private void AfterConnect()
	{
		try
		{
			_ = Task.Run(async () =>
			{
				await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

				Application.Current.Dispatcher.Invoke(() =>
				{
				ViewModel.StateMessage = ViewModel.TgClient.TgQuery;
				ViewModel.IsClientReady = ViewModel.TgClient.IsReady;
				ViewModel.IsFileSession = ViewModel.TgAppSettings.AppXml.IsExistsFileSession;
				if (ViewModel.TgClient.IsReady)
				{
					TgClientUtils.TgClient = ViewModel.TgClient;
					ViewModelClearConfig();
				}
				ViewModel.IsLoad = false;
				});
			}).ConfigureAwait(true);
		}
		catch (Exception ex)
		{
			ViewModel.Exception.Set(ex);
		}
	}

	private void ButtonClientDisconnect_OnClick(object sender, RoutedEventArgs e)
	{
		try
		{
			_ = Task.Run(async () =>
						{
							await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
				Application.Current.Dispatcher.Invoke(() =>
				{
					ViewModel.IsLoad = true;
					
					ViewModel.Exception.Clear();
					if (!ViewModel.ContextManager.ContextTableApps.GetValidXpLite(ViewModel.TgSqlApp).IsValid)
						return;
					ViewModel.TgClient.Disconnect();
					ViewModel.StateMessage = ViewModel.TgClient.TgQuery;
					ViewModel.IsClientReady = ViewModel.TgClient.IsReady;
					ViewModel.IsLoad = false;
				});
						}).ConfigureAwait(true);
		}
		catch (Exception ex)
		{
			ViewModel.Exception.Set(ex);
		}
	}

	private string? GetDesktopConfig(string what)
	{
		ViewModel.TgClient.TgQuery = what;
		switch (what)
		{
			case "api_hash":
				string apiHash = TgDataFormatUtils.ParseGuidToString(ViewModel.TgSqlApp.ApiHash);
				return apiHash;
			case "api_id":
				return ViewModel.TgSqlApp.ApiId.ToString();
			case "phone_number":
				return ViewModel.TgSqlApp.PhoneNumber;
			case "notifications":
				return ViewModel.Notifications;
			case "first_name":
				if (string.IsNullOrEmpty(ViewModel.FirstName))
					ViewModel.IsNeedFirstName = true;
				return ViewModel.FirstName;
			case "last_name":
				if (string.IsNullOrEmpty(ViewModel.LastName))
					ViewModel.IsNeedLastName = true;
				return ViewModel.LastName;
			case "session_pathname":
				string sessionPath = Path.Combine(Directory.GetCurrentDirectory(), ViewModel.TgAppSettings.AppXml.FileSession);
				return sessionPath;
			case "verification_code":
				if (string.IsNullOrEmpty(ViewModel.VerificationCode))
					ViewModel.IsNeedVerificationCode = true;
				return ViewModel.VerificationCode;
			case "password":
				if (string.IsNullOrEmpty(ViewModel.Password))
					ViewModel.IsNeedPassword = true;
				return ViewModel.Password;
			case "session_key":
			case "server_address":
			case "device_model":
			case "system_version":
			case "app_version":
			case "system_lang_code":
			case "lang_pack":
			case "lang_code":
			default:
				return null;
		}
	}

	private void ViewModelClearConfig()
	{
		ViewModel.IsNeedVerificationCode = false;
		ViewModel.VerificationCode = string.Empty;
		ViewModel.IsNeedFirstName = false;
		ViewModel.FirstName = string.Empty;
		ViewModel.IsNeedLastName = false;
		ViewModel.LastName = string.Empty;
		ViewModel.IsNeedPassword = false;
		ViewModel.Password = string.Empty;
	}

	#endregion
}