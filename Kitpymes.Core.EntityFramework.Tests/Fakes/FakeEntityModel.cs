using System.Collections.Generic;

namespace Kitpymes.Core.EntityFramework.Tests
{
    public class FakeEntityModel
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public FakeValueObjectModel FakeValueObject { get; set; } = new FakeValueObjectModel();

        public ICollection<FakeEntityChild> FakeEntityChild { get; set; } = new HashSet<FakeEntityChild>();
    }
}
