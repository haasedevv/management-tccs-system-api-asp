using domain.Entities;

namespace domain.Models.Response.Project;

public class ResumedProject : BaseEntity
{
    public string Name { get; set; }
    public IEnumerable<String> Students { get; set; }
    public IEnumerable<String>? Leaders { get; set; }
}
