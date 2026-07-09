using Estud.Back.Domain.Students;

namespace Estud.Back.Features.Students.GetStudents;

public static class GetStudentsMapper
{
    extension(EstudStudent student)
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
