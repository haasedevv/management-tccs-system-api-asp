using domain.Entities;

namespace domain.Models.Request.Task;

public class CreateTaskModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Deadline { get; set; }
    public ICollection<TaskAttachmentInfos> Attachments { get; set; }
}
