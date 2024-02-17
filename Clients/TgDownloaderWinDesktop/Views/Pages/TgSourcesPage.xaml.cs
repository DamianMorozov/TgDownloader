// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgAdvancedView.xaml
/// </summary>
public partial class TgSourcesPage
{
	#region Public and private fields, properties, constructor

	public TgSourcesPage()
	{
		TgDesktopUtils.TgSourcesVm.OnNavigatedTo();
		InitializeComponent();
	}

	#endregion

	#region Public and private methods

	private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
	{
		TgSqlTableSourceViewModel t = e.Item as TgSqlTableSourceViewModel;
		if (t != null)
			// If filter is turned on, filter completed items.
		{
			//if (this.cbCompleteFilter.IsChecked == true && t.Complete == true)
			//	e.Accepted = false;
			//else
			//	e.Accepted = true;
			e.Accepted = true;
		}
	}

	#endregion
}