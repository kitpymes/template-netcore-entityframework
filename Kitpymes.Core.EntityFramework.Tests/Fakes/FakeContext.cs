using Microsoft.EntityFrameworkCore;

namespace Kitpymes.Core.EntityFramework.Tests
{
    public class FakeContext : EntityFrameworkContext
    {
        public FakeContext(DbContextOptions<FakeContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FakeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FakeEntityChildConfiguration());
        }
    }
}
