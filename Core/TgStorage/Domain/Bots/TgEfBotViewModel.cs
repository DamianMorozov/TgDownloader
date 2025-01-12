// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Bots;

/// <summary> Bot view-model </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgEfBotViewModel : TgEntityViewModelBase<TgEfBotEntity>, ITgDtoViewModel
{
	#region Public and private fields, properties, constructor

	public override TgEfBotRepository Repository { get; } = new(TgEfUtils.EfContext);
	[ObservableProperty]
	public partial TgEfBotDto Dto { get; set; } = default!;

	public TgEfBotViewModel(TgEfBotEntity item) : base()
	{
		Fill(item);
	}

	public TgEfBotViewModel() : base()
	{
		TgEfBotEntity item = new();
		Fill(item);
	}

	#endregion

	#region Public and private methods

	public override string ToString() => Dto.ToString() ?? string.Empty;

	public override string ToDebugString() => Dto.ToDebugString();

	public void Fill(TgEfBotEntity item)
	{
		Dto ??= new();
		Dto.Fill(item, isUidCopy: true);
	}

	public async Task<TgEfStorageResult<TgEfBotEntity>> SaveAsync() =>
		await Repository.SaveAsync(Dto.GetEntity());

	#endregion
}