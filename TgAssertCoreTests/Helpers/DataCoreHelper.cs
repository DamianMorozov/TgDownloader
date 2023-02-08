// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgAssertCoreTests.Helpers;

public class DataCoreHelper
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static DataCoreHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static DataCoreHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public TgStorageHelper TgStorage { get; } = TgStorageHelper.Instance;
    public AppSettingsHelper AppSettings => AppSettingsHelper.Instance;

    public DataCoreHelper()
    {
        Init();
    }

    #endregion

    #region Public and private methods

    public void Init()
    {
        AppSettings.LoadXmlSettings();
        TgStorage.CreateOrConnectDb(false);
    }

    #endregion
}