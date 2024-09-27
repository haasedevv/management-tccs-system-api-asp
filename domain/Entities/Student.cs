namespace domain.Entities;

public class Student : BaseEntity
{
    public int UserId { get; set; }
    public bool Available { get; set; } = true;
    public User User { get; set; }
}
