namespace Syki.Tests.Base;

public class BasicInstitutionTestDto
{
    public AcademicPeriodOut AcademicPeriod1 { get; set; }
    public AcademicPeriodOut AcademicPeriod2 { get; set; }
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
    public DisciplineOut ComputationalThinkingAndAlgorithms { get; set; }
    public DisciplineOut IntegratorProjectOne { get; set; }
    
    public DisciplineOut Arch { get; set; }
    public DisciplineOut Databases { get; set; }
    public DisciplineOut DataStructures { get; set; }
    public DisciplineOut InfoAndSociety { get; set; }
    public DisciplineOut Poo { get; set; }
    public DisciplineOut IntegratorProjectTwo { get; set; }
}
