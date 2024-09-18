namespace Syki.Tests.Base;

public class BasicInstitutionTestDto
{
    public AcademicPeriodOut AcademicPeriod1 { get; set; }
    public AcademicPeriodOut AcademicPeriod2 { get; set; }

    public CampusOut Campus { get; set; }

    public CourseOut AdsCourse { get; set; }
    public BasicInstitutionTestAdsDisciplinesDto AdsDisciplines { get; set; } = new();
    public CourseCurriculumOut AdsCourseCurriculum { get; set; }
    public CourseOfferingOut AdsCourseOffering { get; set; }

    public CourseOut DireitoCourse { get; set; }
    public BasicInstitutionTestDireitoDisciplinesDto DireitoDisciplines { get; set; } = new();
    public CourseCurriculumOut DireitoCourseCurriculum { get; set; }
    public CourseOfferingOut DireitoCourseOffering { get; set; }
}

public class BasicInstitutionTestAdsDisciplinesDto
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

public class BasicInstitutionTestDireitoDisciplinesDto
{
    public DisciplineOut PhilosophicalBases { get; set; }
    public DisciplineOut CommunicationAndLegalArgumentation { get; set; }
    public DisciplineOut ManSocietyAndLaw { get; set; }
    public DisciplineOut PoliticsAndStateInFocus { get; set; }
    public DisciplineOut GeneralTheoryOfLaw { get; set; }
}
