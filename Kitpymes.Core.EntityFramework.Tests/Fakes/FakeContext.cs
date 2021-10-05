using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Kitpymes.Core.EntityFramework.Tests
{
    public class FakeContext : EntityFrameworkDbContext
    {
        public FakeContext(DbContextOptions<FakeContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.WithEntitiesConfigurations(Assembly.GetExecutingAssembly());
        }
    }
}
