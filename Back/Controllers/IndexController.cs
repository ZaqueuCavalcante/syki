using Dapper;
using Npgsql;
using Syki.Shared;
using Syki.Back.Settings;
using Syki.Back.Database;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[ApiController, Route("[controller]")]
public class IndexController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public IndexController(SykiDbContext ctx) => _ctx = ctx;

    [HttpGet("academico")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> GetAllAcademico([FromServices] DatabaseSettings dbSettings)
    {
        using var connection = new NpgsqlConnection(dbSettings.ConnectionString);

        const string sql = @"
            SELECT
                (SELECT COUNT(1) FROM syki.campi WHERE faculdade_id = f.id) AS campus,
                (SELECT COUNT(1) FROM syki.cursos WHERE faculdade_id = f.id) AS cursos,
                (SELECT COUNT(1) FROM syki.disciplinas WHERE faculdade_id = f.id) AS disciplinas,
                (SELECT COUNT(1) FROM syki.grades WHERE faculdade_id = f.id) AS grades,
                (SELECT COUNT(1) FROM syki.ofertas WHERE faculdade_id = f.id) AS ofertas,
                (SELECT COUNT(1) FROM syki.turmas WHERE faculdade_id = f.id) AS turmas,
                (SELECT COUNT(1) FROM syki.professores WHERE faculdade_id = f.id) AS professores,
                (SELECT COUNT(1) FROM syki.alunos WHERE faculdade_id = f.id) AS alunos,
                (SELECT COUNT(1) FROM syki.livros WHERE faculdade_id = f.id) AS livros
            FROM
            	syki.faculdades f
            WHERE
            	f.id = @Facul
        ";

        var parameters = new { Facul = User.Facul() };

        var data = await connection.QueryFirstAsync<IndexAcademicoOut>(sql, parameters);
        
        return Ok(data);
    }
}
