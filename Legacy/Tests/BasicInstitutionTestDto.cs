namespace Syki.Tests.Base;

public class BasicInstitutionTestDto
{
    public AcademicPeriodOut AcademicPeriod1 { get; set; }
    public AcademicPeriodOut AcademicPeriod2 { get; set; }

    public CreateCampusOut Campus { get; set; }

    public CreateCourseOut AdsCourse { get; set; }
    public BasicInstitutionTestAdsDisciplinesDto AdsDisciplines { get; set; } = new();
    public CourseCurriculumOut AdsCourseCurriculum { get; set; }
    public CourseOfferingOut AdsCourseOffering { get; set; }
    public BasicInstitutionTestAdsClassesDto AdsClasses { get; set; } = new();

    public CreateCourseOut DireitoCourse { get; set; }
    public BasicInstitutionTestDireitoDisciplinesDto DireitoDisciplines { get; set; } = new();
    public CourseCurriculumOut DireitoCourseCurriculum { get; set; }
    public CourseOfferingOut DireitoCourseOffering { get; set; }

    public TeacherOut Teacher { get; set; }
    public StudentOut Student { get; set; }
}

public class BasicInstitutionTestAdsDisciplinesDto
{
    public CreateCourseDisciplineOut HumanMachineInteractionDesign { get; set; }
    public CreateCourseDisciplineOut IntroToComputerNetworks { get; set; }
    public CreateCourseDisciplineOut IntroToWebDev { get; set; }
    public CreateCourseDisciplineOut DiscreteMath { get; set; }
    public CreateCourseDisciplineOut ComputationalThinkingAndAlgorithms { get; set; }
    public CreateCourseDisciplineOut IntegratorProjectOne { get; set; }

    public CreateCourseDisciplineOut Arch { get; set; }
    public CreateCourseDisciplineOut Databases { get; set; }
    public CreateCourseDisciplineOut DataStructures { get; set; }
    public CreateCourseDisciplineOut InfoAndSociety { get; set; }
    public CreateCourseDisciplineOut Poo { get; set; }
    public CreateCourseDisciplineOut IntegratorProjectTwo { get; set; }

    public List<Guid> GetIds()
    {
        return [.. GetType()
            .GetProperties()
            .Where(p => p.PropertyType == typeof(CreateCourseDisciplineOut))
            .Select(p => (CreateCourseDisciplineOut?)p.GetValue(this))
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
    public CreateCourseDisciplineOut PhilosophicalBases { get; set; }
    public CreateCourseDisciplineOut CommunicationAndLegalArgumentation { get; set; }
    public CreateCourseDisciplineOut ManSocietyAndLaw { get; set; }
    public CreateCourseDisciplineOut PoliticsAndStateInFocus { get; set; }
    public CreateCourseDisciplineOut GeneralTheoryOfLaw { get; set; }
}
