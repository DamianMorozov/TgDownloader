// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Windows.Input;

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgAdvancedView.xaml
/// </summary>
public partial class TgMenuSourcesPage : TgPageBase
{
	#region Public and private fields, properties, constructor

	public TgMenuSourcesViewModel ViewModel { get; set; }

	public TgMenuSourcesPage(TgMenuSourcesViewModel viewModel)
	{
		ViewModel = viewModel;
		ViewModel.OnNavigatedTo();
		InitializeComponent();
	}

	#endregion
	#region Public and private methods

	private void FieldDirectory_OnKeyUp(object sender, KeyEventArgs e)
	{
		if (sender is not TextBox textBox) return;
		if (textBox.Tag is not long sourceId) return;
		if (!Directory.Exists(textBox.Text)) return;

		TgSqlTableSourceModel sourceDb = ViewModel.ContextManager.ContextTableSources.GetItem(sourceId);
		if (!sourceDb.IsExists) return;
		sourceDb.Directory = textBox.Text;
		ViewModel.ContextManager.ContextTableSources.AddOrUpdateItem(sourceDb);
	}

	#endregion
}