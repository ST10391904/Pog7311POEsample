using Prog7311PoeTwo;

var builder = WebApplication.CreateBuilder(args);

// MVC services
builder.Services.AddControllersWithViews();

// HttpClient factory
builder.Services.AddHttpClient();

// API base URL
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl!);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();