// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Features.Versions;

public partial class VersionComponent : TgPageComponentEnumerable<TgEfVersionEntity>
{
	#region Public and private fields, properties, constructor

	//

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

	    Items = efContext.VersionRepository.GetEnumerable(0, isNoTracking: false)
			.Items.OrderByDescending(x => x.Version).ToList();
        ItemsCount = efContext.VersionRepository.GetCount();

        IsBlazorLoading = false;
	}

    #endregion
}