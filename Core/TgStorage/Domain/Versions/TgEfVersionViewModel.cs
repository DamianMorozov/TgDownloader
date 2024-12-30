// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Versions;

/// <summary> Version view-model </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgEfVersionViewModel : TgEntityViewModelBase<TgEfVersionEntity>, ITgDtoViewModel
{
	#region Public and private fields, properties, constructor

	public override TgEfVersionRepository Repository { get; } = new(TgEfUtils.EfContext);
	[ObservableProperty]
	private TgEfVersionDto _dto = default!;

	public TgEfVersionViewModel(TgEfVersionEntity item) : base()
	{
		Fill(item);
	}

	public TgEfVersionViewModel() : base()
	{
		var item = Repository.GetNewItem();
		Fill(item);
	}

	#endregion

	#region Public and private methods

	public override string ToString() => Dto.ToString() ?? string.Empty;

	public override string ToDebugString() => Dto.ToDebugString();

	public void Fill(TgEfVersionEntity item)
	{
		Dto ??= new();
		Dto.Fill(item, isUidCopy: true);
	}

	public async Task<TgEfStorageResult<TgEfVersionEntity>> SaveAsync(TgEfVersionEntity item) =>
		await Repository.SaveAsync(item);

	public async Task<TgEfStorageResult<TgEfVersionEntity>> SaveAsync() =>
		await Repository.SaveAsync(Dto.GetEntity());

	#endregion
}