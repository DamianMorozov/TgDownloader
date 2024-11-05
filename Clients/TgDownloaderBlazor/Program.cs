// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// Radzen
// https://blazor.radzen.com/get-started
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<TgJsService>();
// Inject
builder.Services.AddHttpClient();

// Register TgEfContext as the DbContext for EF Core
builder.Services.AddDbContextFactory<TgEfContext>(options => options
	.UseSqlite(b => b.MigrationsAssembly(nameof(TgDownloaderBlazor))));
TgEfUtils.CreateAndUpdateDbAsync().GetAwaiter().GetResult();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
