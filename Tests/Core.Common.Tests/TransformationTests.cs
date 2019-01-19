using System;
using Xunit;
using Core.Common;

namespace Core.Common.Tests
{
    public class TransformationTests
    {
        [Theory]
        [InlineData("Account Name Key", "accountnamekey")]
        [InlineData("@ccount Name Key&%$@!$)_-=_+(*&%&^)$#@", "ccountnamekey")]
        [InlineData("123-Account-Name-Key", "123accountnamekey")]
        public void Transform_NameToKey(string input, string expected)
        {
            // 1. Arrange

            // 2. Act
            var actual = Common.Transformations.NameKey.Transform(input);

            // 3. Assert
            Assert.Equal(expected, actual);
        }
    }
}
