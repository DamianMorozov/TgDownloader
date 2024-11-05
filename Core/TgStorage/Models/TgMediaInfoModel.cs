// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models;

public sealed class TgMediaInfoModel(string remote, long size, DateTime dtCreate)
{
	public string Remote { get; set; } = remote;
	public long Size { get; set; } = size;
	public DateTime DtCreate { get; set; } = dtCreate;
	public string LocalFileWithNumber { get; set; } = remote;
	public string LocalFileWithoutNumber { get; set; } = remote;
	public string LocalPath { get; set; } = string.Empty;
	public string LocalFullWithNumber => Path.Combine(LocalPath, LocalFileWithNumber);
	private static readonly char[] InvalidChars = Path.GetInvalidFileNameChars();

	public TgMediaInfoModel() : this(string.Empty, default, default) { }

	public void Normalize()
	{
		LocalFileWithNumber = Normalize(LocalFileWithNumber);
		LocalFileWithoutNumber = Normalize(LocalFileWithoutNumber);
	}

	public string Normalize(string fileName)
	{
		fileName = fileName.Trim();
		// Replace characters in the file name depending on the OS
		fileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? fileName.Replace("/", "\\") : fileName.Replace("\\", "/");
		return SanitizeFileName(fileName);
	}

	/// <summary> Replace invalid characters for file names </summary>
	private string SanitizeFileName(string fileName) => 
		InvalidChars.Aggregate(fileName, (current, c) => current.Replace(c.ToString(), "_"));
}