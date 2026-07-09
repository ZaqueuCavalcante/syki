namespace Estud.Tests.Domain;

public class EnumUniqueValuesUnitTests
{
    [Test]
    public void EnumUniqueValues_Should_have_unique_integer_values_for_all_enums()
    {
        var duplicates = new List<string>();

        var enumTypes = typeof(CommandStatus).Assembly.GetTypes().Where(t => t.IsEnum);

        foreach (var enumType in enumTypes)
        {
            var values = Enum.GetValues(enumType)
                .Cast<object>()
                .Select(v => new { Name = v.ToString()!, Value = Convert.ToInt64(v) })
                .ToList();

            var grouped = values
                .GroupBy(v => v.Value)
                .Where(g => g.Count() > 1);

            foreach (var group in grouped)
            {
                var names = string.Join(", ", group.Select(g => g.Name));
                duplicates.Add($"{enumType.Name}: [{names}] = {group.Key}");
            }
        }

        duplicates.Should().BeEmpty("duplicated enum items:\n" + string.Join("\n", duplicates));
    }
}
