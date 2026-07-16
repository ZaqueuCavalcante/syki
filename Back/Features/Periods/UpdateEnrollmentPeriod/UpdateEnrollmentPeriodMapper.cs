using Estud.Back.Domain.Periods;

namespace Estud.Back.Features.Periods.UpdateEnrollmentPeriod;

public static class UpdateEnrollmentPeriodMapper
{
    extension(EnrollmentPeriod period)
    {
        public UpdateEnrollmentPeriodOut ToUpdateEnrollmentPeriodOut()
        {
            return new()
            {
                Id = period.Id,
                Name = period.Name,
                StartAt = period.StartAt,
                EndAt = period.EndAt,
            };
        }
    }
}
