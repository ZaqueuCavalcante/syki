namespace Syki.Tests.Tasks;

public class SykiTasksUnitTests
{
    [Test]
    public void All_domain_events_should_have_description_attribute()
    {
        // Arrange
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName!.StartsWith("Back"))
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(ISykiTask).IsAssignableFrom(p) && !p.IsInterface)
            .ToList();

        // Act / Assert
        foreach (var type in types)
        {
            var attributes = (SykiTaskDescriptionAttribute[])type.GetCustomAttributes(typeof(SykiTaskDescriptionAttribute), true);
            attributes.Should().ContainSingle();
            attributes[0].Description.Should().NotBeNullOrEmpty();
        }
    }
}
