using NUnit.Framework;
using OtomatikMuhendis.TRIdentity.Api;

namespace OtomatikMuhendis.TRIdentity.Tests
{
    public class IdentityTests
    {
        private Identity _sut;

        [Theory]
        [TestCase("39212903144", true)]
        [TestCase("51077628376", true)]
        [TestCase("12characters", false)]
        [TestCase("1", false)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("a", false)]
        [TestCase(" ", false)]
        public void IsNumberValid_VariousValuesProvided_EqualToExpectedResult(string identityNumber, bool expected)
        {
            // Arrange
            _sut = new Identity(identityNumber);

            // Act
            var actual = _sut.IsNumberValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}