using Api;
using Api.Graph;
using Api.Service;
using Api.Store;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DataContext>(ServiceLifetime.Transient);
builder.Services.AddTransient<DbContext, DataContext>();
builder.Services.AddTransient<IUnitOfWork,UnitOfWork>();
builder.Services.AddTransient<EmployeeService>();
builder.Services.AddTransient<DepartmentService>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddApiGraphQL();
builder.Services.AddSingleton<DbSeed>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var dbSeed = app.Services.GetRequiredService<DbSeed>();
    dbSeed.SeedDepartmentAndEmployeeData(false);
}
app.UseGraphQL<RootSchema>();
app.UseGraphQLAltair();
app.UseHttpsRedirection();

app.Run();

