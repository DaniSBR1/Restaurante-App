using Microsoft.EntityFrameworkCore;
using RestauranteApp.BL;
using RestauranteApp.DA.Context;
using RestauranteApp.DA.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RestauranteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMesaRepository, MesaRepository>();

builder.Services.AddScoped<MesaBL>();

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
    pattern: "{controller=Mesas}/{action=Index}/{id?}");

app.Run();
