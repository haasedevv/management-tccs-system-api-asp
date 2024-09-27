using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Mapping;

public class TeacherMap: IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasKey(prop => prop.Id);
        builder.Property(props => props.UserId).IsRequired();
        builder.Property(props => props.Area).IsRequired();
        builder.Property(props => props.AvailableEvaluation);
        builder.Property(props => props.AvailableOrientation);
        builder
            .HasOne(prop => prop.User)
            .WithOne(prop => prop.Teacher)
            .HasForeignKey<Teacher>(prop => prop.UserId);
    }
}
