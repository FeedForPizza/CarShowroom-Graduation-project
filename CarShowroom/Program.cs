using CarShowroom.Data;
using CarShowroom.Entities;
using Stripe;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using AutoMapper;
using CarShowroom.Models;

var builder = WebApplication.CreateBuilder(args);
StripeConfiguration.ApiKey = "sk_test_51MqzY8CQrFM3nxZSX49ZJeh8eGLzMbvyHf6N5ClJA8JqvyTEBNr2Hv7zFQHZIZrfkChtKV7rz0zUgO2XDUrw3TtW00jayPvczV";// Add services to the container.


builder.Services.AddControllersWithViews();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddDbContext<CarShowroomContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
