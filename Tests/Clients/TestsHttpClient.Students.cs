using System.Net.Http.Json;
using Estud.Back.Features.Students.GetStudent;
using Estud.Back.Features.Students.GetStudents;
using Estud.Back.Features.Students.CreateStudent;
using Estud.Back.Features.Students.GetStudentClass;
using Estud.Back.Features.Students.GetStudentDetails;
using Estud.Back.Features.Students.AssignStudentToClass;
using Estud.Back.Features.Students.CreateClassActivityWork;
using Estud.Back.Features.Students.GetStudentClassActivity;
using Estud.Back.Features.Students.GetStudentClassActivities;
using Estud.Back.Features.Students.EnrollStudentInCourseOffering;

namespace Estud.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateStudentOut, ErrorOut>> CreateStudent(
        string name,
        string email,
        string? phoneNumber = null,
        DateOnly? birthdate = null
    ) {
        var data = new CreateStudentIn { Name = name, Email = email, PhoneNumber = phoneNumber, Birthdate = birthdate };
        var response = await http.PostAsJsonAsync("/students", data);
        return await response.Resolve<CreateStudentOut>();
    }

    public async Task<OneOf<GetStudentsOut, ErrorOut>> GetStudents(
        string? filter = null,
        int? page = null,
        int? pageSize = null
    ) {
        var data = new GetStudentsIn
        {
            Filter = filter,
            Page = page ?? 1,
            PageSize = pageSize ?? 10,
        };

        var response = await http.GetAsync("/students".AddQueryString(data));
        return await response.Resolve<GetStudentsOut>();
    }

    public async Task<OneOf<GetStudentOut, ErrorOut>> GetStudent(int id)
    {
        var response = await http.GetAsync($"/students/{id}");
        return await response.Resolve<GetStudentOut>();
    }

    public async Task<OneOf<GetStudentDetailsOut, ErrorOut>> GetStudentDetails(int id)
    {
        var response = await http.GetAsync($"/students/{id}/details");
        return await response.Resolve<GetStudentDetailsOut>();
    }

    public async Task<OneOf<GetStudentClassOut, ErrorOut>> GetStudentClass(int id)
    {
        var response = await http.GetAsync($"/students/classes/{id}");
        return await response.Resolve<GetStudentClassOut>();
    }

    public async Task<OneOf<GetStudentClassActivitiesOut, ErrorOut>> GetStudentClassActivities(int classId)
    {
        var response = await http.GetAsync($"/students/classes/{classId}/activities");
        return await response.Resolve<GetStudentClassActivitiesOut>();
    }

    public async Task<OneOf<GetStudentClassActivityOut, ErrorOut>> GetStudentClassActivity(int classId, int activityId)
    {
        var response = await http.GetAsync($"/students/classes/{classId}/activities/{activityId}");
        return await response.Resolve<GetStudentClassActivityOut>();
    }

    public async Task<OneOf<CreateClassActivityWorkOut, ErrorOut>> CreateClassActivityWork(
        int activityId,
        string? link = "https://github.com/ZaqueuCavalcante/estud"
    ) {
        var data = new CreateClassActivityWorkIn { Link = link };
        var response = await http.PostAsJsonAsync($"/students/activities/{activityId}/works", data);
        return await response.Resolve<CreateClassActivityWorkOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> AssignStudentToClass(int studentId, int classId)
    {
        var data = new AssignStudentToClassIn { ClassId = classId };
        var response = await http.PostAsJsonAsync($"/students/{studentId}/classes", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<EnrollStudentInCourseOfferingOut, ErrorOut>> EnrollStudentInCourseOffering(int studentId, int courseOfferingId)
    {
        var data = new EnrollStudentInCourseOfferingIn { CourseOfferingId = courseOfferingId };
        var response = await http.PostAsJsonAsync($"/students/{studentId}/course-offerings", data);
        return await response.Resolve<EnrollStudentInCourseOfferingOut>();
    }
}
