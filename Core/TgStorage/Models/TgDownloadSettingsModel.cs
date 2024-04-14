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

	public TgEfContext EfContext { get; } = default!;

	public TgEfSourceViewModel SourceVm { get; set; }
	
	public TgEfVersionViewModel VersionVm { get; set; }
	
	[DefaultValue(false)]
	public bool IsRewriteFiles { get; set; }
	[DefaultValue(false)]
	public bool IsRewriteMessages { get; set; }
	[DefaultValue(true)]
	public bool IsJoinFileNameWithMessageId { get; set; }

	public TgDownloadSettingsModel()
	{
		EfContext = TgStorageUtils.GetEfContextProd();
		SourceVm = new TgEfSourceViewModel(EfContext);
		IsJoinFileNameWithMessageId = this.GetDefaultPropertyBool(nameof(IsJoinFileNameWithMessageId));
		IsRewriteFiles = this.GetDefaultPropertyBool(nameof(IsRewriteFiles));
		IsRewriteMessages = this.GetDefaultPropertyBool(nameof(IsRewriteMessages));
	}

	#endregion

	#region Public and private methods

    public string ToDebugString() => $"{SourceVm.ToDebugString()}";

    public static TgDownloadSettingsModel CreateNew() => new();

    public async Task UpdateSourceWithSettingsAsync()
    {
        if (!SourceVm.IsReadySourceId)
            return;
        TgEfOperResult<TgEfSourceEntity> operResult = await EfContext.SourceRepository.SaveAsync(SourceVm.Item);
        if (operResult.IsExist) 
	        SourceVm.Item = operResult.Item;
    }

    #endregion
}