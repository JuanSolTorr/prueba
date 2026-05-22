using PokemonApp.Database;
using PokemonApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register In-Memory Caching and HTTP Client
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();

// Register local JSON database context as singleton
builder.Services.AddSingleton<JsonDatabaseContext>();

// Register custom services
builder.Services.AddScoped<IPokemonService, PokemonService>();
builder.Services.AddScoped<IItemService, ItemService>();

var app = builder.Build();

// Seed/Initialize the Database in the background so the API can start responding immediately.
_ = Task.Run(async () =>
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<JsonDatabaseContext>();
        var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();
        var client = httpClientFactory.CreateClient();

        await dbContext.InitializeAsync(client);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred during database initialization: {ex.Message}");
    }
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
