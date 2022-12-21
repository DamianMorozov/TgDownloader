// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderCore.Helpers;

public class AppHelper
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static AppHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static AppHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public string Version { get; private set; }

    public AppHelper()
    {
        Version = string.Empty;
    }

    #endregion

    #region Public and private methods

    public void SetVersion(Assembly assembly)
    {
        Version = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion ?? string.Empty;
        ushort count = 0, pos = 0;
        foreach (char c in Version)
        {
            if (Equals(c, '.')) count++;
            if (count is 3) break;
            pos++;
        }
        if (count is 3)
            Version = Version.Substring(0, pos);
        Version = $"v.{Version}";
    }

    #endregion
}
