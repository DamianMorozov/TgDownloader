// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using Wpf.Ui.Mvvm.Interfaces;

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgAdvancedView.xaml
/// </summary>
public partial class TgViewSourcesPage : INotifyPropertyChanged
{
	#region Public and private fields, properties, constructor

	public TgViewSourcesViewModel ViewModel { get; set; }

	public TgViewSourcesPage(TgViewSourcesViewModel viewModel) : base(viewModel)
	{
		ViewModel = viewModel;
		InitializeComponent();
	}

	#endregion

	#region Public and private methods

	private void TgViewSourcesPage_OnLoaded(object sender, RoutedEventArgs e)
	{
		ViewModel.ReloadSources();
	}

	private void ButtonSourceReload_OnClick(object sender, RoutedEventArgs e)
	{
		if (sender is not Button button) return;
		if (button.Tag is not long sourceId) return;
		_ = Task.Run(async () =>
		{
			await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
			ViewModel.IsReload = true;
		}).ConfigureAwait(true);

		TgSqlTableSourceModel source = ViewModel.Sources.First(item => item.Id.Equals(sourceId));
		int index = ViewModel.Sources.IndexOf(source);
		ViewModel.Sources[index] = ViewModel.ContextManager.ContextTableSources.GetItem(sourceId);
		//TgDownloadSettingsModel tgDownloadSettings = new() { SourceId = sourceId };
		//ViewModel.ClientConnectExists();
		//	ViewModel.TgClient.DownloadAllData(tgDownloadSettings, StoreMessage, StoreDocument,
		//		ViewModel.ContextManager.ContextTableMessages.FindExistsMessage);
		//ViewModel.TgClient.UpdateStatus(ViewModel.Locale.SettingsSource);
		
		_ = Task.Run(async () =>
		{
			await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
			ViewModel.IsReload = false;
		}).ConfigureAwait(true);
		}

	private void ButtonSourceReloadAll_OnClick(object sender, RoutedEventArgs e)
	{
		_ = Task.Run(async () =>
					{
						await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		ViewModel.IsReload = true;
					}).ConfigureAwait(true);
		
		for (int index = 0; index < ViewModel.Sources.Count; index++)
		{
			ViewModel.Sources[index] = ViewModel.ContextManager.ContextTableSources.GetItem(ViewModel.Sources[index].Id);
		}

		_ = Task.Run(async () =>
		{
			await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
			ViewModel.IsReload = false;
		}).ConfigureAwait(true);
	}

	private void StoreMessage(int arg1, long arg2, DateTime arg3, TgEnumMessageType arg4, long arg5, string arg6)
	{
		//
	}

	private void StoreDocument(long arg1, long arg2, long arg3, string arg4, long arg5, long arg6)
	{
		//
	}

	#endregion
}