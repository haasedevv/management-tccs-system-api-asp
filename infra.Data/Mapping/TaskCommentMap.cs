using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Mapping;

public class TaskCommentMap : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> builder)
    {
        builder.HasKey(prop => prop.Id);

        builder.HasOne(tc => tc.ProjectTask)
            .WithMany(pt => pt.TaskComments)
            .HasForeignKey(tc => tc.TaskId)
            .IsRequired();

        builder.HasOne(tc => tc.User)
            .WithMany(u => u.TaskComments)
            .HasForeignKey(tc => tc.UserId)
            .IsRequired();

        builder.Property(prop => prop.Comment)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(prop => prop.Created)
            .IsRequired();
    }
}
