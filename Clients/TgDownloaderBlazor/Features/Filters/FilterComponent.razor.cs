// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Domain;

namespace TgDownloaderBlazor.Features.Filters;

public partial class FilterComponent : TgPageComponentEnumerable<TgEfFilterEntity>
{
    #region Public and private fields, properties, constructor

    private TgEfFilterRepository FilterRepository { get; } = new(TgEfUtils.EfContext);

    #endregion

	#region Public and private methods

	protected override async Task OnInitializedAsync()
    {
	    await base.OnInitializedAsync();
	    if (!IsBlazorLoading)
		    return;

	    await using TgEfContext efContext = await EfFactory.CreateDbContextAsync();
	    if (!AppSettings.AppXml.IsExistsEfStorage)
	    {
		    IsBlazorLoading = false;
		    return;
	    }

		Items = (await FilterRepository.GetListAsync(0, 0, isNoTracking: false)).Items;
        ItemsCount = await FilterRepository.GetCountAsync();

        IsBlazorLoading = false;
    }

    #endregion
}