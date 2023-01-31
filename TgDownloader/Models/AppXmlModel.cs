// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Runtime.Serialization;
using System.Xml.Serialization;
using TgCore.Interfaces;
using TgCore.Utils;

namespace TgDownloader.Models;

[Serializable]
[XmlRoot("App", Namespace = "", IsNullable = true)]
[DebuggerDisplay("{nameof(AppModel)} | {Version} | {StoragePath}")]
public class AppXmlModel : IModel
{
    #region Public and private fields, properties, constructor

    [DefaultValue("<unknown>")]
    [XmlIgnore]
    public string Version { get; set; }
    [DefaultValue("<not set>")]
    [XmlElement]
    public string StoragePath { get; set; }
    [XmlIgnore]
    public bool IsExistsStoragePath => File.Exists(StoragePath);

    public AppXmlModel()
    {
        Version = this.GetPropertyDefaultValueAsString(nameof(Version));
        StoragePath = this.GetPropertyDefaultValueAsString(nameof(StoragePath));
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

    public void SetStoragePath(string storagePath)
    {
        StoragePath = storagePath;
    }

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected AppXmlModel(SerializationInfo info, StreamingContext context)
    {
        Version = info.GetString(nameof(Version)) ?? this.GetPropertyDefaultValueAsString(nameof(Version));
        StoragePath = info.GetString(nameof(StoragePath)) ?? this.GetPropertyDefaultValueAsString(nameof(StoragePath));
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(Version), Version);
        info.AddValue(nameof(StoragePath), StoragePath);
    }

    #endregion
}