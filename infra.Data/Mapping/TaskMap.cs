using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Mapping;

public class TaskMap: IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.HasKey(prop => prop.Id);
        builder
            .HasOne(prop => prop.Project)
            .WithMany(prop => prop.Tasks)
            .HasForeignKey(prop => prop.ProjectId);
        builder.Property(prop => prop.Title).IsRequired().HasMaxLength(100);
        builder.Property(prop => prop.Description).IsRequired().HasMaxLength(255);
        builder.Property(prop => prop.Completed).IsRequired();
        builder.Property(prop => prop.Deadline).IsRequired();
    }
}
