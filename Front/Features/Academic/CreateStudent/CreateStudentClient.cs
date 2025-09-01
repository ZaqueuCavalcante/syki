namespace Syki.Front.Features.Academic.CreateStudent;

public class CreateStudentClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<StudentOut, ErrorOut>> Create(string name, string email, Guid courseOfferingId, DateOnly? dateOfBirth = null, string? phoneNumber = null)
    {
        var data = new CreateStudentIn
        {
            Name = name,
            Email = email,
            CourseOfferingId = courseOfferingId,
            DateOfBirth = dateOfBirth,
            PhoneNumber = phoneNumber,
            
        };

        var response = await http.PostAsJsonAsync("/academic/students", data);

        return await response.Resolve<StudentOut>();
    }
}
