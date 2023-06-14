// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgCore.Helpers;

[DebuggerDisplay("{ToString()}")]
public class TgAppSettingsHelper : ITgHelper, ITgSerializable
{
	#region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	private static TgAppSettingsHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public static TgAppSettingsHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

	#endregion

	#region Public and private fields, properties, constructor

	[XmlIgnore]
	public AppXmlModel AppXml { get; set; }
	public bool IsReady => AppXml.IsReady;

	public TgAppSettingsHelper()
	{
		AppXml = new();
		LoadXmlSettings();
		// If file app xml is not exists.
		StoreXmlSettings();
	}

	#endregion

	#region Public and private methods

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	protected TgAppSettingsHelper(SerializationInfo info, StreamingContext context)
	{
		object? app = info.GetValue(nameof(AppXml), typeof(AppXmlModel));
		AppXml = app as AppXmlModel ?? new();
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
		using StreamReader streamReader = new(TgFileUtils.AppXmlSettings, encoding ?? Encoding.Unicode);
		string xml = streamReader.ReadToEnd();
		if (!string.IsNullOrEmpty(xml))
			AppXml = TgDataFormatUtils.DeserializeFromXml<AppXmlModel>(xml);
	}

	public void DefaultXmlSettings(Encoding? encoding = null)
	{
		AppXml.FileSession = TgFileUtils.Session;
			AppXml.FileStorage = TgFileUtils.Storage;
		StoreXmlSettingsUnsafe();
	}

	public void StoreXmlSettings(Encoding? encoding = null)
	{
		if (string.IsNullOrEmpty(AppXml.FileSession) || !AppXml.IsExistsFileSession)
			AppXml.FileSession = TgFileUtils.Session;
		if (string.IsNullOrEmpty(AppXml.FileStorage) || !AppXml.IsExistsFileStorage)
			AppXml.FileStorage = TgFileUtils.Storage;
		StoreXmlSettingsUnsafe();
	}

	public void StoreXmlSettingsUnsafe(Encoding? encoding = null)
	{
		string xml = TgDataFormatUtils.SerializeAsXmlDocument(AppXml, true).InnerXml;
		xml = TgDataFormatUtils.GetPrettyXml(xml);
		using FileStream fileStream = new(TgFileUtils.AppXmlSettings, FileMode.Create);
		using StreamWriter streamWriter = new(fileStream, encoding ?? Encoding.Unicode);
		streamWriter.Write(xml);
	}

	public override string ToString() =>
		$"{AppXml.Version} | {AppXml.FileSession} | {AppXml.FileStorage}";

	#endregion
}