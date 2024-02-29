using Dapper;
using Npgsql;
using Syki.Shared;
using Syki.Back.Settings;

namespace Syki.Back.Services;

public class IndexService : IIndexService
{
    private readonly DatabaseSettings _dbSettings;
    public IndexService(DatabaseSettings dbSettings) => _dbSettings = dbSettings;

    public async Task<IndexAdmOut> GetAllAdm()
    {
        using var connection = new NpgsqlConnection(_dbSettings.ConnectionString);

        const string sql = @"
            SELECT
                COUNT(1)-1 AS faculdades,
                (SELECT COUNT(1)-1 FROM syki.users) AS users,
                (SELECT COUNT(1) FROM syki.campi) AS campus,
                (SELECT COUNT(1) FROM syki.cursos) AS cursos,
                (SELECT COUNT(1) FROM syki.disciplinas) AS disciplinas,
                (SELECT COUNT(1) FROM syki.grades) AS grades,
                (SELECT COUNT(1) FROM syki.ofertas) AS ofertas,
                (SELECT COUNT(1) FROM syki.professores) AS professores,
                (SELECT COUNT(1) FROM syki.alunos) AS alunos
            FROM
            	syki.faculdades
        ";

        var data = await connection.QueryFirstAsync<IndexAdmOut>(sql);
        
        return data;
    }

    public async Task<IndexAcademicoOut> GetAllAcademico(Guid faculdadeId)
    {
        using var connection = new NpgsqlConnection(_dbSettings.ConnectionString);

        const string sql = @"
            SELECT
                (SELECT COUNT(1) FROM syki.campi WHERE institution_id = f.id) AS campus,
                (SELECT COUNT(1) FROM syki.cursos WHERE faculdade_id = f.id) AS cursos,
                (SELECT COUNT(1) FROM syki.disciplinas WHERE faculdade_id = f.id) AS disciplinas,
                (SELECT COUNT(1) FROM syki.grades WHERE faculdade_id = f.id) AS grades,
                (SELECT COUNT(1) FROM syki.ofertas WHERE faculdade_id = f.id) AS ofertas,
                (SELECT COUNT(1) FROM syki.turmas WHERE faculdade_id = f.id) AS turmas,
                (SELECT COUNT(1) FROM syki.professores WHERE faculdade_id = f.id) AS professores,
                (SELECT COUNT(1) FROM syki.alunos WHERE institution_id = f.id) AS alunos,
                (SELECT COUNT(1) FROM syki.notifications WHERE faculdade_id = f.id) AS notifications
            FROM
            	syki.faculdades f
            WHERE
            	f.id = @Facul
        ";

        var parameters = new { Facul = faculdadeId };

        var data = await connection.QueryFirstAsync<IndexAcademicoOut>(sql, parameters);
        
        return data;
    }

    public async Task<IndexAlunoOut> GetAllAluno(Guid id)
    {
        using var connection = new NpgsqlConnection(_dbSettings.ConnectionString);

        const string sql = @"
            SELECT COUNT(1)
            FROM syki.users
            WHERE id = @Id
        ";

        var parameters = new { Id = id };

        var data = await connection.QueryFirstAsync<IndexAlunoOut>(sql, parameters);

        data = new IndexAlunoOut
        {
            DisciplinasConcluidas = 5,
            DisciplinasTotal = 78,
            Media = 7.9M,
            CR = 6.5M,
        };
        
        return data;
    }
}
