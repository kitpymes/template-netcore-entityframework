using Kitpymes.Core.Repositories;

namespace Kitpymes.Core.EntityFramework.Tests
{
    public interface IFakeRepository : IRelationalRepository<FakeEntity>
    {
    }
}
