using domain.Enums;

namespace domain.Models.Response.Auth;

public class LoginResponseModel
{
    public string Token { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public bool? AvailableOrientation { get; set; }
    public bool? AvailableEvaluation { get; set; }
    public TypeUserEnum UserType { get; set; }
}
