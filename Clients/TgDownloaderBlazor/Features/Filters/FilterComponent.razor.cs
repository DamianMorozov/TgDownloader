// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Features.Filters;

public partial class FilterComponent : TgPageComponentEnumerable<TgEfFilterEntity>
{
    #region Public and private fields, properties, constructor

    private TgEfFilterRepository FilterRepository { get; } = new(TgEfUtils.EfContext);

	~FilterComponent()
	{
		FilterRepository.Dispose();
	}

    #endregion

	#region Public and private methods

	protected override async Task OnInitializedAsync()
    {
	    await base.OnInitializedAsync();
	    if (!IsBlazorLoading)
		    return;

	    await using var efContext = await EfFactory.CreateDbContextAsync();
	    if (!AppSettings.AppXml.IsExistsFileStorage)
	    {
		    IsBlazorLoading = false;
		    return;
	    }

		Items = (await FilterRepository.GetEnumerableAsync(0, isNoTracking: false)).Items;
        ItemsCount = await FilterRepository.GetCountAsync();

        IsBlazorLoading = false;
    }

    #endregion
}