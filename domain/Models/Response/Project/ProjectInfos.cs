using domain.Enums;

namespace domain.Models.Response.Project;

public class ProjectInfos
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ProjectStatus Status { get; set; }
    public IEnumerable<UserInfos> Students { get; set; }
    public IEnumerable<UserInfos> teacherAdvisor { get; set; }
    public IEnumerable<UserInfos> teacherEvaluators { get; set; }
}
