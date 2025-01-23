﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public partial class TgUpdateViewModel : TgPageViewModelBase
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	public partial string UpdateLog { get; set; } = string.Empty;
	public IRelayCommand UpdateCommand { get; }

	public TgUpdateViewModel(ITgSettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
	{
		// Commands
		UpdateCommand = new AsyncRelayCommand(UpdateAsync);
	}

	#endregion

	#region Public and private methods

	public override async Task OnNavigatedToAsync(NavigationEventArgs e) => await LoadDataAsync(async () =>
	{
		await ReloadUiAsync();
	});

	private async Task UpdateAsync() => await ContentDialogAsync(VelopackUpdateAsync, TgResourceExtensions.AskUpdateApp());

	/// <summary> Velopack installer update </summary>
	private async Task VelopackUpdateAsync()
	{
		UpdateLog = string.Empty;
		var log = new StringBuilder();
		try
		{
			log.AppendLine($"{TgConstants.AppTitleDesktop} {TgAppSettingsHelper.Instance.AppVersion} started");
			log.AppendLine($"Checking updates on the link github.com...");
			var mgr = new UpdateManager(new GithubSource(TgConstants.LinkGitHub, string.Empty, prerelease: false));
			// Check for new version
			var newVersion = await mgr.CheckForUpdatesAsync();
			if (newVersion is null)
			{
				log.AppendLine("You are using the latest version of the software");
				return;
			}
			// Download new version
			log.AppendLine("Download new version...");
			await mgr.DownloadUpdatesAsync(newVersion);
			//// Install new version and restart app
			//var prompt = AnsiConsole.Prompt(
			//	new SelectionPrompt<string>()
			//		.Title("Install new version and restart app?")
			//		.PageSize(Console.WindowHeight - 5)
			//		.MoreChoicesText(TgLocale.MoveUpDown)
			//		.AddChoices(TgLocale.MenuNo, TgLocale.MenuYes));
			//var isInstall = prompt.Equals(TgLocale.MenuYes);
			//if (isInstall)
			//	mgr.ApplyUpdatesAndRestart(newVersion);
		}
		// Cannot perform this operation in an application which is not installed
		catch (Exception ex)
		{
			log.AppendLine(ex.Message);
		}
		UpdateLog = log.ToString();
	}

	#endregion
}