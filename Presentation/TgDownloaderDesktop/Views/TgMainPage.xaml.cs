﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Views;

public sealed partial class TgMainPage : Page
{
	#region Public and private fields, properties, constructor

	public TgMainViewModel ViewModel { get; }

	public TgMainPage()
	{
		ViewModel = App.GetService<TgMainViewModel>();
		InitializeComponent();
		Loaded += PageLoaded;
	}

	#endregion

	#region Public and private methods

	protected override async void OnNavigatedTo(NavigationEventArgs e)
	{
		base.OnNavigatedTo(e);
		await ViewModel.OnNavigatedToAsync(e);
	}

	private void PageLoaded(object sender, RoutedEventArgs e) => ViewModel.OnLoaded(XamlRoot);

	#endregion
}