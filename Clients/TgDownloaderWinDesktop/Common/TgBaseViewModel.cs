// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgCore.Helpers;
using TgCore.Utils;
using TgDownloader.Helpers;
using TgStorage.Models.Apps;

namespace TgDownloaderWinDesktop.Common;

/// <summary>
/// Base class for ViewModel.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public partial class TgBaseViewModel : ObservableObject, INavigationAware
{
	#region Public and private fields, properties, constructor

	public TgLocaleHelper Locale => TgLocaleHelper.Instance;
	public TgSqlContextCacheHelper ContextCache => TgSqlContextCacheHelper.Instance;
	public TgSqlContextManagerHelper ContextManager => TgSqlContextManagerHelper.Instance;
	internal TgClientHelper TgClient => TgClientHelper.Instance;
	public EnumToBooleanConverter EnumToBooleanConverter { get; set; }
	public bool IsReload { get; set; }

	public TgBaseViewModel()
	{
		EnumToBooleanConverter = new();
	}

	#endregion

	#region Public and private methods

	public override string ToString() => $"{Locale} | {ContextManager}";

	private string? GetConfigFromDb(string what)
	{
		TgSqlTableAppModel app = ContextManager.ContextTableApps.GetCurrentItem();
		string? result = what switch
		{
			"api_hash" => TgDataFormatUtils.ParseGuidToString(app.ApiHash),
			"api_id" => app.ApiId.ToString(),
			"phone_number" => app.PhoneNumber,
			"session_pathname" => TgAppSettingsHelper.Instance.AppXml.FileSession,
			_ => null
		};
		switch (what)
		{
			case "api_hash":
			case "api_id":
			case "phone_number":
				ContextManager.ContextTableApps.AddOrUpdateItem(app);
				break;
		}
		return result;
	}

	public void ClientConnectExists()
	{
		if (!ContextManager.ContextTableApps.GetValidXpLite(ContextManager.ContextTableApps.GetCurrentItem()).IsValid) return;
		TgClient.Connect(GetConfigFromDb, ContextManager.ContextTableApps.GetCurrentProxy());
	}

	public virtual void OnNavigatedTo()
	{
	}

	public virtual void OnNavigatedFrom()
	{
	}

	[RelayCommand]
	internal async Task OnSourceReload()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		
		//await Shell.Current.GoToAsync(nameof(TgAppPage)).ConfigureAwait(false);
	}

	[RelayCommand]
	internal async Task OnMenuStorageAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
	}

	[RelayCommand]
	internal async Task OnMenuClientAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		//await Shell.Current.GoToAsync(nameof(TgClientPage)).ConfigureAwait(false);
	}

	[RelayCommand]
	internal async Task OnMenuFiltersAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		//await Shell.Current.GoToAsync(nameof(TgFiltersPage)).ConfigureAwait(false);
	}

	[RelayCommand]
	internal async Task OnMenuDownloadAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		//await Shell.Current.GoToAsync(nameof(TgDownloadPage)).ConfigureAwait(false);
	}

	[RelayCommand]
	internal async Task MenuAdvancedAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		//await Shell.Current.GoToAsync(nameof(TgAdvancedPage)).ConfigureAwait(false);
	}

	[RelayCommand]
	internal async Task MenuAboutAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		//await Shell.Current.GoToAsync(nameof(TgAboutPage)).ConfigureAwait(false);
	}

	#endregion
}