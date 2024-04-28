// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Sources;

/// <summary>
/// View for TgSqlTableSourceModel.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgEfSourceViewModel : TgViewModelBase
{
    #region Public and private fields, properties, constructor

    public TgEfSourceEntity Item { get; set; }
    public float Progress => (float) Item.FirstId * 100 / Item.Count;
    public string ProgressPercentString => Progress == 0 ? "{0:00.00}" : $"{Progress:#00.00} %";
    public string ProgressItemString => $"{Item.FirstId} from {Item.Count}";
    public long CurrentFileSize { get; set; }
    public long CurrentFileTransmitted { get; set; }
    public float CurrentFileProgress => CurrentFileSize > 0 ? (float) CurrentFileTransmitted * 100 / CurrentFileSize : 0;
    public string CurrentFileProgressPercentString => 
	    (CurrentFileProgress == 0 ? "{0:00.00}" : $"{CurrentFileProgress:#00.00}") + " %";
    // ReSharper disable once InconsistentNaming
    public string CurrentFileProgressMBString => 
	    (CurrentFileTransmitted == 0 ? "{0:0.00}" : $"{(float) CurrentFileTransmitted / 1024 / 1024:### ##0.00}") + " from " +
		(CurrentFileSize == 0 ? "{0:0.00}" : $"{(float) CurrentFileSize / 1024 / 1024:### ##0.00}") + " MB";
    public long CurrentFileSpeed { get; set; }
    // ReSharper disable once InconsistentNaming
    public string CurrentFileSpeedKBString =>
	    (CurrentFileSpeed == 0 ? "{0:0.00}" : $"{(float) CurrentFileSpeed / 1024:### 000.00}") + " KB/sec";
	// ReSharper disable once InconsistentNaming
    public string CurrentFileSpeedMBString =>
	    (CurrentFileSpeed == 0 ? "{0:0.00}" : $"{(float) CurrentFileSpeed / 1024 / 1024:##0.00}") + " MB/sec";
    public bool IsComplete => Item.FirstId >= Item.Count;

	public Guid SourceUid
	{
		get => Item.Uid;
		set
		{
			TgEfOperResult<TgEfSourceEntity> operResult = EfContext.SourceRepository.GetAsync(
				new TgEfSourceEntity { Uid = value }, isNoTracking: false).GetAwaiter().GetResult();
			Item = operResult.IsExist
				? operResult.Item
				: EfContext.SourceRepository.GetNewAsync(isNoTracking: false).GetAwaiter().GetResult().Item;
		}
	}

	[DefaultValue(0)]
    public long SourceId { get => Item.Id; set => Item.Id = value; }
    [DefaultValue("")]
    public DateTime SourceDtChanged { get => Item.DtChanged; set => Item.DtChanged = value; }
    [DefaultValue("")]
	public string SourceDtChangedString => $"{SourceDtChanged:yyyy-mm-dd HH:mm:ss}";
	[DefaultValue("")]
    public string SourceUserName { get => Item.UserName; set => Item.UserName = value; }
    [DefaultValue("")]
    public string SourceAbout { get => Item.About; set => Item.About = value; }
    [DefaultValue("")]
    public string SourceTitle { get => Item.Title; set => Item.Title = value; }
    [DefaultValue(1)]
    public int SourceFirstId { get => Item.FirstId; set => Item.FirstId = value; }
    [DefaultValue(1)]
    public int SourceLastId { get => Item.Count; set => Item.Count = value; }
    [DefaultValue("")]
    public string SourceDirectory { get => Item.Directory; set => Item.Directory = value; }
    [DefaultValue("")]
    public string CurrentFileName { get; set; }
    [DefaultValue(1)]
    public int SourceScanCurrent { get; set; }
    [DefaultValue(1)]
    public int SourceScanCount { get; set; }
    public bool IsAutoUpdate { get => Item.IsAutoUpdate; set => Item.IsAutoUpdate = value; }
    public bool IsReadySourceDirectory => !string.IsNullOrEmpty(SourceDirectory) && Directory.Exists(SourceDirectory);
    public string IsReadySourceDirectoryDescription => IsReadySourceDirectory 
        ? $"{TgLocaleHelper.Instance.TgDirectoryIsExists}." : $"{TgLocaleHelper.Instance.TgDirectoryIsNotExists}!";
    public bool IsReady => IsReadySourceId && IsReadySourceDirectory;
    public bool IsReadySourceId => SourceId > 1;
    public bool IsReadySourceFirstId => SourceFirstId > 0;
    public bool IsDownload { get; private set; }

    public TgEfSourceViewModel(TgEfSourceEntity item)
    {
		Item = item;
        SourceDirectory = item.Directory;
        SourceId = item.Id;
        SourceDtChanged = item.DtChanged;
        SourceUserName = item.UserName;
        SourceFirstId = item.FirstId;
        SourceLastId = item.Count;
        SourceAbout = item.About;
        SourceTitle = item.Title;
        SourceScanCurrent = this.GetDefaultPropertyInt(nameof(SourceScanCurrent));
        SourceScanCount = this.GetDefaultPropertyInt(nameof(SourceScanCount));
        IsAutoUpdate = this.GetDefaultPropertyBool(nameof(IsAutoUpdate));
        CurrentFileName = string.Empty;
    }

    public TgEfSourceViewModel(TgEfContext efContext) : this(efContext.SourceRepository.CreateNewAsync().GetAwaiter().GetResult().Item) { }

    #endregion

    #region Public and private methods

    public override string ToString() => $"{Item} | {Progress}";

    public override string ToDebugString() => $"{base.ToDebugString()} | {TgCommonUtils.GetIsReady(IsReady)} | {TgCommonUtils.GetIsAutoUpdate(IsAutoUpdate)} | {SourceUserName} | {SourceId} | {SourceFirstId}";

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

    public void SetIsDownload(bool isDownload) => IsDownload = isDownload;

	#endregion
}