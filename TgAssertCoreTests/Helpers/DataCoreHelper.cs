// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgLocalization.Helpers;
using TgStorageCore.Helpers;

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

    public TgLocaleHelper TgLocale{ get; } = TgLocaleHelper.Instance;
    public TgStorageHelper TgStorage { get; } = TgStorageHelper.Instance;

    #endregion

    #region Public and private methods

    //

    #endregion
}