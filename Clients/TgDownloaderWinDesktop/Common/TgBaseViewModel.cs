// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Common;

/// <summary>
/// Base class for ViewModel.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public partial class TgBaseViewModel : ObservableObject, INavigationAware
{
	#region Public and private fields, properties, constructor

	public TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
	public TgSqlContextCacheHelper ContextCache => TgSqlContextCacheHelper.Instance;
	public TgSqlContextManagerHelper ContextManager => TgSqlContextManagerHelper.Instance;
	public TgClientHelper TgClient { get; set; } = TgClientHelper.Instance;
	public EnumToBooleanConverter EnumToBooleanConverter { get; set; }
	public bool IsLoad { get; set; }
	public string TgClientQuery { get; set; } = "";
	public bool IsClientReady { get; set; }
	public bool IsFileSession { get; set; }

	public TgBaseViewModel()
	{
		EnumToBooleanConverter = new();
	}

	#endregion

	#region Public and private methods

	public override string ToString() => $"{TgLocale} | {ContextManager}";

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
		//await Shell.Current.GoToAsync(nameof(TgMenuClientPage)).ConfigureAwait(false);
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