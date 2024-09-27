using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Mapping;

public class StudentMap: IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(prop => prop.Id);
        builder.Property(props => props.UserId).IsRequired();
        builder.Property(props => props.Available).IsRequired();
        builder
            .HasOne(prop => prop.User)
            .WithOne(prop => prop.Student)
            .HasForeignKey<Student>(prop => prop.UserId);
    }
}
