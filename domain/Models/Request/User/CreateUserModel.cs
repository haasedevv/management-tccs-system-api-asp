using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using domain.Enums;
using Microsoft.Extensions.Options;

namespace domain.Models.Request.User;

public class CreateUserModel
{
    public string Name { get; set; }
    public string Password { get; set; }
    public string Enrollment { get; set; }
    public string Email { get; set; }
    public TypeUserEnum TypeUser { get; set; }
}
