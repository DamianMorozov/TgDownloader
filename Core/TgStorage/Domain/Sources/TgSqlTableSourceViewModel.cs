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
    public DateTime SourceDtChanged { get => Source.DtChanged; set => Source.DtChanged = value; }
    [DefaultValue("")]
    public string SourceUserName { get => Source.UserName; set => Source.UserName = value; }
    [DefaultValue("")]
    public string SourceAbout { get => Source.About; set => Source.About = value; }
    [DefaultValue("")]
    public string SourceTitle { get => Source.Title; set => Source.Title = value; }
    [DefaultValue(1)]
    public int SourceFirstId { get => Source.FirstId; set => Source.FirstId = value; }
    [DefaultValue(1)]
    public int SourceLastId { get => Source.Count; set => Source.Count = value; }
    [DefaultValue("")]
    public string SourceDirectory { get => Source.Directory; set => Source.Directory = value; }
    [DefaultValue(1)]
    public int SourceScanCurrent { get; set; }
    [DefaultValue(1)]
    public int SourceScanCount { get; set; }
    public bool IsAutoUpdate { get => Source.IsAutoUpdate; set => Source.IsAutoUpdate = value; }

    public bool IsReadySourceDirectory => !string.IsNullOrEmpty(SourceDirectory) && Directory.Exists(SourceDirectory);
    public string IsReadySourceDirectoryDescription => IsReadySourceDirectory 
        ? $"{TgLocaleHelper.Instance.TgDirectoryIsExists}." : $"{TgLocaleHelper.Instance.TgDirectoryIsNotExists}!";
    public bool IsReady => IsReadySourceId && IsReadySourceDirectory;
    public bool IsReadySourceId => SourceId is not 0;
    public bool IsReadySourceFirstId => SourceFirstId > 0;
    public bool IsReadySourceUserName => !Equals(SourceUserName, string.Empty);
    public bool IsReadyAbout => !string.IsNullOrEmpty(SourceAbout);

    public TgSqlTableSourceViewModel(TgSqlTableSourceModel source)
    {
        Source = source;
        SourceDirectory = source.Directory;
        SourceId = source.Id;
        SourceDtChanged = source.DtChanged;
        SourceUserName = source.UserName;
        SourceFirstId = source.FirstId;
        SourceLastId = source.Count;
        SourceAbout = source.About;
        SourceTitle = source.Title;
        SourceScanCurrent = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceScanCurrent));
        SourceScanCount = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceScanCount));
        IsAutoUpdate = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsAutoUpdate));
    }

    public TgSqlTableSourceViewModel() : this(TgSqlUtils.CreateNewSource()) { }

    #endregion

    #region Public and private methods

    public override string ToString() => $"{Source} | {Progress}";

    public override string ToDebugString() => $"{base.ToDebugString()} | {TgCommonUtils.GetIsReady(IsReady)} | {TgCommonUtils.GetIsAutoUpdate(IsAutoUpdate)} | {SourceId} | {SourceFirstId}";

    public static TgSqlTableSourceViewModel CreateNew() => new();

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