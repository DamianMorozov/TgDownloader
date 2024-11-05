// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models;

public sealed class TgMediaInfoModel(string remote, long size, DateTime dtCreate)
{
	public string Remote { get; set; } = remote;
	public long Size { get; set; } = size;
	public DateTime DtCreate { get; set; } = dtCreate;
	public string Number { get; set; } = string.Empty;
	public string LocalFile { get; set; } = remote;
	public string LocalPath { get; set; } = string.Empty;
	public bool IsJoinFileNameWithMessageId { get; set; } = false;
	public string LocalFileWithNumber => IsJoinFileNameWithMessageId ? $"{Number} {LocalFile}" : LocalFile;
	public string LocalFullWithNumber => Path.Combine(LocalPath, LocalFileWithNumber);
	private static readonly char[] InvalidChars = Path.GetInvalidFileNameChars();

	public TgMediaInfoModel() : this(string.Empty, default, default) { }

	public void Normalize(bool isJoinFileNameWithMessageId)
	{
		IsJoinFileNameWithMessageId = isJoinFileNameWithMessageId;
		LocalFile = LocalFile.Trim();
		// Replace characters in the file name depending on the OS
		LocalFile = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? LocalFile.Replace("/", "\\") : LocalFile.Replace("\\", "/");
		// Replace invalid characters for file names 
		LocalFile = InvalidChars.Aggregate(LocalFile, (current, c) => current.Replace(c.ToString(), "_"));
	}
}