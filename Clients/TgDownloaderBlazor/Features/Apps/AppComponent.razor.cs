// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Features.Apps;

public sealed partial class AppComponent : TgPageComponentEnumerable<TgEfAppEntity>
{
    #region Public and private fields, properties, constructor

    //

    #endregion

    #region Public and private methods

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (!IsLoading) return;

        await using var dbContext = await DbFactory.CreateDbContextAsync();
        Items = dbContext.AppsRepo.GetEnumerable(0).ToList();
        ItemsCount = dbContext.AppsRepo.GetCount();
        
        IsLoading = false;
    }

    private async Task GridLoadData(LoadDataArgs args)
    {
        try
        {
            //var result = await TgStorageService.GetApps(filter: $@"(contains(UID,""{SearchString}"") or contains(API_HASH,""{search}"") or contains(PHONE_NUMBER,""{search}"") or contains(PROXY_UID,""{search}"")) and {(string.IsNullOrEmpty(args.Filter) ? "true" : args.Filter)}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null);
            //switch (typeof(TEntity))
            //{
            //    case var cls when cls == typeof(TgEfAppEntity):
            //Items = dbContext.AppsRepo.GetEnumerable(0).Cast<TEntity>();
            //ItemsCount = dbContext.AppsRepo.GetCount();

            //var query = dbContext.Apps.AsNoTracking().AsQueryable();
            //// Filter via the Where method
            //if (!string.IsNullOrEmpty(args.Filter))
            //{
            //    //query = query.Where(args.Filter);
            //}
            //// Sort via the OrderBy method
            //if (!string.IsNullOrEmpty(args.OrderBy))
            //{
            //    //query = query.OrderBy(args.OrderBy);
            //}
            //// Perform paging via Skip and Take.
            //Items = query.Skip(args.Skip.Value).Take(args.Top.Value).ToList();
            //ItemsCount = query.Count();

            //        break;
            //    case var cls when cls == typeof(TgEfFilterEntity):
            //        Items = dbContext.FilterRepo.GetEnumerable(0).Cast<TEntity>();
            //        ItemsCount = dbContext.FilterRepo.GetCount();
            //        break;
            //    case var cls when cls == typeof(TgEfMessageEntity):
            //        Items = dbContext.MessageRepo.GetEnumerable(0).Cast<TEntity>();
            //        ItemsCount = dbContext.MessageRepo.GetCount();
            //        break;
            //    case var cls when cls == typeof(TgEfProxyEntity):
            //        Items = dbContext.ProxyRepo.GetEnumerable(0).Cast<TEntity>();
            //        ItemsCount = dbContext.ProxyRepo.GetCount();
            //        break;
            //    case var cls when cls == typeof(TgEfSourceEntity):
            //        Items = dbContext.SourceRepo.GetEnumerable(0).Cast<TEntity>();
            //        ItemsCount = dbContext.SourceRepo.GetCount();
            //        break;
            //    case var cls when cls == typeof(TgEfVersionEntity):
            //        Items = dbContext.VersionRepo.GetEnumerable(0).Cast<TEntity>();
            //        ItemsCount = dbContext.VersionRepo.GetCount();
            //        break;
            //}
            IsLoading = false;
        }
        catch (Exception ex)
        {
            NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Apps" });
        }
    }

    private async Task Search(ChangeEventArgs args)
    {
        SearchString = $"{args.Value}";
        await Grid.GoToPage(0);
        await Grid.Reload();
    }

    //private async Task AddButtonClick(MouseEventArgs args)
    //{
    //    await DialogService.OpenAsync<AddApp>("Add App", null);
    //    await Grid.Reload();
    //}

    //private async Task EditRow(DataGridRowMouseEventArgs<TgEfAppEntity> args)
    //{
    //    await DialogService.OpenAsync<EditApp>("Edit App", new Dictionary<string, object> { { "UID", args.Data.Uid } });
    //    await Grid.Reload();
    //}

    //private async Task GridDeleteButtonClick(MouseEventArgs args, TgEfAppEntity app)
    //{
    //    try
    //    {
    //        if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
    //        {
    //            var deleteResult = await TgStorageService.DeleteApp(uid: app.Uid);

    //            if (deleteResult != null)
    //            {
    //                await Grid.Reload();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        NotificationService.Notify(new NotificationMessage
    //        {
    //            Severity = NotificationSeverity.Error,
    //            Summary = $"Error",
    //            Detail = $"Unable to delete App"
    //        });
    //    }
    //}

    //private async Task ExportClick(RadzenSplitButtonItem args)
    //{
    //    if (args?.Value == "csv")
    //    {
    //        await TgStorageService.ExportAppsToCSV(new Query
    //        {
    //            Filter = $@"{(string.IsNullOrEmpty(Grid.Query.Filter) ? "true" : Grid.Query.Filter)}",
    //            OrderBy = $"{Grid.Query.OrderBy}",
    //            Expand = "",
    //            Select = string.Join(",", Grid.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
    //        }, "Apps");
    //    }

    //    if (args == null || args.Value == "xlsx")
    //    {
    //        await TgStorageService.ExportAppsToExcel(new Query
    //        {
    //            Filter = $@"{(string.IsNullOrEmpty(Grid.Query.Filter) ? "true" : Grid.Query.Filter)}",
    //            OrderBy = $"{Grid.Query.OrderBy}",
    //            Expand = "",
    //            Select = string.Join(",", Grid.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
    //        }, "Apps");
    //    }
    //}

    #endregion
}