// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Features.Sources;

public partial class SourceComponent : TgPageComponentEnumerable<TgEfSourceEntity>
{
	#region Public and private fields, properties, constructor

	private TgEfSourceRepository SourceRepository { get; } = new(TgEfUtils.EfContext);

	#endregion

	#region Public and private methods

	~SourceComponent()
	{
		SourceRepository.Dispose();
	}

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

	    Items = (await SourceRepository.GetEnumerableAsync(0, isNoTracking: false))
			.Items.OrderBy(x => x.UserName).ThenBy(x => x.Title).ToList();
        ItemsCount = await SourceRepository.GetCountAsync();

        IsBlazorLoading = false;
	}

    #endregion
}