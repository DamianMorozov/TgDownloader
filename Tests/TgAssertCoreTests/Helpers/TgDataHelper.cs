// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgAssertCoreTests.Helpers;

public class TgDataHelper
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgDataHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgDataHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public TgSqlContextManagerHelper ContextManager => TgSqlContextManagerHelper.Instance;
    public TgAppSettingsHelper AppSettings => TgAppSettingsHelper.Instance;

    public TgDataHelper()
    {
		AppSettings.LoadXmlSettings();
		ContextManager.CreateOrConnectDb(false);
	}

    #endregion
}