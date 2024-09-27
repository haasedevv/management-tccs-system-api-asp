using domain.Entities;

namespace domain.Interfaces.Services;

public interface ITeacherService
{
    Task<ServiceResponse<string>> ChangeOrientationAvailable(int userId, bool status);
    Task<ServiceResponse<string>> ChangeEvaluationAvailable(int userId, bool status);
}
