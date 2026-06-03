using Amazon.S3;
using Newtonsoft.Json;
using AwsZapasAdrianJacek.Data;
using AwsZapasAdrianJacek.Repositories;
using AwsZapasAdrianJacek.Services;
using Microsoft.EntityFrameworkCore;
using AwsZapasAdrianJacek.Helper;
using AwsZapasAdrianJacek.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddTransient<RepositoryZapatillas>();


builder.Services.AddTransient<ServiceStorageS3>();

string miSecret = await HelperSecretManager.GetSecretAsync();
KeysModel model = JsonConvert.DeserializeObject<KeysModel>(miSecret);
builder.Services.AddSingleton<KeysModel>(x => model);
//string connectionString = builder.Configuration.GetConnectionString("MySql");
//string connectionString = await HelperSecretManager.GetSecretAsync();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySQL(model.MySql);
});
//builder.Services.AddDbContext<DataContext>(options => options.UseMySQL(connectionString));


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
