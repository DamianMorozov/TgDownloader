﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Components.Pages;

public partial class ProxyComponent : TgPageComponent
{
	#region Public and private fields, properties, constructor

	private IList<TgEfProxyEntity> Proxies { get; set; } = new List<TgEfProxyEntity>();

	#endregion

	#region Public and private methods

	protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

		await using var dbContext = await DbFactory.CreateDbContextAsync();
		Proxies = dbContext.ProxyRepo.GetEnumerable(0).ToList();
	}

	#endregion
}