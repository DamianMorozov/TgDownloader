// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Sources;

public sealed partial class TgEfSourceDto : TgViewModelBase, ITgDbEntity, ITgDbFillEntity<TgEfSourceDto>
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private Guid _uid;
	[ObservableProperty]
	private long _id;
	[ObservableProperty]
	private long _accessHash = -1;
	[ObservableProperty]
	private string _userName = string.Empty;
	[ObservableProperty]
	private string _dtChanged = string.Empty;
	[ObservableProperty]
	private string _dtChangedString = string.Empty;
	[ObservableProperty]
	private DateTime _sourceDtChanged = DateTime.MinValue;
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
	[DefaultValue(1)]
	[ObservableProperty]
	private int _sourceScanCurrent = 1;
	[DefaultValue(1)]
	[ObservableProperty]
	private int _sourceScanCount;
	[ObservableProperty]
	private string _currentFileName = string.Empty;

	public float Progress => (float)FirstId * 100 / Count;
	public string ProgressPercentString => Progress == 0 ? "{0:00.00}" : $"{Progress:#00.00} %";
	public bool IsComplete => FirstId >= Count;
	[DefaultValue("")]
	public string SourceDtChangedString => $"{SourceDtChanged:yyyy-MM-dd HH:mm:ss}";
	public string ProgressItemString => $"{FirstId} from {Count}";
	public float CurrentFileProgress => CurrentFileSize > 0 ? (float)CurrentFileTransmitted * 100 / CurrentFileSize : 0;
	public string CurrentFileProgressPercentString =>
		(CurrentFileProgress == 0 ? "{0:00.00}" : $"{CurrentFileProgress:#00.00}") + " %";
	// ReSharper disable once InconsistentNaming
	public string CurrentFileProgressMBString =>
		(CurrentFileTransmitted == 0 ? "{0:0.00}" : $"{(float)CurrentFileTransmitted / 1024 / 1024:### ##0.00}") + " from " +
		(CurrentFileSize == 0 ? "{0:0.00}" : $"{(float)CurrentFileSize / 1024 / 1024:### ##0.00}") + " MB";
	// ReSharper disable once InconsistentNaming
	public string CurrentFileSpeedKBString =>
		(CurrentFileSpeed == 0 ? "{0:0.00}" : $"{(float)CurrentFileSpeed / 1024:### 000.00}") + " KB/sec";
	// ReSharper disable once InconsistentNaming
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

	public void Default()
	{
		throw new NotImplementedException();
	}

	public TgEfSourceDto Fill(TgEfSourceDto item, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = item.Uid;
		DtChangedString = string.IsNullOrEmpty(item.DtChanged) ? item.DtChanged : TgEfHelper.GetDtAsDateString(DateTime.Now);
		if (Id == this.GetDefaultPropertyLong(nameof(Id)))
			Id = item.Id;
		AccessHash = item.AccessHash;
		IsActive = item.IsActive;
		FirstId = item.FirstId;
		UserName = item.UserName;
		Title = item.Title;
		About = item.About;
		Count = item.Count;
		Directory = item.Directory;
		IsAutoUpdate = item.IsAutoUpdate;
		return this;
	}

	public void SetIsDownload(bool isDownload) => IsDownload = isDownload;

	/// <summary> Set new source </summary>
	public void SetSource(long id, string title, string about)
	{
		Id = id;
		Title = title;
		About = about;
	}

	#endregion
}
