using Amazon.S3;
using AwsZapasAdrianJacek.Data;
using AwsZapasAdrianJacek.Repositories;
using AwsZapasAdrianJacek.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddTransient<RepositoryZapatillas>();


builder.Services.AddTransient<ServiceStorageS3>();

string connectionString = builder.Configuration.GetConnectionString("MySql");

builder.Services.AddDbContext<DataContext>(options => options.UseMySQL(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
