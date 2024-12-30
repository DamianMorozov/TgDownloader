// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models;

/// <summary> Download settings </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgDownloadSettingsViewModel : ObservableRecipient, ITgCommon
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private TgEfContactViewModel _contactVm;
	[ObservableProperty]
	private TgEfSourceViewModel _sourceVm;
	[ObservableProperty]
	private TgEfStoryViewModel _storyVm;
	[ObservableProperty]
	private TgEfVersionViewModel _versionVm;
	[ObservableProperty]
	private bool _isRewriteFiles;
	[ObservableProperty]
	private bool _isRewriteMessages;
	[ObservableProperty]
	private bool _isJoinFileNameWithMessageId;
	[ObservableProperty]
	[DefaultValue(5)]
	private int _countThreads;
	[ObservableProperty]
	private TgDownloadChat _chat;

	public TgDownloadSettingsViewModel()
	{
		ContactVm = new();
		SourceVm = new();
		StoryVm = new();
		VersionVm = new();
		_chat = new();

		IsJoinFileNameWithMessageId = true;
		IsRewriteFiles = false;
		IsRewriteMessages = false;
		CountThreads = 5;
	}

	#endregion

	#region Public and private methods

    public string ToDebugString() => $"{SourceVm.ToDebugString()}";

    public async Task UpdateSourceWithSettingsAsync()
    {
        if (!SourceVm.Dto.IsReadySourceId) return;
		var entity = SourceVm.Dto.GetEntity();
        _ = await SourceVm.SaveAsync(entity);
    }

    public async Task UpdateContactWithSettingsAsync()
    {
        if (!ContactVm.Dto.IsReady) return;
        _ = await ContactVm.SaveAsync();
    }

    public async Task UpdateStoryWithSettingsAsync()
    {
        if (!StoryVm.Dto.IsReady) return;
        _ = await StoryVm.SaveAsync();
    }

    #endregion
}