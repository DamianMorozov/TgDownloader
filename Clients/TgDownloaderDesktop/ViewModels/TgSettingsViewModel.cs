// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public partial class TgSettingsViewModel : TgPageViewModelBase
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private ElementTheme _elementTheme;
	[ObservableProperty]
	private ObservableCollection<string> _appThemes = default!;
	[ObservableProperty]
	private string _appTheme = default!;
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
				App.MainWindow.DispatcherQueue.TryEnqueue(async () => await SettingsService.SetAppLanguageAsync(AppLanguage));
				Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = value switch
				{
					nameof(TgEnumLanguage.Russian) => "ru-RU",
					nameof(TgEnumLanguage.English) => "en-US",
					_ => "en-US",
				};
				OnPropertyChanged();
			}
		}
	}

	public ICommand SettingsDefaultCommand { get; }

	public TgSettingsViewModel(ITgSettingsService settingsService) : base(settingsService)
	{
		Default();
		SettingsDefaultCommand = new AsyncRelayCommand(SettingsDefaultAsync);
	}

	#endregion

	#region Public and private methods

	public async Task OnNavigatedToAsync(NavigationEventArgs navigationEventArgs)
	{
		await SettingsService.LoadAsync();
	}

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

	private async Task SettingsDefaultAsync() => await ContentDialogAsync(SettingsDefaultCoreAsync, TgResourceExtensions.AskSettingsDefault());

	private async Task SettingsDefaultCoreAsync()
	{
		SettingsService.Default();
		Default();
		await Task.CompletedTask;
	}

	#endregion
}
