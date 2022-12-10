// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderCore.Helpers;

public class TgSettingsHelper
{
    #region Design pattern "Lazy Singleton"

    private static TgSettingsHelper _instance;
    public static TgSettingsHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public string DestDirectory { get; set; }
    public string ChannelUsername { get; set; }
    public int MessageStartId { get; set; }
    public int MessageCount { get; set; }

    public TgSettingsHelper()
    {
        DestDirectory = string.Empty;
        ChannelUsername = string.Empty;
        MessageStartId = 1;
        MessageCount = 0;
    }

    #endregion

}
