using domain.Entities;

namespace domain.Models.Response.Project;

public class ResumedTask: BaseEntity
{
    public string Title { get; set; }
    public bool Completed { get; set; }
}
