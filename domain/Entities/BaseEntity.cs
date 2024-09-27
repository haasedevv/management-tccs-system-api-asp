using System.ComponentModel.DataAnnotations.Schema;

namespace domain.Entities;

public abstract class BaseEntity
{
    public virtual int Id { get; set; }
}
