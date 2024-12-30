// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgInfrastructure.Contracts;

namespace TgDownloaderWinDesktopWPF.Helpers;

/// <summary>
/// XAML helper.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
internal sealed class TgXamlHelper : ITgHelper
{
	#region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	private static TgXamlHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public static TgXamlHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

	#endregion

	#region Public and private methods

    public string ToDebugString() => TgLocaleHelper.Instance.UseOverrideMethod;

    #endregion
}