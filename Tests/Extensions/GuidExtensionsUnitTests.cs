namespace Syki.Tests.Extensions;

public class GuidExtensionsUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.GuidsToHashCodes))]
    public void Should_convert_guid_to_hash_code(Guid guid, int hashCode)
    {
        // Arrange / Act
        var result = guid.ToHashCode();

        // Assert
        result.Should().Be(hashCode);
    }
}
