// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using TgCore.Models;
using TgDownloader.Utils;
using Windows.ApplicationModel.Contacts;

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgAdvancedView.xaml
/// </summary>
public partial class TgMenuSourcesPage : INotifyPropertyChanged
{
	#region Public and private fields, properties, constructor

	public TgMenuSourcesViewModel ViewModel { get; set; }

	public TgMenuSourcesPage(TgMenuSourcesViewModel viewModel) : base(viewModel)
	{
		ViewModel = viewModel;
		InitializeComponent();
	}

	#endregion

	#region Public and private methods

	private void TgMenuSourcesPage_OnLoaded(object sender, RoutedEventArgs e)
	{
		ViewModel.Load();
		if (TgClientUtils.TgClient.IsReady)
		{
			ViewModel.TgClient = TgClientUtils.TgClient;
			ViewModel.IsClientReady = ViewModel.TgClient.IsReady;
		}
	}

	private void ButtonSourceLoad_OnClick(object sender, RoutedEventArgs e)
	{
		if (sender is not Button button)
		{
			UpdateStatusWithId(0, 0, "Button is not ready!");
			return;
		}

		if (button.Tag is not long sourceId)
		{
			UpdateStatusWithId(0, 0, "Tag is not ready!");
			return;
		}

		if (!ViewModel.TgClient.IsReady)
		{
			UpdateStatusWithId(0, 0, "Client is not ready!");
			return;
		}

		TgSqlTableSourceModel source = ViewModel.Sources.First(item => item.Id.Equals(sourceId));
		int index = ViewModel.Sources.IndexOf(source);
		if (!Directory.Exists(ViewModel.Sources[index].Directory))
		{
			UpdateStatusWithId(source.Id, 0, $"Directory is not exists! {ViewModel.Sources[index].Directory}");
			return;
		}
		
		ViewModel.Sources[index] = ViewModel.ContextManager.ContextTableSources.GetItem(sourceId);
		//source.Directory = ViewModel.Sources[index].Directory;

		TgDownloadSettingsModel tgDownloadSettings = new()
		{
			SourceId = source.Id, SourceFirstId = source.FirstId, DestDirectory = source.Directory
		};

		ViewModel.TgClient.UpdateStatusWithId = UpdateStatusWithId;
			_ = Task.Run(async () =>
			{
				ViewModel.IsLoad = true;
				await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
				ViewModel.TgClient.DownloadAllData(tgDownloadSettings,
					ViewModel.ContextManager.ContextTableMessages.StoreMessage,
					ViewModel.ContextManager.ContextTableDocuments.StoreDocument,
					ViewModel.ContextManager.ContextTableMessages.FindExistsMessage);
				ViewModel.TgClient.UpdateStatus(ViewModel.TgLocale.SettingsSource);
				ViewModel.IsLoad = false;
			}).ConfigureAwait(true);
	}

	private void UpdateStatusWithId(long sourceId, int messageId, string message)
	{
		ViewModel.TgClientQuery = sourceId > 0 ? $"Source {sourceId} | Message {messageId} | {message}": message;
		SourceReload(sourceId);
	}

	private void ButtonSourceReloadAll_OnClick(object sender, RoutedEventArgs e)
	{
		if (!ViewModel.TgClient.IsReady)
		{
			UpdateStatusWithId(0, 0, "Client is not ready!");
			return;
		}
		SourceReload(0);
	}

	private void SourceReload(long sourceId)
	{
		Application.Current.Dispatcher.Invoke(() =>
		{
			// Code to modify the SourceCollection goes here
		for (int index = 0; index < ViewModel.Sources.Count; index++)
			{
				if (sourceId > 0)
				{
					if (ViewModel.Sources[index].Id.Equals(sourceId))
						ViewModel.Sources[index] = ViewModel.ContextManager.ContextTableSources.GetItem(ViewModel.Sources[index].Id);
				}
				else
					ViewModel.Sources[index] = ViewModel.ContextManager.ContextTableSources.GetItem(ViewModel.Sources[index].Id);
			}
		});
	}

	#endregion
}