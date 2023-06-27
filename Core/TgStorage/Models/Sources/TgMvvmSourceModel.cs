// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using CommunityToolkit.Mvvm.Input;

namespace TgStorage.Models.Sources;

/// <summary>
/// View for TgSqlTableSourceModel.
/// </summary>
public sealed partial class TgMvvmSourceModel : TgMvvmBase
{
	#region Public and private fields, properties, constructor

	public TgSqlTableSourceModel Source { get; set; }
	public bool IsDestDirectoryExists => Directory.Exists(Source.Directory);
	public int Progress => Source.FirstId * 100 / Source.Count;
	public string ProgressString => $"{Progress:###.##} % | {Source.FirstId} from {Source.Count}";
	public bool IsExistsInStorage => Source.IsExists;
	public bool IsComplete => Source.FirstId >= Source.Count;
	public bool IsLoad { get; set; }
	public int FirstId { get => Source.FirstId; set => Source.FirstId = value; }
	public string DestDirectory { get => Source.Directory; set => Source.Directory = value; }
	public Action<TgMvvmSourceModel> DownloadSourceFromTelegram { get; set; }

	public TgMvvmSourceModel(TgSqlTableSourceModel source, Action<TgMvvmSourceModel> downloadSourceFromTelegram)
	{
		Source = source;
		DownloadSourceFromTelegram = downloadSourceFromTelegram;
	}

	public TgMvvmSourceModel(TgSqlTableSourceModel source)
	{
		Source = source;
		DownloadSourceFromTelegram = _ => { };
	}

	#endregion

	#region Public and private methods

	[RelayCommand]
	public async Task OnDownloadSourceAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		DownloadSourceFromTelegram(this);
	}

	#endregion
}