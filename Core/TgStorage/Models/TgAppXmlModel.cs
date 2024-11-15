﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models;

/// <summary> App xml </summary>
[DebuggerDisplay("{ToDebugString()}")]
[Serializable]
[XmlRoot("App", Namespace = "", IsNullable = true)]
public sealed class TgAppXmlModel : ObservableObject, ITgCommon
{
	#region Public and private fields, properties, constructor

	[DefaultValue("")]
	[XmlElement("FileSession")]
	public string XmlFileSession { get; set; }
	[XmlIgnore]
	public bool IsExistsFileSession => File.Exists(XmlFileSession);
	[DefaultValue("")]
	[XmlElement("EfStorage")]
	public string XmlEfStorage { get; set; }
	[XmlIgnore]
	public bool IsExistsEfStorage => File.Exists(XmlEfStorage) && new FileInfo(XmlEfStorage).Length > 0;
	//[DefaultValue("")]
	//[XmlElement("FileStorage")]
	//public string XmlFileStorage { get; set; }
	//[XmlIgnore]
	//public bool IsExistsDeprecatedStorage => File.Exists(XmlFileStorage) && new FileInfo(XmlFileStorage).Length > 0;
	[XmlIgnore]
	public bool IsReady => IsExistsFileSession && IsExistsEfStorage;

	public TgAppXmlModel()
	{
		XmlFileSession = this.GetDefaultPropertyString(nameof(XmlFileSession));
		XmlEfStorage = this.GetDefaultPropertyString(nameof(XmlEfStorage));
		//XmlFileStorage = this.GetDefaultPropertyString(nameof(XmlFileStorage));
	}

	#endregion

	#region Public and private methods

    public string ToDebugString() =>
	    $"{TgCommonUtils.GetIsReady(IsReady)} | {nameof(XmlFileSession)}: {XmlFileSession} | {nameof(XmlEfStorage)}: {XmlEfStorage}";

	/// <summary>
	///  Set path for file session.
	/// </summary>
	/// <param name="path"></param>
	public void SetFileSessionPath(string path)
	{
		XmlFileSession = !File.Exists(path) && Directory.Exists(path)
			? Path.Combine(path, TgFileUtils.FileSession)
			: path;
		if (!IsExistsFileSession)
		{
			XmlFileSession = Path.Combine(Directory.GetCurrentDirectory(), TgFileUtils.FileSession);
		}
	}

	/// <summary> Set path for file storage </summary>
	/// <param name="path"></param>
	public void SetEfStoragePath(string path)
	{
		XmlEfStorage = !File.Exists(path) && Directory.Exists(path)
			? Path.Combine(path, TgFileUtils.FileEfStorage)
			: path;
		if (!IsExistsEfStorage)
		{
			XmlEfStorage = Path.Combine(Directory.GetCurrentDirectory(), TgFileUtils.FileEfStorage);
		}
	}

	///// <summary> Set path for deprecated storage </summary>
	//public void SetDeprecatedStoragePath(string path)
	//{
	//	XmlFileStorage = !File.Exists(path) && Directory.Exists(path)
	//		? Path.Combine(path, TgFileUtils.FileDeprecatedStorage)
	//		: path;
	//	if (!IsExistsDeprecatedStorage)
	//	{
	//		XmlFileStorage = Path.Combine(Directory.GetCurrentDirectory(), TgFileUtils.FileDeprecatedStorage);
	//	}
	//}

	#endregion
}