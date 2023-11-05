// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models;

/// <summary>
/// Download settings.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
public class TgDownloadSettingsModel : ObservableObject, ITgCommon
{
	#region Public and private fields, properties, constructor

	public TgSqlContextManagerHelper ContextManager => TgSqlContextManagerHelper.Instance;

	public TgSqlTableSourceViewModel SourceVm { get; set; }
	[DefaultValue(false)]
	public bool IsRewriteFiles { get; set; }
	[DefaultValue(false)]
	public bool IsRewriteMessages { get; set; }
	[DefaultValue(true)]
	public bool IsJoinFileNameWithMessageId { get; set; }

	public TgDownloadSettingsModel()
	{
		SourceVm = TgSqlTableSourceViewModel.CreateNew();
		IsJoinFileNameWithMessageId = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsJoinFileNameWithMessageId));
		IsRewriteFiles = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsRewriteFiles));
		IsRewriteMessages = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsRewriteMessages));
	}

	#endregion

	#region Public and private methods

    public string ToDebugString() => $"{SourceVm.ToDebugString()}";

    public static TgDownloadSettingsModel CreateNew() => new();

    public async Task UpdateSourceWithSettingsAsync()
    {
        if (!SourceVm.IsReadySourceId)
            return;
        if (await ContextManager.SourceRepository.SaveByViewModelAsync(SourceVm))
            SourceVm.Source = await ContextManager.SourceRepository.GetAsync(SourceVm.Source.Id);
    }

    #endregion
}