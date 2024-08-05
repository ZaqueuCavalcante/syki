namespace Syki.Tests.Base;

public class BasicInstitutionTestDto
{
    public AcademicPeriodOut AcademicPeriod { get; set; }
    public CampusOut Campus { get; set; }
    public CourseOut Course { get; set; }
    public BasicInstitutionTestDisciplinesDto Disciplines { get; set; } = new();
    public CourseCurriculumOut CourseCurriculum { get; set; }
    public CourseOfferingOut CourseOffering { get; set; }
}

public class BasicInstitutionTestDisciplinesDto
{
    public DisciplineOut DiscreteMath { get; set; }
    public DisciplineOut IntroToWebDev { get; set; }
    public DisciplineOut HumanMachineInteractionDesign { get; set; }
    public DisciplineOut IntroToComputerNetworks { get; set; }
    public DisciplineOut CumputationalThinkingAndAlgorithms { get; set; }
    public DisciplineOut IntegratorProjectOne { get; set; }
}
