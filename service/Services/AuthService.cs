using System.Linq.Expressions;
using domain.Entities;
using domain.Enums;
using domain.Models.Request.Auth;
using domain.Models.Response.Auth;
using infra.Data.Repository;

namespace service.Services;

public class AuthService
{
    private readonly BaseRepository<Authentication> _baseRepositoryAuthentication;
    private readonly UserRepository _userRepository;
    private readonly AuthenticationRepository _authenticationRepository;
    private readonly BaseRepository<User> _userBaseRepository;
    public AuthService(
        BaseRepository<Authentication> baseRepositoryAuthentication,
        BaseRepository<User> userBaseRepository,
        UserRepository userRepository,
        AuthenticationRepository authenticationRepository
        )
    {
        _userRepository = userRepository;
        _baseRepositoryAuthentication = baseRepositoryAuthentication;
        _authenticationRepository = authenticationRepository;
        _userBaseRepository = userBaseRepository;
    }

    public async Task<ServiceResponse<LoginResponseModel?>> Login(LoginRequestModel body)
    {
        try
        {
            var user = (await _userBaseRepository.FindBy(predicate: x => x.Login == body.Login,
                new Expression<Func<User, object>>[]
                {
                    x => x.Teacher
                })).FirstOrDefault();

            if(user == null) return new ServiceResponse<LoginResponseModel?>
            {
                Data = null,
                Message = "Usuário não encontrado!",
                Status = 404
            };

            var matchedPasswords = PasswordHasher.VerifyPasswordHash(body.Password, user.Password);

            if (!matchedPasswords) return new ServiceResponse<LoginResponseModel?>
            {
                Data = null,
                Message = "As senhas não conferem!",
                Status = 401
            };

            var sessionToken = JwtAuth.GenerateTokenSession(user);
            var authentication = new Authentication
            {
                UserId = user.Id,
                Token = sessionToken,
                Created = DateTime.UtcNow
            };

            var lastAuthentication = await _authenticationRepository.GetByUserId(user.Id);

            if (lastAuthentication != null)
            {
                await _baseRepositoryAuthentication.Remove(lastAuthentication);
            }

            await _baseRepositoryAuthentication.Add(authentication);


            var loginResponseModel = user.TypeUser == TypeUserEnum.Teacher ? new LoginResponseModel
            {
                Token = sessionToken,
                Id = user.Id,
                Name = user.Name,
                UserType = user.TypeUser,
                AvailableEvaluation = user.Teacher.AvailableEvaluation,
                AvailableOrientation = user.Teacher.AvailableOrientation
            } : new LoginResponseModel
            {
                Token = sessionToken,
                Id = user.Id,
                Name = user.Name,
                UserType = user.TypeUser
            };

            return new ServiceResponse<LoginResponseModel?>
            {
                Data = loginResponseModel,
                Status = 200,
                Message = "Login efetuado!"
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
