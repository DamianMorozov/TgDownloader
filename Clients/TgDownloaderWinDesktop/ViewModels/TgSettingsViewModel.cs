// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSettingsViewModel : TgPageViewModelBase, INavigationAware
{
    public static Wpf.Ui.Appearance.ThemeType CurrentTheme { get; set; } = Wpf.Ui.Appearance.ThemeType.Unknown;
	
    public void OnNavigatedTo()
	{
        InitializeViewModelAsync().GetAwaiter();
    }

	public void OnNavigatedFrom() { }

    protected override async Task InitializeViewModelAsync()
    {
        await base.InitializeViewModelAsync();

        CurrentTheme = Wpf.Ui.Appearance.Theme.GetAppTheme();
    }

	[RelayCommand]
	public async Task OnChangeThemeAsync(string parameter)
	{
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            switch (parameter)
            {
                case "theme_light":
                    if (CurrentTheme == Wpf.Ui.Appearance.ThemeType.Light)
                        break;
                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
                    CurrentTheme = Wpf.Ui.Appearance.ThemeType.Light;
                    break;
                default:
                    if (CurrentTheme == Wpf.Ui.Appearance.ThemeType.Dark)
                        break;
                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
                    CurrentTheme = Wpf.Ui.Appearance.ThemeType.Dark;
                    break;
            }
        }, false).ConfigureAwait(false);
    }
}