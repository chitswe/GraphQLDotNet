using System.ComponentModel.DataAnnotations;
using Data.Ef;

namespace Api.Graph.Department;

public class DepartmentDto : EntityDto{
    [Required]
    public required string Name{get;set;}
}

public class DepartmentInput: EntityInputGraphType<DepartmentDto>{
    
}