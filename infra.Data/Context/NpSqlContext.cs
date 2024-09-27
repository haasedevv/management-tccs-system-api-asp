using domain.Entities;
using infra.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace infra.Data.Context;

public class NpSqlContext: DbContext
{
    public NpSqlContext() {}

    public NpSqlContext(DbContextOptions<NpSqlContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuthenticationMap());
        modelBuilder.ApplyConfiguration(new RequestMap());
        modelBuilder.ApplyConfiguration(new TaskAttachmentMap());
        modelBuilder.ApplyConfiguration(new TaskCommentMap());
        modelBuilder.ApplyConfiguration(new TaskMap());
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new ProjectMap());
        modelBuilder.ApplyConfiguration(new TeacherMap());
        modelBuilder.ApplyConfiguration(new StudentMap());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Authentication> Authentications { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<ProjectTask> Task { get; set; }
    public DbSet<TaskAttachment> TaskAttachments { get; set; }
    public DbSet<TaskComment> TaskComments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Student> Students { get; set; }
}
