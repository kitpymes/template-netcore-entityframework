namespace Kitpymes.Core.EntityFramework.Tests
{
    public class FakeEntityChild
    {
        public long Id { get; set; }
        public long FakeEntityId { get; set; }
        public FakeEntity FakeEntity { get; set; } = new FakeEntity();
    }
}
