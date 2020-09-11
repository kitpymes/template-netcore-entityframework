using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kitpymes.Core.EntityFramework.Tests
{
    public sealed class FakeEntityConfiguration : IEntityTypeConfiguration<FakeEntity>
    {
        public void Configure(EntityTypeBuilder<FakeEntity> builder)
        {
            builder.ToTable(nameof(FakeEntity));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(100);
            builder.OwnsOne(x => x.FakeValueObject);
            builder.HasMany(x => x.FakeEntityChild).WithOne(x => x.FakeEntity).HasForeignKey(x => x.FakeEntityId);
        }
    }
}
