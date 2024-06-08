using System.ComponentModel.DataAnnotations;
using Data.Ef;
using Data.Graph;

namespace Api.Model;

[EntityGraphType("Employees")]
public class Department: Entity
{
    [Required]
    public string? Name{get;set;}
    public ICollection<Employee>? Employees{get;set;}
}
