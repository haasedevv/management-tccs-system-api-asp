using domain.Enums;

namespace domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ProjectStatus Status { get; set; }
    public double? Grade { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public ICollection<ProjectTask> Tasks { get; } = new List<ProjectTask>();
    public ICollection<Request> Requests { get; } = new List<Request>();
    public ICollection<TaskAttachment> TaskAttachments { get; } = new List<TaskAttachment>();
}
