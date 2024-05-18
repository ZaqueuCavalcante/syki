namespace Syki.Tests.Unit;

public class ListExtensionsUnitTests
{
    [Test]
    public void Shoud_return_true_when_empty_list()
    {
        // Arrange
        var guids = new List<Guid>();

        var result = guids.IsSubsetOf(_guids);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Shoud_return_true_when_one_item_list()
    {
        // Arrange
        List<Guid> guids = 
        [
            Guid.Parse("0e88b426-f78b-42f2-a7b7-3bc1d8508ef1"),
        ];

        var result = guids.IsSubsetOf(_guids);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Shoud_return_true_when_two_item_list()
    {
        // Arrange
        List<Guid> guids = 
        [
            Guid.Parse("0e88b426-f78b-42f2-a7b7-3bc1d8508ef1"),
            Guid.Parse("e2e833ce-9eee-4755-96be-66c52d7dc260"),
        ];

        var result = guids.IsSubsetOf(_guids);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Shoud_return_true_when_all_items_list()
    {
        // Arrange
        List<Guid> guids = 
        [
            Guid.Parse("bab5f379-ac8b-446d-9325-13d18cd42227"),
            Guid.Parse("e2e833ce-9eee-4755-96be-66c52d7dc260"),
            Guid.Parse("17a84760-c56b-4af0-8701-d6ba9e11495e"),
            Guid.Parse("0e88b426-f78b-42f2-a7b7-3bc1d8508ef1"),
            Guid.Parse("439f1f9d-5be0-4456-8364-a2a2391953bb"),
        ];

        var result = guids.IsSubsetOf(_guids);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    [Repeat(100)]
    public void Shoud_return_false_when_random_guid()
    {
        // Arrange
        List<Guid> guids = 
        [
            Guid.NewGuid(),
        ];

        var result = guids.IsSubsetOf(_guids);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Shoud_return_false_when_duplicated()
    {
        // Arrange
        List<Guid> guids = 
        [
            Guid.Parse("e2e833ce-9eee-4755-96be-66c52d7dc260"),
            Guid.Parse("e2e833ce-9eee-4755-96be-66c52d7dc260"),
        ];

        var result = guids.IsSubsetOf(_guids);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Shoud_return_false_when_triplicated()
    {
        // Arrange
        List<Guid> guids = 
        [
            Guid.Parse("e2e833ce-9eee-4755-96be-66c52d7dc260"),
            Guid.Parse("e2e833ce-9eee-4755-96be-66c52d7dc260"),
            Guid.Parse("e2e833ce-9eee-4755-96be-66c52d7dc260"),
        ];

        var result = guids.IsSubsetOf(_guids);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [Repeat(100)]
    public void Shoud_return_false_when_has_one_out()
    {
        // Arrange
        List<Guid> guids = 
        [
            Guid.NewGuid(),
            Guid.Parse("17a84760-c56b-4af0-8701-d6ba9e11495e"),
            Guid.Parse("0e88b426-f78b-42f2-a7b7-3bc1d8508ef1"),
        ];

        var result = guids.IsSubsetOf(_guids);

        // Assert
        result.Should().BeFalse();
    }

    private static readonly List<Guid> _guids =
    [
        Guid.Parse("e2e833ce-9eee-4755-96be-66c52d7dc260"),
        Guid.Parse("bab5f379-ac8b-446d-9325-13d18cd42227"),
        Guid.Parse("439f1f9d-5be0-4456-8364-a2a2391953bb"),
        Guid.Parse("0e88b426-f78b-42f2-a7b7-3bc1d8508ef1"),
        Guid.Parse("17a84760-c56b-4af0-8701-d6ba9e11495e"),
    ];
}
