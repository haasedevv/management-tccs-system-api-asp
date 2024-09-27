namespace domain.Models.Request.Request;

public class CreateRequestModel
{
    public int ProjectId { get; set; }
    public int CreatorId { get; set; }
    public List<int> UsersId { get; set; }
}
