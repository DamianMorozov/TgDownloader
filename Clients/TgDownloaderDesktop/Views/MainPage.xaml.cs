using Microsoft.UI.Xaml.Controls;

using TgDownloaderDesktop.ViewModels;

namespace TgDownloaderDesktop.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
