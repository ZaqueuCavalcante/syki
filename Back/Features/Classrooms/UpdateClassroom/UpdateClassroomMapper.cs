using Estud.Back.Domain.Classrooms;

namespace Estud.Back.Features.Classrooms.UpdateClassroom;

public static class UpdateClassroomMapper
{
    extension(Classroom classroom)
    {
        public UpdateClassroomOut ToUpdateClassroomOut()
        {
            return new()
            {
                Id = classroom.Id,
                Name = classroom.Name,
                CampusId = classroom.CampusId,
                Capacity = classroom.Capacity,
            };
        }
    }
}
