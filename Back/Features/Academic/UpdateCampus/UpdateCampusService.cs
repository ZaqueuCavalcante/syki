namespace Syki.Back.Features.Academic.UpdateCampus;

public class UpdateCampusService(SykiDbContext ctx)
{
	public async Task<CampusOut> Update(Guid institutionId, UpdateCampusIn data)
	{
		var campus = await ctx.Campi
			.FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == data.Id);

		if (campus == null)
			Throw.DE010.Now();

		campus.Update(data.Name, data.City);
		await ctx.SaveChangesAsync();

		return campus.ToOut();
	}
}
