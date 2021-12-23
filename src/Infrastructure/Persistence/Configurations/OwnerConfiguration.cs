using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    internal sealed class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.ToTable("Owners");
            builder.HasKey(owner => owner.Id);
            builder.Property(owner => owner.Id).ValueGeneratedOnAdd();
            builder.Property(owner => owner.Name).IsRequired().HasMaxLength(60);
            builder.Property(owner => owner.Address).IsRequired().HasMaxLength(100);
            builder.Property(owner => owner.DateOfBirth).IsRequired();
            builder.HasMany(owner => owner.Accounts)
                .WithOne()
                .HasForeignKey(account => account.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
