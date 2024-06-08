using Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Api;

public class DataContext:DbContext
{
    public DbSet<SeedPoint> SeedPoints{get;set;}
    public DbSet<Department> Departments{get;set;}
    public DbSet<Employee> Employees{get;set;}

    public string DbPath { get; }
    public DataContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "employee.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
