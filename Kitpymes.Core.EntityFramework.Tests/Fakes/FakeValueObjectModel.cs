namespace Kitpymes.Core.EntityFramework.Tests
{
    public class FakeValueObjectModel
    {
        public string? Property1 { get; private set; }
        public string? Property2 { get; private set; }

        public FakeValueObjectModel()
        {
        }

        public FakeValueObjectModel(string property1, string property2)
        {
            Property1 = property1;
            Property2 = property2;
        }
    }
}
