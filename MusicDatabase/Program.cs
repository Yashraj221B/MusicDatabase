using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

if (!Directory.Exists(Path.Combine(builder.Environment.ContentRootPath, "Songs")))
{
    Directory.CreateDirectory(Path.Combine(builder.Environment.ContentRootPath, "Songs"));
}

if (!Directory.Exists(Path.Combine(builder.Environment.ContentRootPath, "Thumbs")))
{
    Directory.CreateDirectory(Path.Combine(builder.Environment.ContentRootPath, "Thumbs"));
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Songs")),
    RequestPath = "/Songs",
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Thumbs")),
    RequestPath = "/Thumbs",
});


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();