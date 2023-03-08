// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgCore.Helpers;

[DebuggerDisplay("{nameof(AppSettingsHelper)} | {AppXml.Version} | {AppXml.FileSession} | {AppXml.FileStorage}")]
public class AppSettingsHelper : IHelper
{
	#region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	private static AppSettingsHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public static AppSettingsHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

	#endregion

	#region Public and private fields, properties, constructor

	[XmlIgnore]
	public AppXmlModel AppXml { get; set; }
	public bool IsReady => AppXml.IsReady;

	public AppSettingsHelper()
	{
		AppXml = new();
		LoadXmlSettings();
		// If file app xml is not exists.
		StoreXmlSettings();
	}

	#endregion

	#region Public and private methods - ISerializable

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	protected AppSettingsHelper(SerializationInfo info, StreamingContext context)
	{
		object? app = info.GetValue(nameof(AppXml), typeof(AppXmlModel));
		AppXml = app is AppXmlModel appXml ? appXml : new();
	}

	/// <summary>
	/// Get object data for serialization info.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(AppXml), AppXml);
	}

	public void LoadXmlSettings(Encoding? encoding = null)
	{
		if (!File.Exists(AppXml.LocalFilePath)) return;
		using StreamReader streamReader = new(FileUtils.AppXmlSettings, encoding ?? Encoding.Unicode);
		string xml = streamReader.ReadToEnd();
		if (!string.IsNullOrEmpty(xml))
			AppXml = DataFormatUtils.DeserializeFromXml<AppXmlModel>(xml);
	}

	public void StoreXmlSettings(Encoding? encoding = null)
	{
		if (string.IsNullOrEmpty(AppXml.FileSession) || !AppXml.IsExistsFileSession)
			AppXml.FileSession = FileUtils.Session;
		if (string.IsNullOrEmpty(AppXml.FileStorage) || !AppXml.IsExistsFileStorage)
			AppXml.FileStorage = FileUtils.Storage;
		string xml = DataFormatUtils.SerializeAsXmlDocument(AppXml, true).InnerXml;
		xml = DataFormatUtils.GetPrettyXml(xml);
		using FileStream fileStream = new(FileUtils.AppXmlSettings, FileMode.Create);
		using StreamWriter streamWriter = new(fileStream, encoding ?? Encoding.Unicode);
		streamWriter.Write(xml);
	}

	#endregion
}