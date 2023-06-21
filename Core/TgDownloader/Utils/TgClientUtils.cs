// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgDownloader.Helpers;

namespace TgDownloader.Utils;

/// <summary>
/// TgClient utils.
/// </summary>
public static class TgClientUtils
{
	#region Public and private fields, properties, constructor

	public static TgClientHelper TgClient { get; set; } = TgClientHelper.Instance;

	#endregion
}