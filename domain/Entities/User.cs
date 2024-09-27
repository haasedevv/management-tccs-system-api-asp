using System.ComponentModel.DataAnnotations;
using domain.Enums;

namespace domain.Entities;

public class User: BaseEntity
{
    public User(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public string Login { get; }
    public string Password { get; }
    public string Enrollment { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public bool Available { get; set; } = true;
    public TypeUserEnum TypeUser { get; set; }
    public ICollection<Request> Requests { get; } = [];
    public Authentication? Authentication { get; } = null;
    public Teacher Teacher { get; set; }
    public Student Student { get; set; }
    public ICollection<TaskAttachment> TaskAttachments { get; } = [];
    public ICollection<TaskComment> TaskComments { get; } = [];
}
