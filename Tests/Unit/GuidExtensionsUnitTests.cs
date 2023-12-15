using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class GuidExtensionsUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.GuidsToHashCodes))]
    public void Shoud_convert_guid_to_hash_code((Guid guid, int hashCode) data)
    {
        // Arrange / Act
        var result = data.guid.ToHashCode();

        // Assert
        result.Should().Be(data.hashCode);
    }
}
