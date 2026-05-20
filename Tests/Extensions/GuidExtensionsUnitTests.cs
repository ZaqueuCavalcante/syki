namespace Syki.Tests.Extensions;

public class GuidExtensionsUnitTests
{
    [Test]
    [TestCaseSource(nameof(GuidsToHashCodes))]
    public void GuidExtensions_Should_convert_guid_to_hash_code(Guid guid, int hashCode)
    {
        // Arrange / Act
        var result = guid.ToHashCode();

        // Assert
        result.Should().Be(hashCode);
    }

    private static IEnumerable<object[]> GuidsToHashCodes()
    {
        foreach (var (guid, hashCode) in new List<(Guid, int)>()
        {
            (Guid.Parse("e2e833ce-9eee-4755-96be-66c52d7dc260"), 6652_7260),
            (Guid.Parse("bab5f379-ac8b-446d-9325-13d18cd42227"), 3184_2227),
            (Guid.Parse("439f1f9d-5be0-4456-8364-a2a2391953bb"), 2239_1953),
            (Guid.Parse("0197428e-be72-7d8a-94d3-6cf24a0e55f4"), 3624_0554)
        })
        {
            yield return [guid, hashCode];
        }
    }
}
