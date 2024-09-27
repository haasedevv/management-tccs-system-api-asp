namespace domain.Entities;

public class Authentication : BaseEntity
{
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime Created { get; set; }
}
