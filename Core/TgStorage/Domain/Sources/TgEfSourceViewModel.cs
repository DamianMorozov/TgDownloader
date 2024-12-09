// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Sources;

/// <summary> View-model for TgSqlTableSourceModel </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgEfSourceViewModel : TgViewModelBase
{
    #region Public and private fields, properties, constructor

    public TgEfSourceRepository SourceRepository { get; } = new(TgEfUtils.EfContext);
    public TgEfSourceEntity Item { get; set; } = default!;
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
			TgEfStorageResult<TgEfSourceEntity> storageResult = SourceRepository.Get(
				new() { Uid = value }, isNoTracking: false);
			Item = storageResult.IsExists
				? storageResult.Item
				: SourceRepository.GetNew(isNoTracking: false).Item;
		}
	}

	[DefaultValue("")]
	public DateTime SourceDtChanged { get => Item.DtChanged; set => Item.DtChanged = value; }
	[DefaultValue("")]
	public string SourceDtChangedString => $"{SourceDtChanged:yyyy-mm-dd HH:mm:ss}";
	[DefaultValue(0)]
    public long SourceId { get => Item.Id; set => Item.Id = value; }
	[DefaultValue(-1)]
	public long AccessHash { get => Item.AccessHash; set => Item.AccessHash = value; }
	[DefaultValue(false)]
	public bool IsContactActive { get => Item.IsActive; set => Item.IsActive = value; }
	[DefaultValue("")]
    public string SourceUserName { get => Item.UserName ?? string.Empty; set => Item.UserName = value; }
    [DefaultValue("")]
    public string SourceAbout { get => Item.About ?? string.Empty; set => Item.About = value; }
    [DefaultValue("")]
    public string SourceTitle { get => Item.Title ?? string.Empty; set => Item.Title = value; }
    [DefaultValue(1)]
    public int SourceFirstId { get => Item.FirstId; set => Item.FirstId = value; }
    [DefaultValue(1)]
    public int SourceLastId { get => Item.Count; set => Item.Count = value; }
    [DefaultValue("")]
    public string SourceDirectory { get => Item.Directory ?? string.Empty; set => Item.Directory = value; }
    [DefaultValue("")] 
    public string CurrentFileName { get; set; } = default!;
    [DefaultValue(1)]
    public int SourceScanCurrent { get; set; }
    [DefaultValue(1)]
    public int SourceScanCount { get; set; }
    public bool IsAutoUpdate { get => Item.IsAutoUpdate; set => Item.IsAutoUpdate = value; }
    public bool IsReadySourceDirectory => !string.IsNullOrEmpty(SourceDirectory) && Directory.Exists(SourceDirectory);
    public string IsReadySourceDirectoryDescription => IsReadySourceDirectory 
        ? $"{TgLocaleHelper.Instance.TgDirectoryIsExists}." : $"{TgLocaleHelper.Instance.TgDirectoryIsNotExists}!";
    public bool IsReady => IsReadySourceId && IsReadySourceDirectory;
    public bool IsReadySourceId => SourceId > 0;
    public bool IsReadySourceFirstId => SourceFirstId > 0;
    public bool IsDownload { get; private set; }

    public TgEfSourceViewModel(TgEfSourceEntity item) : base()
    {
		Default(item);
    }

    public TgEfSourceViewModel() : base()
    {
	    TgEfSourceEntity item = SourceRepository.GetNew(false).Item;
		Default(item);
	}

    #endregion

    #region Public and private methods

    private void Default(TgEfSourceEntity item)
    {
	    Item = item;
	    SourceUid = item.Uid;
		SourceDtChanged = item.DtChanged;
		SourceId = item.Id;
		AccessHash = item.AccessHash;
		IsActive = item.IsActive;
	    SourceUserName = item.UserName ?? string.Empty;
	    SourceAbout = item.About ?? string.Empty;
	    SourceTitle = item.Title ?? string.Empty;
		SourceDirectory = item.Directory ?? string.Empty;
		SourceFirstId = item.FirstId;
		SourceLastId = item.Count;
	    SourceScanCurrent = this.GetDefaultPropertyInt(nameof(SourceScanCurrent));
	    SourceScanCount = this.GetDefaultPropertyInt(nameof(SourceScanCount));
	    IsAutoUpdate = this.GetDefaultPropertyBool(nameof(IsAutoUpdate));
	    CurrentFileName = string.Empty;
	}

    public override string ToString() => $"{Item} | {Progress}";

    public override string ToDebugString() => $"{base.ToDebugString()} | {TgCommonUtils.GetIsReady(IsReady)} | " +
        $"{TgCommonUtils.GetIsAutoUpdate(IsAutoUpdate)} | {SourceUserName} | {SourceId} | {SourceFirstId}";

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