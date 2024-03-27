// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Features.Sources;

public partial class SourceComponent : TgPageComponentEnumerable<TgEfSourceEntity>
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

	    await using var dbContext = await DbFactory.CreateDbContextAsync();
	    if (!AppSettings.AppXml.IsExistsFileStorage)
	    {
		    IsBlazorLoading = false;
		    return;
	    }

	    Items = dbContext.SourceRepo.GetEnumerable(0).OrderBy(x => x.UserName).ThenBy(x => x.Title).ToList();
        ItemsCount = dbContext.SourceRepo.GetCount();

        IsBlazorLoading = false;
	}

    #endregion
}