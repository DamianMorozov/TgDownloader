// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models;

/// <summary> Download settings </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgDownloadSettingsViewModel : ObservableObject, ITgCommon
{
	#region Public and private fields, properties, constructor

	public TgEfContactViewModel ContactVm { get; set; }
	public TgEfSourceViewModel SourceVm { get; set; }
	public TgEfStoryViewModel StoryVm { get; set; }
	public TgEfVersionViewModel VersionVm { get; set; }
	[DefaultValue(false)]
	public bool IsRewriteFiles { get; set; }
	[DefaultValue(false)]
	public bool IsRewriteMessages { get; set; }
	[DefaultValue(true)]
	public bool IsJoinFileNameWithMessageId { get; set; }
	[DefaultValue(5)]
	public int CountThreads { get; set; }

	public TgDownloadSettingsViewModel()
	{
		ContactVm = new();
		SourceVm = new();
		StoryVm = new();
		VersionVm = new();
		
		IsJoinFileNameWithMessageId = this.GetDefaultPropertyBool(nameof(IsJoinFileNameWithMessageId));
		IsRewriteFiles = this.GetDefaultPropertyBool(nameof(IsRewriteFiles));
		IsRewriteMessages = this.GetDefaultPropertyBool(nameof(IsRewriteMessages));
		CountThreads = this.GetDefaultPropertyInt(nameof(CountThreads));
	}

	#endregion

	#region Public and private methods

    public string ToDebugString() => $"{SourceVm.ToDebugString()}";

    public async Task UpdateSourceWithSettingsAsync()
    {
        if (!SourceVm.IsReadySourceId) return;
        var storageResult = await SourceVm.SourceRepository.SaveAsync(SourceVm.Item);
        if (storageResult.IsExists) 
	        SourceVm.Item = storageResult.Item;
    }

    public async Task UpdateContactWithSettingsAsync()
    {
        if (!ContactVm.IsReady) return;
        var storageResult = await ContactVm.ContactRepository.SaveAsync(ContactVm.Item);
        if (storageResult.IsExists)
			ContactVm.Item = storageResult.Item;
    }

    public async Task UpdateStoryWithSettingsAsync()
    {
        if (!StoryVm.IsReady) return;
        var storageResult = await StoryVm.StoryRepository.SaveAsync(StoryVm.Item);
        if (storageResult.IsExists)
			StoryVm.Item = storageResult.Item;
    }

    #endregion
}