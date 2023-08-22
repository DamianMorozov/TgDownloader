// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Sources;

/// <summary>
/// View for TgSqlTableSourceModel.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSqlTableSourceViewModel : TgViewModelBase
{
	#region Public and private fields, properties, constructor

	public TgSqlTableSourceModel Source { get; set; }
	public int Progress => Source.FirstId * 100 / Source.Count;
	public string ProgressString => $"{Progress:###.##} % | {Source.FirstId} from {Source.Count}";
	public bool IsComplete => Source.FirstId >= Source.Count;
    [DefaultValue(0)]
    public long SourceId { get => Source.Id; set => Source.Id = value; }
    [DefaultValue("")]
    public string SourceUserName { get => Source.UserName; set => Source.UserName = value; }
    [DefaultValue("")]
    public string SourceAbout { get => Source.About; set => Source.About = value; }
    [DefaultValue("")]
    public string SourceTitle { get => Source.Title; set => Source.Title = value; }
    [DefaultValue(1)]
    public int SourceFirstId { get => Source.FirstId; set => Source.FirstId = value; }
    [DefaultValue(1)]
    public int SourceLastId { get => Source.Count; set => _ = value; }
    [DefaultValue("")]
	public string SourceDirectory { get => Source.Directory; set => Source.Directory = value; }
    [DefaultValue(1)]
    public int SourceScanCurrent { get; set; }
    [DefaultValue(1)]
    public int SourceScanCount { get; set; }

    public bool IsSourceDirectoryExists => Directory.Exists(Source.Directory);
    public bool IsReadySourceDirectory => !string.IsNullOrEmpty(SourceDirectory);
    public bool IsReady => IsReadySourceId && IsReadySourceDirectory;
    public bool IsReadySourceId => SourceId is not 0;
    public bool IsReadySourceFirstId => SourceFirstId > 0;
    public bool IsReadySourceUserName => !Equals(SourceUserName, string.Empty);
    public bool IsReadyAbout => !string.IsNullOrEmpty(SourceAbout);

    public Action<TgSqlTableSourceViewModel> LoadAction { get; set; }
	public Action<TgSqlTableSourceViewModel> UpdateAction { get; set; }
	public Action<TgSqlTableSourceViewModel> DownloadAction { get; set; }
	public Action<TgSqlTableSourceViewModel> EditAction { get; set; }

	public TgSqlTableSourceViewModel(TgSqlTableSourceModel source, 
		Action<TgSqlTableSourceViewModel> loadAction, Action<TgSqlTableSourceViewModel> updateAction,
		Action<TgSqlTableSourceViewModel> downloadAction, Action<TgSqlTableSourceViewModel> editAction)
	{
		Source = source;
		LoadAction = loadAction;
		UpdateAction = updateAction;
		DownloadAction = downloadAction;
		EditAction = editAction;
	}

	public TgSqlTableSourceViewModel(TgSqlTableSourceModel source)
	{
		Source = source;
		LoadAction = _ => { };
		UpdateAction = _ => { };
		DownloadAction = _ => { };
		EditAction = _ => { };
        SourceDirectory = this.GetPropertyDefaultValue(nameof(SourceDirectory));
        SourceId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceId));
        SourceUserName = this.GetPropertyDefaultValue(nameof(SourceUserName));
        SourceFirstId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceFirstId));
        SourceLastId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceLastId));
        SourceAbout = this.GetPropertyDefaultValue(nameof(SourceAbout));
        SourceTitle = this.GetPropertyDefaultValue(nameof(SourceTitle));
        SourceScanCurrent = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceScanCurrent));
        SourceScanCount = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceScanCount));
    }

    public TgSqlTableSourceViewModel() : this(TgSqlUtils.CreateNewSource()) { }

    #endregion

    #region Public and private methods

    public override string ToString() => $"{Source} | {Progress}";

    public override string ToDebugString() => $"{base.ToDebugString()} | {TgCommonUtils.GetIsReady(IsReady)} | {SourceId} | {SourceFirstId}";

    public static TgSqlTableSourceViewModel CreateNew() => new();

    [RelayCommand]
	public void OnLoad() => LoadAction(this);

	[RelayCommand]
	public void OnUpdate() => UpdateAction(this);

	[RelayCommand]
	public void OnDownload() => DownloadAction(this);

	[RelayCommand]
	public void OnEdit() => EditAction(this);

    /// <summary>
    /// Set new source.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="title"></param>
    /// <param name="about"></param>
    public void SetSource(long id, string title, string about)
    {
        SourceId = id;
        SourceTitle = title;
        SourceAbout = about;
    }

	#endregion
}