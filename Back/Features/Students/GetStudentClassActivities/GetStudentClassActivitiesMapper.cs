using Estud.Back.Domain.Classes;

namespace Estud.Back.Features.Students.GetStudentClassActivities;

public static class GetStudentClassActivitiesMapper
{
    extension(ClassActivity activity)
    {
        public GetStudentClassActivitiesItemOut ToGetStudentClassActivitiesItemOut(ClassActivityWork? work)
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
                WorkStatus = work?.Status ?? ClassActivityWorkStatus.Pending,
                WorkLink = work?.Link,
                Value = work?.Note ?? 0,
                PonderedValue = (work?.Note ?? 0) * activity.Weight / 100M,
            };
        }
    }
}
