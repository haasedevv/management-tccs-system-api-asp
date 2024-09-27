using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Mapping;

public class RequestMap: IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.HasKey(prop => prop.Id);
        builder.Property(prop => prop.Status).IsRequired();
        builder
            .HasOne(e => e.User)
            .WithMany(e => e.Requests)
            .HasForeignKey(e => e.UserId);
        builder
            .HasOne<Project>(e => e.Project)
            .WithMany(e => e.Requests)
            .HasForeignKey(e => e.ProjectId);
    }
}
