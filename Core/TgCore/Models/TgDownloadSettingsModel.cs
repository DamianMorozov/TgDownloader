// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgCore.Models;

/// <summary>
/// Download settings.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public class TgDownloadSettingsModel : ObservableObject, ITgCommon
{
	#region Public and private fields, properties, constructor

	[DefaultValue(0)]
	public long SourceId { get; set; }
	[DefaultValue("")]
	public string SourceUserName { get; set; }
	[DefaultValue("")]
	public string SourceTitle { get; private set; }
	[DefaultValue("")]
	public string SourceAbout { get; private set; }
	[DefaultValue("")]
	public string DestDirectory { get; set; }
	[DefaultValue(1)]
	public int SourceFirstId { get; set; }
	[DefaultValue(1)]
	public int SourceLastId { get; set; }
	[DefaultValue(1)]
	public int SourceScanCurrent { get; set; }
	[DefaultValue(1)]
	public int SourceScanCount { get; set; }
	[DefaultValue(false)]
	public bool IsRewriteFiles { get; set; }
	[DefaultValue(false)]
	public bool IsRewriteMessages { get; set; }
	[DefaultValue(true)]
	public bool IsJoinFileNameWithMessageId { get; set; }
	[DefaultValue(false)]
	public bool IsAutoUpdate { get; set; }
	public bool IsReady => IsReadySourceId && IsReadyDestDirectory;
	public bool IsReadySourceId => SourceId is not 0;
	public bool IsReadySourceFirstId => SourceFirstId > 0;
	public bool IsReadySourceUserName => !Equals(SourceUserName, string.Empty);
	public bool IsReadyAbout => !string.IsNullOrEmpty(SourceAbout);
	public bool IsReadyDestDirectory => !string.IsNullOrEmpty(DestDirectory);

	public TgDownloadSettingsModel()
	{
		DestDirectory = this.GetPropertyDefaultValue(nameof(DestDirectory));
		IsJoinFileNameWithMessageId = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsJoinFileNameWithMessageId));
		IsAutoUpdate = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsAutoUpdate));
		IsRewriteFiles = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsRewriteFiles));
		IsRewriteMessages = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsRewriteMessages));
		SourceLastId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceLastId));
		SourceFirstId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceFirstId));
		SourceScanCurrent = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceScanCurrent));
		SourceScanCount = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceScanCount));
		SourceId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceId));
		SourceUserName = this.GetPropertyDefaultValue(nameof(SourceUserName));
		SourceTitle = this.GetPropertyDefaultValue(nameof(SourceTitle));
		SourceAbout = this.GetPropertyDefaultValue(nameof(SourceAbout));
	}

	#endregion

	#region Public and private methods

	public void Reset()
	{
		DestDirectory = this.GetPropertyDefaultValue(nameof(DestDirectory));
		IsJoinFileNameWithMessageId = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsJoinFileNameWithMessageId));
		IsAutoUpdate = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsAutoUpdate));
		IsRewriteFiles = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsRewriteFiles));
		IsRewriteMessages = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsRewriteMessages));
		SourceFirstId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceFirstId));
		SourceLastId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceLastId));
		SourceScanCurrent = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceScanCurrent));
		SourceScanCount = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceScanCount));
		SourceId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(SourceId));
		SourceUserName = this.GetPropertyDefaultValue(nameof(SourceUserName));
		SourceTitle = this.GetPropertyDefaultValue(nameof(SourceTitle));
		SourceAbout = this.GetPropertyDefaultValue(nameof(SourceAbout));
	}

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