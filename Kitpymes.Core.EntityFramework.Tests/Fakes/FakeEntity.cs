using System.Collections.Generic;

namespace Kitpymes.Core.EntityFramework.Tests
{
    public class FakeEntity
    {
        public long Id { get; private set; }
        public string? Name { get; private set; }
        public string? Surname { get; private set; }
        public FakeValueObject FakeValueObject { get; private set; } = new FakeValueObject();
        public ICollection<FakeEntityChild> FakeEntityChild { get; private set; } = new List<FakeEntityChild>();

        public FakeEntity()
        {
        }

        public FakeEntity(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public FakeEntity(string name, string surname, FakeValueObject fakeValueObject)
        {
            Name = name;
            Surname = surname;
            FakeValueObject = fakeValueObject;
        }

        public FakeEntity(long id, string name, string surname, FakeValueObject fakeValueObject)
        {
            Id = id;
            Name = name;
            Surname = surname;
            FakeValueObject = fakeValueObject;
        }
    }
}
