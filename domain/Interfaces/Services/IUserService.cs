using domain.Entities;
using domain.Models.Request.User;

namespace domain.Interfaces.Services;

public interface IUserService
{
    Task<ServiceResponse<String>> AddUser(CreateUserModel body);
    Task<ServiceResponse<string>> ChangeUserAvailable(int id, ChangeUserAvailableModel body);
    Task<ServiceResponse<IEnumerable<User>>> GetProjectStudents(int projectId);
}
