// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Services;

/// <summary>
/// Service that provides pages for navigation.
/// </summary>
public sealed class PageService : IPageService
{
	/// <summary>
	/// Service which provides the instances of pages.
	/// </summary>
	private readonly IServiceProvider _serviceProvider;

	/// <summary>
	/// Creates new instance and attaches the <see cref="IServiceProvider"/>.
	/// </summary>
	public PageService(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	/// <inheritdoc />
	public T? GetPage<T>() where T : class
	{
		if (!typeof(FrameworkElement).IsAssignableFrom(typeof(T)))
			throw new InvalidOperationException("The page should be a WPF control.");

		return (T?)_serviceProvider.GetService(typeof(T));
	}

	/// <inheritdoc />
	public FrameworkElement? GetPage(Type pageType)
	{
		if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
			throw new InvalidOperationException("The page should be a WPF control.");

		return _serviceProvider.GetService(pageType) as FrameworkElement;
	}
}
