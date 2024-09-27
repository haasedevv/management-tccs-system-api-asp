namespace domain.Entities;

public class TaskComment: BaseEntity
{
    public int TaskId { get; set; }
    public int UserId { get; set; }
    public string Comment { get; set; }
    public DateTime Created { get; set; }
    public User User { get; set; }
    public ProjectTask ProjectTask { get; set; }

}

