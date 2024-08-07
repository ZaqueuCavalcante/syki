namespace Syki.Tests.Extensions;

public class GuidExtensionsUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.GuidsToHashCodes))]
    public void Should_convert_guid_to_hash_code((Guid guid, int hashCode) data)
    {
        // Arrange / Act
        var result = data.guid.ToHashCode();

        // Assert
        result.Should().Be(data.hashCode);
    }
}
