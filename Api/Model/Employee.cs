using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Api.Graph.Department;
using Api.Graph.Employee;
using Data.Ef;
using Data.Graph;
namespace Api.Model;

[EntityGraphType()]
public class Employee : ArchivableEntity
{
    [Required]
    [Description("Employee Name")]
    public string? Name{get;set;}
    [Description("Job Position")]
    public string? JobPosition{get;set;}
    [Description("Department")]    
    public Department? Department{get;set;}
    [Many2oneGraphQLField(graphType:typeof(DepartmentGraphType),entityType:typeof(Department), whereInputGraphType:typeof(DepartmentWhereInputGraphType), orderByInputGraphType:typeof(DepartmentOrderByInputGraphType),name:"Department")]
    public int DepartmentId{get;set;}
    [Required]
    [Description("Employment Type")]
    [GraphQLField(graphqlType:typeof(EmploymentTypeEnum))]
    public EmploymentType EmploymentType{get;set;}
}

public enum EmploymentType{
    [Description("Full time")]
    FULL_TIME=1,
    [Description("Contract")]
    CONTRACT=2,
    [Description("Part time")]
    PART_TIME=3
}
