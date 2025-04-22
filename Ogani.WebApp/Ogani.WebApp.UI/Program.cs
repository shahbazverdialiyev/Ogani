using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ogani.WebApp.Business.Mappings.AutoMapper;
using Ogani.WebApp.Business.Validators.ProductValidators;
using Ogani.WebApp.Business.Validators.CategoryValidators;
using Ogani.WebApp.DataAccess.Contexts;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.Business.Services;
using Ogani.WebApp.DataAccess.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<OganiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<ProductCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ProductUpdateValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<CategoryCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CategoryUpdateValidator>();

builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();

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
