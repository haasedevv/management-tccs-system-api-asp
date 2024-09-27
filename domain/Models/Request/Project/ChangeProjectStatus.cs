using domain.Enums;

namespace domain.Models.Request.Project;

public class ChangeProjectStatus
{
    public ProjectStatus Status { get; set; }
}
