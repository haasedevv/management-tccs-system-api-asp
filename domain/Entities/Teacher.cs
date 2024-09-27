using domain.Enums;

namespace domain.Entities;

public class Teacher : BaseEntity
{
    public int UserId { get; set; }
    public AreaTypeEnum Area { get; set; }
    public bool AvailableOrientation { get; set; } = true;
    public bool AvailableEvaluation { get; set; } = true;
    public User User { get; set; }
}
