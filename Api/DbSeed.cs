using Microsoft.EntityFrameworkCore;
using Api.Model;
namespace Api;

public class SeedPoint{
    public int Id{get;set;}
    public string? Name{get;set;}
    public bool Seeded{get;set;}
}

public  class DbSeed
{
    private DataContext context;
    public DbSeed(DataContext dataContext){
        context = dataContext;
    }
    private SeedPoint GetSeedPoint(string name){
        var seedPoint = context.SeedPoints.Where(s=>s.Name == name).FirstOrDefault();
        if(seedPoint == null){
            seedPoint = new SeedPoint(){Name=name, Seeded=false};
            context.SeedPoints.Add(seedPoint);
            context.SaveChanges();
        }
        return seedPoint;
    }
    public  void SeedDepartmentAndEmployeeData(bool clear=false){
        context.Database.EnsureCreated();
        var seedPoint = GetSeedPoint("department_employee");
        if(clear){
            context.Employees.ExecuteDelete();
            context.Departments.ExecuteDelete();
        }
        else if (seedPoint.Seeded)
            return ;
        var it = new Department(){
            Name="IT",
            Employees = new List<Employee>(){
                new Employee(){ Name="Chit Swe Lin", JobPosition="Head of IT", EmploymentType= EmploymentType.FULL_TIME},
                new Employee(){Name="Mg Mg", JobPosition="Network Administrator", EmploymentType= EmploymentType.PART_TIME}
            },
            
        };
        var finance = new Department(){
            Name="Finance",
            Employees = new List<Employee>(){
                new Employee(){Name="Aye Aye", JobPosition="Chief Accountant"},
                new Employee(){Name="Aung Aung", JobPosition = "Senior Accountant"}
            }
        };
        context.Departments.Add(it);
        context.Departments.Add(finance);
        seedPoint.Seeded = true;
        context.SaveChanges();
    }
}
