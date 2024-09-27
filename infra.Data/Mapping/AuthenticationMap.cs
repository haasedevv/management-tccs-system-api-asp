using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Mapping;

public class AuthenticationMap :IEntityTypeConfiguration<Authentication>
{
    public void Configure(EntityTypeBuilder<Authentication> builder)
    {
        builder.HasKey(prop => prop.Id);
        builder.HasOne<User>().WithOne(prop => prop.Authentication).HasForeignKey<Authentication>(prop => prop.UserId);
        builder.Property(prop => prop.Token).IsRequired();
        builder.Property(prop => prop.Created).IsRequired();
    }
}

