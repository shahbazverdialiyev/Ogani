using Microsoft.AspNetCore.Mvc;
using Ogani.Api.Handlers;
using Ogani.WebApp.API.Extensions;
using Ogani.WebApp.Business.Extensions;
using Ogani.WebApp.DataAccess.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDataAccessServices(connectionString);

builder.Services.AddBusinessServices();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

await app.SeedIdentityAsync();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();