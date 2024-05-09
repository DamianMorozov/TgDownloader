namespace TgStorage.Contracts;

public interface ITgEfVersionRepository
{
	short LastVersion { get; }
	TgEfVersionEntity GetLastVersion();
	void FillTableVersions();
	Task FillTableVersionsAsync();
}