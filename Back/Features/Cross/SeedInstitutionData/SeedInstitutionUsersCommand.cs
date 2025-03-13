using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Academic.CreateTeacher;

namespace Syki.Back.Features.Cross.SeedInstitutionData;

[CommandDescription("Realizar seed de usuários da instituição")]
public record SeedInstitutionUsersCommand(Guid InstitutionId, Guid DireitoCourseOfferingId, Guid AdsCourseOfferingId) : ICommand;

public class SeedInstitutionUsersCommandHandler(
    SykiDbContext ctx,
    CreateTeacherService createTeacherService,
    CreateStudentService createStudentService) : ICommandHandler<SeedInstitutionUsersCommand>
{
    public async Task Handle(Guid commandId, SeedInstitutionUsersCommand command)
    {
        var id = command.InstitutionId;

        var teachers = new List<string>()
        {
            "Davi Pessoa Ferraz",
            "Luciete Bezerra Alves",
            "Manuela Abath Valença",
            "Paulo Marcelo Pedrosa de Almeida",
        };
        foreach (var name in teachers)
        {
            await createTeacherService.CreateWithThrowOnError(id, CreateTeacherIn.Seed(name));
        }

        var direitoStudents = new List<string>()
        {
            "Alisson Aranda de Aguiar",
            "Maria Júlia de Oliveira Melo",
            "Everton Ian de Galhardo Filho",
            "Alma Celeste Maldonado Mendonça",
        };
        foreach (var name in direitoStudents)
        {
            await createStudentService.CreateWithThrowOnError(id, CreateStudentIn.Seed(name, command.DireitoCourseOfferingId));
        }

        var adsStudents = new List<string>()
        {
            "Simone Bezerra",
            "Marcelo Lima Filho",
            "Josilda Aragão Paz",
            "Marlene de Oliveira",
            "Miguel Gomes da Silva",
            "Zaqueu do Vale Cavalcante",
        };
        foreach (var name in adsStudents)
        {
            await createStudentService.CreateWithThrowOnError(id, CreateStudentIn.Seed(name, command.AdsCourseOfferingId));
        }

        ctx.AddCommand(id, new SeedInstitutionClassesCommand(id), parentId: commandId);
    }
}
