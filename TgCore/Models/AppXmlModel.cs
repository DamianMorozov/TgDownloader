// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgCore.Models;

[Serializable]
[XmlRoot("App", Namespace = "", IsNullable = true)]
[DebuggerDisplay("{nameof(AppXmlModel)} | {FileSession} | {FileStorage} | {IsUseProxy}")]
public class AppXmlModel : IModel
{
	#region Public and private fields, properties, constructor

	[XmlIgnore]
	public string LocalFilePath => FileUtils.AppXmlSettings;
	[DefaultValue("")]
	[XmlIgnore]
	public string Version { get; set; }
	[DefaultValue("")]
	[XmlElement]
	public string FileSession { get; set; }
	[XmlIgnore]
	public bool IsExistsFileSession => File.Exists(FileSession);
	[DefaultValue("")]
	[XmlElement]
	public string FileStorage { get; set; }
	[XmlIgnore]
	public bool IsExistsFileStorage => File.Exists(FileStorage);
	[XmlIgnore] public bool IsReady => IsExistsFileSession && IsExistsFileStorage;
	[DefaultValue(false)]
	[XmlElement]
	public bool IsUseProxy { get; set; }

	public AppXmlModel()
	{
		if (string.IsNullOrEmpty(Version))
			Version = this.GetPropertyDefaultValue(nameof(Version));
		FileSession = this.GetPropertyDefaultValue(nameof(FileSession));
		FileStorage = this.GetPropertyDefaultValue(nameof(FileStorage));
		IsUseProxy = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsUseProxy));
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
		Version = $"v{Version}";
	}

	public void SetFileSessionPath(string path)
	{
		FileSession = !File.Exists(path) && Directory.Exists(path)
			? Path.Combine(path, FileUtils.Session)
			: path;
		if (!IsExistsFileSession)
		{
			FileSession = Path.Combine(Directory.GetCurrentDirectory(), FileUtils.Session);
		}
	}

	public void SetFileStoragePath(string path)
	{
		FileStorage = !File.Exists(path) && Directory.Exists(path)
			? Path.Combine(path, FileUtils.Storage)
			: path;
		if (!IsExistsFileStorage)
		{
			FileStorage = Path.Combine(Directory.GetCurrentDirectory(), FileUtils.Storage);
		}
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
		Version = info.GetString(nameof(Version)) ?? this.GetPropertyDefaultValue(nameof(Version));
		FileSession = info.GetString(nameof(FileSession)) ?? this.GetPropertyDefaultValue(nameof(FileSession));
		FileStorage = info.GetString(nameof(FileStorage)) ?? this.GetPropertyDefaultValue(nameof(FileStorage));
		IsUseProxy = info.GetBoolean(nameof(IsUseProxy));
	}

	/// <summary>
	/// Get object data for serialization info.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(Version), Version);
		info.AddValue(nameof(FileSession), FileSession);
		info.AddValue(nameof(FileStorage), FileStorage);
		info.AddValue(nameof(IsUseProxy), IsUseProxy);
	}

	#endregion
}