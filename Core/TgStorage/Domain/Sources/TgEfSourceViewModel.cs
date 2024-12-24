// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Sources;

/// <summary> View-model for TgSqlTableSourceModel </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgEfSourceViewModel : TgViewModelBase
{
    #region Public and private fields, properties, constructor

    public TgEfSourceRepository SourceRepository { get; } = new(TgEfUtils.EfContext);
    [ObservableProperty]
    private TgEfSourceDto _dto = default!;

    public TgEfSourceViewModel(TgEfSourceEntity item) : base()
    {
		Default(item, isResetDto: true);
    }

    public TgEfSourceViewModel() : base()
    {
	    TgEfSourceEntity item = SourceRepository.GetNew().Item;
		Default(item, isResetDto: true);
	}

    #endregion

    #region Public and private methods

    public void Default(TgEfSourceEntity item, bool isResetDto)
    {
		if (isResetDto)
		{
			SetSourceUidAsync(item.Uid).GetAwaiter().GetResult();
			Dto.SourceDtChanged = item.DtChanged;
			Dto.Id = item.Id;
			Dto.AccessHash = item.AccessHash;
			Dto.IsSourceActive = item.IsActive;
			Dto.UserName = item.UserName ?? string.Empty;
			Dto.About = item.About ?? string.Empty;
			Dto.Title = item.Title ?? string.Empty;
			Dto.Directory = item.Directory ?? string.Empty;
			Dto.FirstId = item.FirstId;
			Dto.Count = item.Count;
			Dto.IsAutoUpdate = item.IsAutoUpdate;
		}

		Dto.SourceScanCurrent = this.GetDefaultPropertyInt(nameof(Dto.SourceScanCurrent));
	    Dto.SourceScanCount = this.GetDefaultPropertyInt(nameof(Dto.SourceScanCount));
	    Dto.CurrentFileName = string.Empty;
	}

    public override string ToString() => Dto.ToString();

    public override string ToDebugString() => $"{base.ToDebugString()} | {TgCommonUtils.GetIsReady(Dto.IsReady)} | " +
        $"{TgCommonUtils.GetIsAutoUpdate(Dto.IsAutoUpdate)} | {Dto.UserName} | {Dto.Id} | {Dto.FirstId}";

	public async Task SetSourceUidAsync(Guid uid)
	{
		var storageResult = await SourceRepository.GetAsync(new() { Uid = uid });
		Dto = TgEfHelper.ConvertToDto(storageResult.IsExists ? storageResult.Item : (await SourceRepository.GetNewAsync()).Item);
	}

	#endregion
}