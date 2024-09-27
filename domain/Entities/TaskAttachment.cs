namespace domain.Entities;

public class TaskAttachment : BaseEntity
{
    public int ProjectTaskId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Attachment { get; set; }
    public DateTime Created { get; set; }
    public User User { get; set; }
    public ProjectTask ProjectTask { get; set; }
}
