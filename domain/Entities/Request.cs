using domain.Enums;

namespace domain.Entities;

public class Request : BaseEntity
{
    public int ProjectId { get; set; }
    public int UserId { get; set; }
    public StatusRequestEnum Status { get; set; }
    public TypeRequestEnum Type { get; set; }
    public User User { get; set; }
    public Project Project { get; set; }
}
