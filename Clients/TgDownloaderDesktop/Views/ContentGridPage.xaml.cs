using Microsoft.UI.Xaml.Controls;

using TgDownloaderDesktop.ViewModels;

namespace TgDownloaderDesktop.Views;

public sealed partial class ContentGridPage : Page
{
    public ContentGridViewModel ViewModel
    {
        get;
    }

    public ContentGridPage()
    {
        ViewModel = App.GetService<ContentGridViewModel>();
        InitializeComponent();
    }
}
