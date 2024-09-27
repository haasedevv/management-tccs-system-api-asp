namespace domain.Models.Response.Project;

public class GetTasks
{
    public IEnumerable<ResumedTask> Tasks { get; set; }
}
