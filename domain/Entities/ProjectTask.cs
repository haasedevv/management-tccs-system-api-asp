namespace domain.Entities;

public class ProjectTask: BaseEntity
{
    public int ProjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
    public DateTime Deadline { get; set; }

    public Project Project { get; set; }

    public ICollection<TaskAttachment> TaskAttachments {
        get;
        set;
    } = [];
    public ICollection<TaskComment> TaskComments {
        get;
        set;
    } = [];
}
