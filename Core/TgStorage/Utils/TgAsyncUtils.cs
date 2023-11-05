// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Utils;

/// <summary>
/// Async utilities.
/// </summary>
public static class TgAsyncUtils
{
    #region Public and private fields, properties, constructor

    public static TgEnumAppType AppType { get; set; }

    #endregion

    #region Public and private methods

    public static void SetAppType(TgEnumAppType appType)
    {
        AppType = appType;
    }

   // public static T GetFuncResult<T>(Func<Task<T>> job, TgEnumAppType appType = TgEnumAppType.Default)
   // {
   //     if (appType == TgEnumAppType.Default)
			//appType = AppType;
   //     return appType switch
   //     {
   //         TgEnumAppType.Desktop => Task.Run(job).Result,
   //         TgEnumAppType.Console => job().Result,
   //         _ => throw new ArgumentException(null, nameof(appType))
   //     };
   // }

    #endregion
}