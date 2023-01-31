// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Text;
using TgCore.Utils;
using TgDownloader.Models;

namespace TgDownloader.Utils;

public static class AppUtils
{
    #region Public and private fields, properties, constructor

    public static string LocalFilePath => FileNameUtils.AppSettings;
    public static bool IsExistsLocalFile => File.Exists(LocalFilePath);

    #endregion

    #region Public and private methods

    public static AppXmlModel LoadXmlSettings()
    {
        if (!IsExistsLocalFile) return new();
        using StreamReader streamReader = new(LocalFilePath, Encoding.Unicode);
        string xml = streamReader.ReadToEnd();
        return !string.IsNullOrEmpty(xml) ? DataFormatUtils.DeserializeFromXml<AppXmlModel>(xml) : new();
    }

    public static void StoreSettings(AppXmlModel app)
    {
        string xml = DataFormatUtils.SerializeAsXmlDocument(app, true).InnerXml;
        xml = DataFormatUtils.GetPrettyXml(xml);
        using FileStream fileStream = new(LocalFilePath, FileMode.Create);
        using StreamWriter streamWriter = new(fileStream, Encoding.Unicode);
        streamWriter.Write(xml);
    }

    #endregion
}