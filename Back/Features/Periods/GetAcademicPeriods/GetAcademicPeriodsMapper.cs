using Syki.Back.Domain.Periods;

namespace Syki.Back.Features.Periods.GetAcademicPeriods;

public static class GetAcademicPeriodsMapper
{
    extension(AcademicPeriod period)
    {
        public GetAcademicPeriodsItemOut ToGetAcademicPeriodsItemOut()
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
