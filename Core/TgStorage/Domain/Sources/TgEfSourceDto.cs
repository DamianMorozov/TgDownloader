// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Sources;

/// <summary> Source DTO </summary>
public sealed partial class TgEfSourceDto : TgDtoBase, ITgDto<TgEfSourceDto, TgEfSourceEntity>
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private long _id;
	[ObservableProperty]
	private long _accessHash = -1;
	[ObservableProperty]
	private string _userName = string.Empty;
	[ObservableProperty]
	private DateTime _dtChanged = DateTime.MinValue;
	[ObservableProperty]
	private bool _isSourceActive;
	[ObservableProperty]
	private string _title = string.Empty;
	[ObservableProperty]
	private string _about = string.Empty;
	[ObservableProperty]
	private int _firstId;
	[ObservableProperty]
	private int _count;
	[ObservableProperty]
	private string _directory = string.Empty;
	[ObservableProperty]
	private bool _isAutoUpdate;
	[ObservableProperty]
	private long _currentFileSize;
	[ObservableProperty]
	private long _currentFileTransmitted;
	[ObservableProperty]
	private long _currentFileSpeed;
	[ObservableProperty]
	private bool _isDownload;
	[ObservableProperty]
	private int _sourceScanCurrent = 1;
	[ObservableProperty]
	private int _sourceScanCount;
	[ObservableProperty]
	private string _currentFileName = string.Empty;

	public string DtChangedString => $"{DtChanged:yyyy-MM-dd HH:mm:ss}";

	public float Progress => (float)FirstId * 100 / Count;
	public string ProgressPercentString => Progress == 0 ? "{0:00.00}" : $"{Progress:#00.00} %";
	public bool IsComplete => FirstId >= Count;
	public string ProgressItemString => $"{FirstId} from {Count}";
	public float CurrentFileProgress => CurrentFileSize > 0 ? (float)CurrentFileTransmitted * 100 / CurrentFileSize : 0;
	public string CurrentFileProgressPercentString =>
		(CurrentFileProgress == 0 ? "{0:00.00}" : $"{CurrentFileProgress:#00.00}") + " %";
	public string CurrentFileProgressMBString =>
		(CurrentFileTransmitted == 0 ? "{0:0.00}" : $"{(float)CurrentFileTransmitted / 1024 / 1024:### ##0.00}") + " from " +
		(CurrentFileSize == 0 ? "{0:0.00}" : $"{(float)CurrentFileSize / 1024 / 1024:### ##0.00}") + " MB";
	public string CurrentFileSpeedKBString =>
		(CurrentFileSpeed == 0 ? "{0:0.00}" : $"{(float)CurrentFileSpeed / 1024:### 000.00}") + " KB/sec";
	public string CurrentFileSpeedMBString =>
		(CurrentFileSpeed == 0 ? "{0:0.00}" : $"{(float)CurrentFileSpeed / 1024 / 1024:##0.00}") + " MB/sec";
	public bool IsReadySourceId => Id > 0;
	public bool IsReadySourceDirectory => !string.IsNullOrEmpty(Directory) && System.IO.Directory.Exists(Directory);
	public string IsReadySourceDirectoryDescription => IsReadySourceDirectory
		? $"{TgLocaleHelper.Instance.TgDirectoryIsExists}." : $"{TgLocaleHelper.Instance.TgDirectoryIsNotExists}!";
	public bool IsReady => IsReadySourceId && IsReadySourceDirectory;
	public bool IsReadySourceFirstId => FirstId > 0;

	#endregion

	#region Public and private methods

	public override string ToString() => ProgressPercentString;

	public TgEfSourceDto Fill(TgEfSourceDto dto, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = dto.Uid;
		DtChanged = dto.DtChanged;
		Id = dto.Id;
		AccessHash = dto.AccessHash;
		IsSourceActive = dto.IsActive;
		UserName = dto.UserName;
		Title = dto.Title;
		About = dto.About;
		FirstId = dto.FirstId;
		Count = dto.Count;
		Directory = dto.Directory;
		IsAutoUpdate = dto.IsAutoUpdate;

		SourceScanCurrent = dto.SourceScanCurrent;
		SourceScanCount = dto.SourceScanCount;
		CurrentFileName = dto.CurrentFileName;

		return this;
	}

	public TgEfSourceDto Fill(TgEfSourceEntity item, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = item.Uid;
		DtChanged = item.DtChanged;
		Id = item.Id;
		AccessHash = item.AccessHash;
		IsSourceActive = item.IsActive;
		UserName = item.UserName ?? string.Empty;
		Title = item.Title ?? string.Empty;
		About = item.About ?? string.Empty;
		FirstId = item.FirstId;
		Count = item.Count;
		Directory = item.Directory ?? string.Empty;
		IsAutoUpdate = item.IsAutoUpdate;

		SourceScanCurrent = 1;
		SourceScanCount = 1;
		CurrentFileName = string.Empty;

		return this;
	}

	public TgEfSourceDto GetDto(TgEfSourceEntity item)
	{
		var dto = new TgEfSourceDto();
		dto.Fill(item, isUidCopy: true);
		return dto;
	}

	public TgEfSourceEntity GetEntity() => new()
	{
		Uid = Uid,
		DtChanged = DtChanged,
		Id = Id,
		AccessHash = AccessHash,
		IsActive = IsSourceActive,
		UserName = UserName,
		Title = Title,
		About = About,
		FirstId = FirstId,
		Count = Count,
		Directory = Directory,
		IsAutoUpdate = IsAutoUpdate,
	};

	public void SetIsDownload(bool isDownload) => IsDownload = isDownload;

	#endregion
}