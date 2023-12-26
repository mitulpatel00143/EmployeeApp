using DAL.Abstract;
using DAL.Services;
using Microsoft.AspNetCore.Http.Features;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmployee, EmployeeServices>();
builder.Services.AddScoped<ILocationServices, LocationServices>();
builder.Services.Configure<FormOptions>(options => options.BufferBody = true);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Employee/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=MainIndex}/{id?}");

app.Run();
