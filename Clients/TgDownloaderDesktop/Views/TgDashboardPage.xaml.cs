// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Views;

/// <summary>
/// Interaction logic for TgDashboardPage.xaml
/// </summary>
public partial class TgDashboardPage
{
	#region Public and private fields, properties, constructor

	public TgDashboardViewModel ViewModel { get; }

	public TgDashboardPage()
	{
		ViewModel = App.GetService<TgDashboardViewModel>();
		InitializeComponent();
	}

	#endregion
}