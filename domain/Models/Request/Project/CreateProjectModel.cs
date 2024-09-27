using domain.Enums;

namespace domain.Models.Request.Project;

public class CreateProjectModel
{
    public int CreatorId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int LeaderId { get; set; }
    public List<int> Group { get; set; }
}
