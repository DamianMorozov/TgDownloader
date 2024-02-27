// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Features.Pages;

public sealed partial class AppComponent : TgPageComponent
{
	#region Public and private fields, properties, constructor

	private IList<TgEfAppEntity> Items { get; set; } = new List<TgEfAppEntity>();

	#endregion

	#region Public and private methods

	protected override async Task OnInitializedAsync()
	{
        await base.OnInitializedAsync();
		
        await using var dbContext = await DbFactory.CreateDbContextAsync();
#if DEBUG
		Debug.WriteLine($"Apps page | {dbContext}");
#endif
		Items = dbContext.AppsRepo.GetEnumerable(0).ToList();
	}

	#endregion
}