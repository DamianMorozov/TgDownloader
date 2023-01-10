// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Text;
using TgDownloaderCore.Models;
using TgLocaleCore.Utils;

namespace TgDownloaderCore.Utils;

public static class AppUtils
{
    #region Public and private fields, properties, constructor

    public static string LocalFilePath => FileNameUtils.AppSettings;
    public static bool IsExistsLocalFile => File.Exists(LocalFilePath);

    #endregion

    #region Public and private methods

    public static AppModel GetSettings()
    {
        if (!IsExistsLocalFile) return new();
        using StreamReader streamReader = new(LocalFilePath, Encoding.Unicode);
        string xml = streamReader.ReadToEnd();
        return !string.IsNullOrEmpty(xml) ? XmlUtils.DeserializeFromXml<AppModel>(xml) : new();
    }

    public static AppModel LoadSettings() => GetSettings();

    public static void SetSettings(AppModel app)
    {
        //string xml = SerializeAsXmlString<TgSettingsModel>(true);
        string xml = XmlUtils.SerializeAsXmlDocument(app, true).InnerXml;
        xml = XmlUtils.GetPrettyXml(xml);
        using FileStream fileStream = new(LocalFilePath, FileMode.Create);
        using StreamWriter streamWriter = new(fileStream, Encoding.Unicode);
        streamWriter.Write(xml);
    }

    public static void StoreSettings(AppModel app) => SetSettings(app);

    #endregion
}
