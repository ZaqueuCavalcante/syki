using Syki.Back.Features.Academic.CreateCourseOffering;

namespace Syki.Tests.Features.Academic.CreateCourseOffering;

public class GetCourseOfferingCurrentPeriodNumberUnitTests
{
    [Test]
    [TestCaseSource(nameof(GetCurrentPeriodNumberData))]
    public void Should_get_course_offering_current_period_number(string current, string start, byte number)
    {
        // Arrange
        var courseOffering = new CourseOffering() { Period = start };

        // Act
        var periodNumber = courseOffering.GetCurrentPeriodNumber(current);

        // Assert
        periodNumber.Should().Be(number);
    }

    static IEnumerable<object[]> GetCurrentPeriodNumberData()
    {
        foreach (var (current, start, number) in new List<(string, string, byte)>()
        {
            ("2026.1", "2026.1", 1),
            ("2026.1", "2025.2", 2),
            ("2026.1", "2025.1", 3),
            ("2026.1", "2024.2", 4),
            ("2026.1", "2024.1", 5),
            ("2026.1", "2023.2", 6),
            ("2026.1", "2023.1", 7),
            ("2026.1", "2022.2", 8),
            ("2026.1", "2022.1", 9),
            ("2026.1", "2021.2", 10),

            ("2025.2", "2025.2", 1),
            ("2025.2", "2025.1", 2),
            ("2025.2", "2024.2", 3),
            ("2025.2", "2024.1", 4),
            ("2025.2", "2023.2", 5),
            ("2025.2", "2023.1", 6),
            ("2025.2", "2022.2", 7),
            ("2025.2", "2022.1", 8),
            ("2025.2", "2021.2", 9),
            ("2025.2", "2021.1", 10),

            ("2025.1", "2025.1", 1),
            ("2025.1", "2024.2", 2),
            ("2025.1", "2024.1", 3),
            ("2025.1", "2023.2", 4),
            ("2025.1", "2023.1", 5),
            ("2025.1", "2022.2", 6),
            ("2025.1", "2022.1", 7),
            ("2025.1", "2021.2", 8),
            ("2025.1", "2021.1", 9),
            ("2025.1", "2020.2", 10),

            ("2024.2", "2024.2", 1),
            ("2024.2", "2024.1", 2),
            ("2024.2", "2023.2", 3),
            ("2024.2", "2023.1", 4),
            ("2024.2", "2022.2", 5),
            ("2024.2", "2022.1", 6),
            ("2024.2", "2021.2", 7),
            ("2024.2", "2021.1", 8),
            ("2024.2", "2020.2", 9),
            ("2024.2", "2020.1", 10),

            ("2024.1", "2024.1", 1),
            ("2024.1", "2023.2", 2),
            ("2024.1", "2023.1", 3),
            ("2024.1", "2022.2", 4),
            ("2024.1", "2022.1", 5),
            ("2024.1", "2021.2", 6),
            ("2024.1", "2021.1", 7),
            ("2024.1", "2020.2", 8),
            ("2024.1", "2020.1", 9),
            ("2024.1", "2019.2", 10),
        })
        {
            yield return [current, start, number];
        }
    }
}
