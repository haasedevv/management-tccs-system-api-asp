using domain.Enums;

namespace domain.Models.Request.Request;

public class ChangeStatusRequestModel
{
    public StatusRequestEnum Status { get; set; }
}
