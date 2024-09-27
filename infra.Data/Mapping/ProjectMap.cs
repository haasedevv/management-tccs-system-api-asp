using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Mapping;

public class ProjectMap : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(prop => prop.Id);
        builder.Property(prop => prop.Name).IsRequired().HasMaxLength(120);
        builder.Property(prop => prop.Description).IsRequired().HasMaxLength(255);
        builder.Property(prop => prop.Status).IsRequired();
        builder.Property(prop => prop.Grade);
        builder.Property(prop => prop.Created).IsRequired();
        builder.Property(prop => prop.Updated);
        builder.HasMany(prop => prop.Tasks)
            .WithOne(prop => prop.Project)
            .HasForeignKey(prop => prop.ProjectId)
            .IsRequired();

        builder.HasMany(prop => prop.Requests)
            .WithOne(prop => prop.Project)
            .HasForeignKey(prop => prop.ProjectId)
            .IsRequired();
    }
}
