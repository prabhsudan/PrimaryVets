using Microsoft.EntityFrameworkCore;
using PrimaryVets.Database;
using PrimaryVets.Interface;
using PrimaryVets.Repository;
using Whisper.net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSingleton<WhisperFactory>(sp =>
{
    var baseDir = AppContext.BaseDirectory;

    var modelPath = Path.Combine(baseDir, "wwwroot", "ggml-tiny.bin");

    if (!File.Exists(modelPath))
    {
        throw new FileNotFoundException(
            $"Whisper Model File missing! Looked at: '{modelPath}'. " +
            "Ensure 'ggml-tiny.bin' has its properties set to: 'Copy to Output Directory = Copy if newer'."
        );
    }

    return WhisperFactory.FromPath(modelPath);
});


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

app.UseWebSockets();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
