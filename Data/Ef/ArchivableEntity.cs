using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data.Ef;

public class ArchivableEntity: Entity
{
    [Required]
    [DefaultValue(true)]
    public bool Active{get;set;}
}
