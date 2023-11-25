using Dapper;
using Npgsql;
using Syki.Shared;
using Syki.Back.Settings;

namespace Syki.Back.Services;

public class IndexService : IIndexService
{
    private readonly DatabaseSettings _dbSettings;
    public IndexService(DatabaseSettings dbSettings) => _dbSettings = dbSettings;

    public async Task<IndexAcademicoOut> GetAllAcademico(Guid faculdadeId)
    {
        using var connection = new NpgsqlConnection(_dbSettings.ConnectionString);

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

        var parameters = new { Facul = faculdadeId };

        var data = await connection.QueryFirstAsync<IndexAcademicoOut>(sql, parameters);
        
        return data;
    }
}
