namespace Syki.Tests.Base;

public class BasicInstitutionTestDto
{
    public AcademicPeriodOut AcademicPeriod1 { get; set; }
    public AcademicPeriodOut AcademicPeriod2 { get; set; }

    public CreateCampusOut Campus { get; set; }

    public CourseOut AdsCourse { get; set; }
    public BasicInstitutionTestAdsDisciplinesDto AdsDisciplines { get; set; } = new();
    public CourseCurriculumOut AdsCourseCurriculum { get; set; }
    public CourseOfferingOut AdsCourseOffering { get; set; }
    public BasicInstitutionTestAdsClassesDto AdsClasses { get; set; } = new();

    public CourseOut DireitoCourse { get; set; }
    public BasicInstitutionTestDireitoDisciplinesDto DireitoDisciplines { get; set; } = new();
    public CourseCurriculumOut DireitoCourseCurriculum { get; set; }
    public CourseOfferingOut DireitoCourseOffering { get; set; }

    public TeacherOut Teacher { get; set; }
    public StudentOut Student { get; set; }
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

    public List<Guid> GetIds()
    {
        return [.. GetType()
            .GetProperties()
            .Where(p => p.PropertyType == typeof(DisciplineOut))
            .Select(p => (DisciplineOut?)p.GetValue(this))
            .Where(co => co != null)
            .Select(co => co!.Id)];
    }
}

public class BasicInstitutionTestAdsClassesDto
{
    public ClassOut DiscreteMath { get; set; }
    public ClassOut IntroToWebDev { get; set; }
    public ClassOut HumanMachineInteractionDesign { get; set; }
    public ClassOut IntroToComputerNetworks { get; set; }
    public ClassOut ComputationalThinkingAndAlgorithms { get; set; }
    public ClassOut IntegratorProjectOne { get; set; }

    public ClassOut Arch { get; set; }
    public ClassOut Databases { get; set; }
    public ClassOut DataStructures { get; set; }
    public ClassOut InfoAndSociety { get; set; }
    public ClassOut Poo { get; set; }
    public ClassOut IntegratorProjectTwo { get; set; }
}

public class BasicInstitutionTestDireitoDisciplinesDto
{
    public DisciplineOut PhilosophicalBases { get; set; }
    public DisciplineOut CommunicationAndLegalArgumentation { get; set; }
    public DisciplineOut ManSocietyAndLaw { get; set; }
    public DisciplineOut PoliticsAndStateInFocus { get; set; }
    public DisciplineOut GeneralTheoryOfLaw { get; set; }
}
