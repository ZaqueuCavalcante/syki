using Syki.Back.Events;

namespace Syki.Tests.Events;

public class DomainEventsUnitTests
{
    [Test]
    public void All_domain_events_should_have_description_attribute()
    {
        // Arrange
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName!.StartsWith("Back"))
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IDomainEvent).IsAssignableFrom(p) && !p.IsInterface)
            .ToList();

        // Act / Assert
        foreach (var type in types)
        {
            var attributes = (DomainEventAttribute[])type.GetCustomAttributes(typeof(DomainEventAttribute), true);
            attributes.Should().ContainSingle();
            attributes[0].Description.Should().NotBeNullOrEmpty();
        }
    }
}
