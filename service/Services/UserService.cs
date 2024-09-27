using System.Linq.Expressions;
using domain.Entities;
using domain.Enums;
using domain.Interfaces.Services;
using domain.Models.Request.User;
using domain.Validations.Request;
using infra.Data.Repository;

namespace service.Services;

public class UserService : IUserService
{
    private readonly BaseRepository<User> _userBaseRepository;
    private readonly BaseRepository<Student> _studentBaseRepository;
    private readonly BaseRepository<Teacher> _teacherBaseRepository;
    private readonly UserRepository _userRepository;

    public UserService(BaseRepository<User> userBaseRepository, UserRepository userRepository,
        BaseRepository<Student> studentBaseRepository, BaseRepository<Teacher> teacherBaseRepository)
    {
        _userBaseRepository = userBaseRepository;
        _studentBaseRepository = studentBaseRepository;
        _teacherBaseRepository = teacherBaseRepository;
        _userRepository = userRepository;
    }

    public async Task<ServiceResponse<string>> AddUser(CreateUserModel userBody)
    {
        try
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            bool isValid = CreateUserValidation.ValidateCreateUserBody(userBody);

            if (!isValid)
            {
                serviceResponse.Message = "Body informado está incorreto!";
                serviceResponse.Status = 400;
                return serviceResponse;
            }

            var hashedPassword = PasswordHasher.GeneratePasswordHash(userBody.Password);

            User user = new User(userBody.Enrollment, hashedPassword);

            user.Enrollment = userBody.Enrollment;
            user.Name = userBody.Name;
            user.TypeUser = userBody.TypeUser;
            user.Email = userBody.Email;

            await _userBaseRepository.Add(user);

            if (user.TypeUser == TypeUserEnum.Student)
            {
                var Student = new Student
                {
                    UserId = user.Id,
                };

                await _studentBaseRepository.Add(Student);
            }

            if (user.TypeUser == TypeUserEnum.Teacher)
            {
                var Teacher = new Teacher()
                {
                    UserId = user.Id,
                    Area = AreaTypeEnum.Tecnologia
                };

                await _teacherBaseRepository.Add(Teacher);
            }

            serviceResponse.Message = "Usuário criado!";
            serviceResponse.Status = 200;
            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<string>> ChangeUserAvailable(int id, ChangeUserAvailableModel body)
    {
        try
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            var user = await _userBaseRepository.GetById(id);

            if (user == null)
            {
                serviceResponse.Status = 404;
                serviceResponse.Message = "Usuário não encontrado!";
                return serviceResponse;
            }

            user.Available = body.Status;
            await _userBaseRepository.Update(user);

            serviceResponse.Status = 200;
            serviceResponse.Message = "Disponibilidade atualizada!";
            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<IEnumerable<User>>> GetProjectStudents(int projectId)
    {
        try
        {
            var serviceResponse = new ServiceResponse<IEnumerable<User>>();
            var students = await _userRepository.GetStudentsGroup(projectId);

            serviceResponse.Data = students;
            serviceResponse.Message = "OK";
            serviceResponse.Status = 200;

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
