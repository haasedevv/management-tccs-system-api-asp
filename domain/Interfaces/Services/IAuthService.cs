using domain.Entities;
using domain.Models.Request.Auth;
using domain.Models.Response.Auth;

namespace domain.Interfaces.Services;

public interface IAuthService
{
     Task<ServiceResponse<LoginResponseModel>> Login(LoginRequestModel body);
}
