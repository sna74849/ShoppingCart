using DBManager;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// セッションの有効化
builder.Services.AddDistributedMemoryCache();
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

// DatabaseFramework,Service,DaoをTransientでDI登録
builder.Services.AddTransient<DatabaseFramework>();
builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<CartService>();
builder.Services.AddTransient<ItemService>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<RegisterService>();
builder.Services.AddTransient<CustomerDao>();
builder.Services.AddTransient<DestinationDao>();
builder.Services.AddTransient<ItemSalesStockDao>();
builder.Services.AddTransient<OrderDao>();
builder.Services.AddTransient<OrderHeaderDao>();
builder.Services.AddTransient<OrderDetailDao>();
builder.Services.AddTransient<StockDao>();
builder.Services.AddTransient<SalesDao>();

var app = builder.Build();

// キャッシュを無効化してヒストリーバックも再読み込みを促す
app.Use(async (context, next) =>
{
    context.Response.Headers.CacheControl = "no-store, no-cache, must-revalidate";
    context.Response.Headers.Pragma = "no-cache";
    context.Response.Headers.Expires = "0";

    await next();
});

if (!app.Environment.IsDevelopment()) {
    // Configure the HTTP request pipeline.
    app.UseExceptionHandler("/Home/Error");
} 

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// セッションの有効化
app.UseSession();

app.Run();
