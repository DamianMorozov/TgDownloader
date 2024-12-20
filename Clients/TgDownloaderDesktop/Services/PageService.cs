// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Services;

public sealed class PageService : IPageService
{
	private readonly Dictionary<string, Type> _pages = [];

	public PageService()
	{
		Configure<TgMainViewModel, TgMainPage>();
		Configure<TgLoadDataViewModel, TgLoadDataPage>();
		Configure<TgClientViewModel, TgClientPage>();
		Configure<TgContactsViewModel, TgContactsPage>();
		Configure<TgSourcesViewModel, TgSourcesPage>();
		Configure<WebViewViewModel, WebViewPage>();
		Configure<ListDetailsViewModel, ListDetailsPage>();
		Configure<ContentGridViewModel, ContentGridPage>();
		Configure<ContentGridDetailViewModel, ContentGridDetailPage>();
		Configure<DataGridViewModel, DataGridPage>();
		Configure<TgSettingsViewModel, TgSettingsPage>();
	}

	public Type GetPageType(string key)
	{
		Type? pageType;
		lock (_pages)
		{
			if (!_pages.TryGetValue(key, out pageType))
			{
				throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
			}
		}
		return pageType;
	}

	private void Configure<VM, V>()
		where VM : ObservableObject
		where V : Page
	{
		lock (_pages)
		{
			var key = typeof(VM).FullName!;
			if (_pages.ContainsKey(key))
			{
				throw new ArgumentException($"The key {key} is already configured in PageService");
			}

			var type = typeof(V);
			if (_pages.ContainsValue(type))
			{
				throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
			}

			_pages.Add(key, type);
		}
	}
}
