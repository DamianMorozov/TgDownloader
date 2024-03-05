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
	    if (!IsLoading)
		    return;

	    await using var dbContext = await DbFactory.CreateDbContextAsync();
	    if (!AppSettings.AppXml.IsExistsFileStorage)
	    {
		    IsLoading = false;
		    return;
	    }

	    Items = dbContext.VersionRepo.GetEnumerable(0).OrderByDescending(x => x.Version).ToList();
        ItemsCount = dbContext.VersionRepo.GetCount();

        IsLoading = false;
	}

    #endregion
}