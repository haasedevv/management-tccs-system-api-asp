using domain.Entities;
using domain.Interfaces.Services;
using infra.Data.Repository;

namespace service.Services;

public class TeacherService: ITeacherService
{
    private readonly BaseRepository<Teacher> _teacherRepositoryBase;

    public TeacherService(BaseRepository<Teacher> teacherRepositoryBase)
    {
        _teacherRepositoryBase = teacherRepositoryBase;
    }

    public async Task<ServiceResponse<string>> ChangeOrientationAvailable(int userId, bool status)
    {
        try
        {
            var serviceResponse = new ServiceResponse<string>();
            var teacher = (await _teacherRepositoryBase.FindBy(x => x.UserId == userId)).FirstOrDefault();

            if (teacher == null)
            {
                serviceResponse.Status = 404;
                serviceResponse.Message = "Professor não encontrado!";

                return serviceResponse;
            }

            teacher.AvailableOrientation = status;
            await _teacherRepositoryBase.Update(teacher);

            serviceResponse.Status = 200;
            serviceResponse.Message = "Status de orientação alterado!";

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<string>> ChangeEvaluationAvailable(int userId, bool status)
    {
        try
        {
            var serviceResponse = new ServiceResponse<string>();
            var teacher = (await _teacherRepositoryBase.FindBy(x => x.UserId == userId)).FirstOrDefault();

            if (teacher == null)
            {
                serviceResponse.Status = 404;
                serviceResponse.Message = "Professor não encontrado!";

                return serviceResponse;
            }

            teacher.AvailableEvaluation = status;
            await _teacherRepositoryBase.Update(teacher);

            serviceResponse.Status = 200;
            serviceResponse.Message = "Status de avaliação alterado!";

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
