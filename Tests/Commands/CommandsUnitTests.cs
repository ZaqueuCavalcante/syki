namespace Syki.Tests.Commands;

public class CommandsUnitTests
{
    [Test]
    public void All_commands_should_have_description_attribute()
    {
        // Arrange
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName!.StartsWith("Back"))
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(ICommand).IsAssignableFrom(p) && !p.IsInterface)
            .ToList();

        // Act / Assert
        foreach (var type in types)
        {
            var attributes = (CommandDescriptionAttribute[])type.GetCustomAttributes(typeof(CommandDescriptionAttribute), true);
            attributes.Should().ContainSingle();
            attributes[0].Description.Should().NotBeNullOrEmpty();
        }
    }
}
