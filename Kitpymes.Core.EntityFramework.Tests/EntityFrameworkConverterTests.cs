using Kitpymes.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kitpymes.Core.EntityFramework.Tests
{
    [TestClass]
    public class EntityFrameworkConverterTests
    {
        [TestMethod]
        public void EnumConvertName()
        {
            var expectedEnum = GenderTypeEnum.Male;
            var expectedEnumName = GenderTypeEnum.Male.Name;

            var convert = EntityFrameworkConverter.ToEnumName<GenderTypeEnum, int>();

            var actualEnum = convert.ConvertFromProvider(expectedEnumName);
            var actualEnumName = convert.ConvertToProvider(expectedEnum);

            Assert.AreEqual(expectedEnum, actualEnum);
            Assert.AreEqual(expectedEnumName, actualEnumName);
        }

        [TestMethod]
        public void EnumConvertValue()
        {
            var expectedEnum = GenderTypeEnum.Male;
            var expectedEnumValue = GenderTypeEnum.Male.Value;

            var convert = EntityFrameworkConverter.ToEnumValue<GenderTypeEnum, int>();

            var actualEnum = convert.ConvertFromProvider(expectedEnumValue);
            var actualEnumValue = convert.ConvertToProvider(expectedEnum);

            Assert.AreEqual(expectedEnum, actualEnum);
            Assert.AreEqual(expectedEnumValue, actualEnumValue);
        }

        [TestMethod]
        public void StatusActiveConvert()
        {
            var expectedEnum = StatusEnum.Active;
            var expectedActive = StatusEnum.Active.ToString();

            var convert = EntityFrameworkConverter.ToStatus();

            var actualEnum = convert.ConvertFromProvider(expectedActive);
            var actualActive = convert.ConvertToProvider(expectedEnum);

            Assert.AreEqual(expectedEnum, actualEnum);
            Assert.AreEqual(expectedActive, actualActive);
        }

        [TestMethod]
        public void StatusInactiveConvert()
        {
            var expectedEnum = StatusEnum.Inactive;
            var expectedInactive = StatusEnum.Inactive.ToString();

            var convert = EntityFrameworkConverter.ToStatus();

            var actualEnum = convert.ConvertFromProvider(expectedInactive);
            var actualInactive = convert.ConvertToProvider(expectedEnum);

            Assert.AreEqual(expectedEnum, actualEnum);
            Assert.AreEqual(expectedInactive, actualInactive);
        }

        [TestMethod]
        public void EmailConvert()
        {
            var value = "email@email.com";

            var expectedEnum = Email.Create(value);
            var expectedString = value;

            var convert = EntityFrameworkConverter.ToEmail<Email>();

            var actualEnum = convert.ConvertFromProvider(expectedString);
            var actualString = convert.ConvertToProvider(expectedEnum);

            Assert.AreEqual(expectedEnum.ToString(), actualEnum.ToString());
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void NameConvert()
        {
            var value = "Juan pedro";

            var expectedEnum = Name.Create(value);
            var expectedString = value;

            var convert = EntityFrameworkConverter.ToName<Name>();

            var actualEnum = convert.ConvertFromProvider(expectedString);
            var actualString = convert.ConvertToProvider(expectedEnum);

            Assert.AreEqual(expectedEnum.ToString(), actualEnum.ToString());
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void SubdomainConvert()
        {
            var value = "documents"; // Ej. documents.kitpymes.com

            var expectedEnum = Subdomain.Create(value);
            var expectedString = value;

            var convert = EntityFrameworkConverter.ToSubdomain<Subdomain>();

            var actualEnum = convert.ConvertFromProvider(expectedString);
            var actualString = convert.ConvertToProvider(expectedEnum);

            Assert.AreEqual(expectedEnum.ToString(), actualEnum.ToString());
            Assert.AreEqual(expectedString, actualString);
        }
    }
}