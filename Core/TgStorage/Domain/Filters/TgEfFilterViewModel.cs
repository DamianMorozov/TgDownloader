// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Contacts;

/// <summary> Contact view-model </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgEfFilterViewModel : TgEntityViewModelBase<TgEfFilterEntity>, ITgDtoViewModel
{
	#region Public and private fields, properties, constructor

	public override TgEfFilterRepository Repository { get; } = new(TgEfUtils.EfContext);
	[ObservableProperty]
	private TgEfFilterDto _dto = default!;


	public TgEfFilterViewModel(TgEfFilterEntity item) : base()
	{
		Fill(item);
	}

	public TgEfFilterViewModel() : base()
	{
		TgEfFilterEntity item = Repository.GetNewItem();
		Fill(item);
	}

	#endregion

	#region Public and private methods

	public override string ToString() => Dto.ToString() ?? string.Empty;

	public override string ToDebugString() => Dto.ToDebugString();

	public void Fill(TgEfFilterEntity item)
	{
		Dto ??= new();
		Dto.Fill(item, isUidCopy: true);
	}

	public async Task<TgEfStorageResult<TgEfFilterEntity>> SaveAsync(TgEfFilterEntity item) =>
		await Repository.SaveAsync(item);

	public async Task<TgEfStorageResult<TgEfFilterEntity>> SaveAsync() =>
		await Repository.SaveAsync(Dto.GetEntity());

	#endregion
}