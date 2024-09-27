namespace domain.Models.Request.Task;

public class AddAttachmentModel
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Attachment { get; set; }
}
