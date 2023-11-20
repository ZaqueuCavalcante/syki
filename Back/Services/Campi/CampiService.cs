using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class CampiService : ICampiService
{
    private readonly SykiDbContext _ctx;
    public CampiService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<CampusOut> Create(Guid faculdadeId, CampusIn data)
    {
        var campus = new Campus(data.Nome, data.Cidade, faculdadeId);

        await _ctx.Campi.AddAsync(campus);

        await _ctx.SaveChangesAsync();

        return campus.ToOut();
    }

    public async Task<CampusOut> Update(Guid faculdadeId, CampusOut data)
    {
        var campus = await _ctx.Campi
            .FirstOrDefaultAsync(x => x.FaculdadeId == faculdadeId && x.Id == data.Id);

        campus!.Update(data.Nome, data.Cidade);
        await _ctx.SaveChangesAsync();

        return campus.ToOut();
    }

    public async Task<List<CampusOut>> GetAll(Guid faculdadeId)
    {
        var campi = await _ctx.Campi
            .Where(c => c.FaculdadeId == faculdadeId).ToListAsync();

        return campi.ConvertAll(c => c.ToOut());
    }
}
