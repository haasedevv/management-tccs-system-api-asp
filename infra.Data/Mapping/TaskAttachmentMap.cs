using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Mapping;

public class TaskAttachmentMap: IEntityTypeConfiguration<TaskAttachment>
{
    public void Configure(EntityTypeBuilder<TaskAttachment> builder)
    {
        builder.HasKey(prop => prop.Id);


        builder.HasOne(ta => ta.ProjectTask)
            .WithMany(pt => pt.TaskAttachments)
            .HasForeignKey(ta => ta.ProjectTaskId)
            .IsRequired();


        builder.HasOne(ta => ta.User)
            .WithMany(u => u.TaskAttachments)
            .HasForeignKey(ta => ta.UserId)
            .IsRequired();


        builder.Property(prop => prop.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(prop => prop.Attachment)
            .IsRequired()
            .HasMaxLength(Int32.MaxValue);

        builder.Property(prop => prop.Created)
            .IsRequired();
    }
}
