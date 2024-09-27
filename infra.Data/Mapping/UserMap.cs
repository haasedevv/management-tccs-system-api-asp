using Microsoft.EntityFrameworkCore;
using domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Mapping;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(prop => prop.Id);
        builder.HasAlternateKey(prop => prop.Enrollment).HasName("Enrollment");
        builder.Property(prop => prop.Name).IsRequired().HasMaxLength(100);
        builder.Property(prop => prop.Email).IsRequired().HasMaxLength(255);
        builder.Property(prop => prop.Login).IsRequired().HasMaxLength(100);
        builder.Property(prop => prop.Password).IsRequired();
        builder.Property(prop => prop.TypeUser).IsRequired();
        builder.Property(prop => prop.Available).IsRequired();
        builder.HasOne(prop => prop.Authentication).WithOne().HasForeignKey<Authentication>(prop => prop.UserId);
        builder
            .HasMany(e => e.Requests)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);
    }
}
