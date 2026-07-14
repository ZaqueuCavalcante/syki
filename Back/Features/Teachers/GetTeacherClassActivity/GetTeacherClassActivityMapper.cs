using Estud.Back.Domain.Classes;

namespace Estud.Back.Features.Teachers.GetTeacherClassActivity;

public static class GetTeacherClassActivityMapper
{
    extension(ClassActivity activity)
    {
        public GetTeacherClassActivityOut ToGetTeacherClassActivityOut()
        {
            return new()
            {
                Id = activity.Id,
                ClassId = activity.ClassId,
                Note = activity.Note,
                Title = activity.Title,
                Description = activity.Description,
                Type = activity.ActivityType,
                Status = activity.Status,
                Weight = activity.Weight,
                CreatedAt = activity.CreatedAt,
                DueDate = activity.DueDate,
                DueHour = activity.DueHour,
                DeliveredWorks = activity.Works.Count(w => w.Status != ClassActivityWorkStatus.Pending),
                TotalWorks = activity.Works.Count,
                Works = activity.Works
                    .OrderBy(w => w.Student.Name)
                    .Select(w => w.ToGetTeacherClassActivityWorkOut())
                    .ToList(),
            };
        }
    }

    extension(ClassActivityWork work)
    {
        public GetTeacherClassActivityWorkOut ToGetTeacherClassActivityWorkOut()
        {
            return new()
            {
                Id = work.Id,
                StudentId = work.StudentId,
                Student = work.Student.Name,
                Link = work.Link,
                Status = work.Status,
                Value = work.Note,
            };
        }
    }
}
