using Estud.Back.Domain.Periods;

namespace Estud.Back.Features.Periods.GetEnrollmentPeriods;

public static class GetEnrollmentPeriodsMapper
{
    extension(EnrollmentPeriod period)
    {
        public GetEnrollmentPeriodsItemOut ToGetEnrollmentPeriodsItemOut()
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
