namespace Kitpymes.Core.EntityFramework.Tests
{
    public sealed class FakeRepository : EntityFrameworkRepository<FakeEntity>, IFakeRepository
    {
        public FakeRepository(FakeContext context) : base(context) { }
    }
}
