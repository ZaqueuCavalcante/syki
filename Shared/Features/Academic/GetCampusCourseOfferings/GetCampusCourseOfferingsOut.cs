namespace Syki.Shared;

public class GetCampusCourseOfferingsOut
{
    public Guid CourseOfferingId { get; set; }
    public string Course { get; set; }
    public List<GetCampusCourseOfferingsDisciplineOut> Disciplines { get; set; }
}

public class GetCampusCourseOfferingsDisciplineOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ushort Workload { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((GetCampusCourseOfferingsDisciplineOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }

    public override string ToString()
    {
        return Name;
    }
}
