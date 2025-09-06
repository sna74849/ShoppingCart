using DBManager;
using ShoppingCart.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ƒZƒbƒVƒ‡ƒ“‚Ì—LŒø‰»
builder.Services.AddDistributedMemoryCache();
// ƒZƒbƒVƒ‡ƒ“‚Ì—LŒø‰»
builder.Services.AddSession(options => {
    options.IOTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// JSでのJsonのCamelCase変換を無効化
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Singletonで接続管理をDI登録
builder.Services.AddSingleton(new ConnectionManager("shopping"));

// DatabaseServiceをTransientでDI登録
builder.Services.AddTransient<DatabaseService>();

var app = builder.Build();

// キャッシュを無効化してヒストリーバックも再読み込みを促す
app.Use(async (context, next) =>
{
    context.Response.Headers.CacheControl = "no-store, no-cache, must-revalidate";
    context.Response.Headers.Pragma = "no-cache";
    context.Response.Headers.Expires = "0";

    await next();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// ƒZƒbƒVƒ‡ƒ“‚Ì—LŒø‰»
app.UseSession();

app.Run();
