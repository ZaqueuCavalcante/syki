using Dapper;
using Npgsql;

namespace Syki.Back.Services;

public class IndexService(DatabaseSettings dbSettings) : IIndexService
{
    public async Task<IndexAdmOut> GetAllAdm()
    {
        using var connection = new NpgsqlConnection(dbSettings.ConnectionString);

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

    public async Task<IndexAlunoOut> GetAllAluno(Guid id)
    {
        using var connection = new NpgsqlConnection(dbSettings.ConnectionString);

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
