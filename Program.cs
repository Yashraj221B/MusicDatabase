using Microsoft.Extensions.FileProviders;
using MusicDatabase.Data;
using System.Threading;
using System.Diagnostics;

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

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Songs")),
    RequestPath = "/Songs",
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Process ps = new Process();
// ProcessStartInfo StartInfo = new ProcessStartInfo();
// StartInfo.FileName = "python.exe " + Path.Combine(builder.Environment.ContentRootPath,"DownloaderClient.py");
// ps.StartInfo = StartInfo;
// ps.Start();

app.Run();

// ps.Kill();