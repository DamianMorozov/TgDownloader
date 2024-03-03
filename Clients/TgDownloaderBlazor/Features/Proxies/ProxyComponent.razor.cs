// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Features.Proxies;

public partial class ProxyComponent : TgPageComponentEnumerable<TgEfProxyEntity>
{
	#region Public and private fields, properties, constructor

	//

	#endregion

	#region Public and private methods

	protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

		await using var dbContext = await DbFactory.CreateDbContextAsync();
		Items = dbContext.ProxyRepo.GetEnumerable(0).ToList();
        ItemsCount = dbContext.ProxyRepo.GetCount();

        IsLoading = false;
	}

    #endregion
}