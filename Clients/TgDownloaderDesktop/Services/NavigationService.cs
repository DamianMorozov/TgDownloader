﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Services;

// For more information on navigation between pages see
// https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/navigation.md
public sealed class NavigationService : INavigationService
{
	private readonly IPageService _pageService;
	private object? _lastParameterUsed;
	private Frame? _frame;

	public event NavigatedEventHandler? Navigated;

	public Frame? Frame
	{
		get
		{
			if (_frame == null)
			{
				_frame = App.MainWindow.Content as Frame;
				RegisterFrameEvents();
			}

			return _frame;
		}

		set
		{
			UnregisterFrameEvents();
			_frame = value;
			RegisterFrameEvents();
		}
	}

	[MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]
	public bool CanGoBack => Frame != null && Frame.CanGoBack;

	public NavigationService(IPageService pageService)
	{
		_pageService = pageService;
	}

	private void RegisterFrameEvents()
	{
		if (_frame != null)
		{
			_frame.Navigated += OnNavigated;
		}
	}

	private void UnregisterFrameEvents()
	{
		if (_frame != null)
		{
			_frame.Navigated -= OnNavigated;
		}
	}

	public bool GoBack()
	{
		if (CanGoBack)
		{
			var vmBeforeNavigation = _frame.GetPageViewModel();
			_frame.GoBack();
			if (vmBeforeNavigation is INavigationAware navigationAware)
			{
				navigationAware.OnNavigatedFrom();
			}

			return true;
		}

		return false;
	}

	public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
	{
		var pageType = _pageService.GetPageType(pageKey);

		if (_frame != null && (_frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParameterUsed))))
		{
			_frame.Tag = clearNavigation;
			var vmBeforeNavigation = _frame.GetPageViewModel();
			var navigated = _frame.Navigate(pageType, parameter);
			if (navigated)
			{
				_lastParameterUsed = parameter;
				if (vmBeforeNavigation is INavigationAware navigationAware)
				{
					navigationAware.OnNavigatedFrom();
				}
			}

			return navigated;
		}

		return false;
	}

	private void OnNavigated(object sender, NavigationEventArgs e)
	{
		if (sender is Frame frame)
		{
			var clearNavigation = (bool)frame.Tag;
			if (clearNavigation)
			{
				frame.BackStack.Clear();
			}

			if (frame.GetPageViewModel() is INavigationAware navigationAware)
			{
				navigationAware.OnNavigatedTo(e.Parameter);
			}

			Navigated?.Invoke(sender, e);
		}
	}

	public void SetListDataItemForNextConnectedAnimation(object item) => Frame.SetListDataItemForNextConnectedAnimation(item);
}