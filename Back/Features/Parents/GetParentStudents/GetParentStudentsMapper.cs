using Estud.Back.Domain.Parents;

namespace Estud.Back.Features.Parents.GetParentStudents;

public static class GetParentStudentsMapper
{
    extension(ParentStudent link)
    {
        public GetParentStudentsItemOut ToGetParentStudentsItemOut()
        {
            return new()
            {
                Id = link.StudentId,
                Name = link.Student!.Name,
                EnrollmentCode = link.Student!.EnrollmentCode,
                Relationship = link.Relationship,
            };
        }
    }
}
