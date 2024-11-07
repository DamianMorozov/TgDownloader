// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

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
