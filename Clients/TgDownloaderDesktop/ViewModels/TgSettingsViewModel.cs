// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public partial class TgSettingsViewModel : TgPageViewModelBase
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private ITgSettingsService _settingsService;
	[ObservableProperty]
	private ElementTheme _elementTheme;
	[ObservableProperty]
	private ObservableCollection<string> _appThemes = default!;
	[ObservableProperty]
	private string _appTheme = default!;
	private string _appEfStorage = default!;
	public string AppEfStorage
	{
		get => _appEfStorage;
		set
		{
			if (SetProperty(ref _appEfStorage, value))
			{
				SettingsService.SetAppEfStorageAsync(AppEfStorage).ConfigureAwait(false);
			}
			IsExistsEfStorage = File.Exists(value);
		}
	}
	[ObservableProperty]
	private bool _isExistsEfStorage;
	private string _appTgSession = default!;
	public string AppTgSession
	{
		get => _appTgSession;
		set
		{
			if (SetProperty(ref _appTgSession, value))
			{
				SettingsService.SetAppTgSessionAsync(AppTgSession).ConfigureAwait(false);
			}
			IsExistsTgSession = File.Exists(value);
		}
	}
	[ObservableProperty]
	private bool _isExistsTgSession;
	[ObservableProperty]
	private ObservableCollection<string> _languages = default!;
	private string _appLanguage = default!;
	public string AppLanguage
	{
		get => _appLanguage;
		set
		{
			if (SetProperty(ref _appLanguage, value))
			{
				SettingsService.SetAppLanguageAsync(AppLanguage).ConfigureAwait(false);
			}
			Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = value switch
			{
				nameof(TgEnumLanguage.Russian) => "ru-RU",
				nameof(TgEnumLanguage.English) => "en-US",
				_ => "en-US",
			};
		}
	}

	public ICommand SettingsDefaultCommand { get; }

	public TgSettingsViewModel(ITgSettingsService settingsService)
	{
		SettingsService = settingsService;
		Default();
		SettingsDefaultCommand = new RelayCommand(async () => await SettingsDefaultAsync());
	}

	#endregion

	#region Public and private methods

	private void Default()
	{
		AppThemes = [TgResourceExtensions.GetSettingsThemeDefault(), TgResourceExtensions.GetSettingsThemeLight(), TgResourceExtensions.GetSettingsThemeDark()];
		Languages = [nameof(TgEnumLanguage.Default), nameof(TgEnumLanguage.English), nameof(TgEnumLanguage.Russian)];
		ElementTheme = SettingsService.Theme;
		AppTheme = ElementTheme.ToString();
		AppEfStorage = SettingsService.EfStorage;
		AppTgSession = SettingsService.TgSession;
		AppLanguage = SettingsService.AppLanguage;
	}

	public async Task SetAppThemeAsync()
	{
		var theme = ElementTheme.Default;
		if (AppTheme == TgResourceExtensions.GetSettingsThemeLight())
			theme = ElementTheme.Light;
		else if (AppTheme == TgResourceExtensions.GetSettingsThemeDark())
			theme = ElementTheme.Dark;
		await SetAppThemeAsync(theme);
	}

	public async Task SetAppThemeAsync(ElementTheme theme)
	{
		if (ElementTheme != theme)
		{
			ElementTheme = theme;
			await SettingsService.SetAppThemeAsync(ElementTheme);
			AppTheme = ElementTheme.ToString();
		}
	}

	private async Task SettingsDefaultAsync()
	{
		if (XamlRootVm is null) return;
		ContentDialog dialog = new()
		{
			XamlRoot = XamlRootVm,
			Title = TgResourceExtensions.AskSettingsDefault(),
			PrimaryButtonText = TgResourceExtensions.GetYesButton(),
			CloseButtonText = TgResourceExtensions.GetCancelButton(),
			DefaultButton = ContentDialogButton.Close,
			PrimaryButtonCommand = new RelayCommand(() =>
			{
				SettingsService.Default();
				Default();
			})
		};
		_ = await dialog.ShowAsync();
	}

	#endregion
}
