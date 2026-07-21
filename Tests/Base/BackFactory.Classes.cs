namespace Estud.Tests.Base;

public static class BackFactoryClasses
{
    public static async Task<int> GetFirstLessonId(this BackFactory factory, int classId)
    {
        await using var ctx = factory.GetDbContext();
        return await ctx.ClassLessons
            .Where(l => l.ClassId == classId)
            .OrderBy(l => l.Number)
            .Select(l => l.Id)
            .FirstAsync();
    }
}
