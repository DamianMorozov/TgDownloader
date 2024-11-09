// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Views;

public sealed partial class ContentGridDetailPage : Page
{
	#region Public and private fields, properties, constructor

	public ContentGridDetailViewModel ViewModel { get; }

	public ContentGridDetailPage()
	{
		ViewModel = App.GetService<ContentGridDetailViewModel>();
		InitializeComponent();
	}

	#endregion

	#region Public and private methods

	protected override void OnNavigatedTo(NavigationEventArgs e)
	{
		base.OnNavigatedTo(e);
		this.RegisterElementForConnectedAnimation("animationKeyContentGrid", itemHero);
	}

	protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
	{
		base.OnNavigatingFrom(e);
		if (e.NavigationMode == NavigationMode.Back)
		{
			var navigationService = App.GetService<INavigationService>();

			if (ViewModel.Item != null)
			{
				navigationService.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
			}
		}
	}

	#endregion
}
