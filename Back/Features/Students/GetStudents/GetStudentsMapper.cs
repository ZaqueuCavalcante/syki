using Syki.Back.Domain.Students;

namespace Syki.Back.Features.Students.GetStudents;

public static class GetStudentsMapper
{
    extension(SykiStudent student)
    {
        public GetStudentsItemOut ToGetStudentsItemOut()
        {
            return new()
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.User!.Email!,
                EnrollmentCode = student.EnrollmentCode,
                Status = student.Status,
            };
        }
    }
}
