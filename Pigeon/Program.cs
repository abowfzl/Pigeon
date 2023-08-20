using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pigeon.Algoritms;
using Pigeon.Contracts;
using Pigeon.Crawler;
using Pigeon.Data;
using Pigeon.Factories;
using Pigeon.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PigeonDbContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PigeonDbContext>();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation().AddRazorPagesOptions(options =>
{
    options.Conventions.AuthorizeFolder("/Tickets");
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddHttpClient<ICrawler, LinkCrawler>();

builder.Services.AddScoped<UrlService>();
builder.Services.AddScoped<FetchService>();
builder.Services.AddScoped<LogRequestService>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<IndexPageFactory>();
builder.Services.AddScoped<TicketsFactory>();
builder.Services.AddSingleton<Apriori>();

builder.Services.AddHostedService<SchedulingBackgroundService>();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseMigrationsEndPoint();
//}
//else
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
