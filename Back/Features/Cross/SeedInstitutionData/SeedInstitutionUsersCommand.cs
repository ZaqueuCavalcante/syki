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

        // Teachers
        await createTeacherService.Create(id, CreateTeacherIn.Seed("Davi Pessoa Ferraz"));
        await createTeacherService.Create(id, CreateTeacherIn.Seed("Luciete Bezerra Alves"));
        await createTeacherService.Create(id, CreateTeacherIn.Seed("Paulo Marcelo Pedrosa de Almeida"));
        await createTeacherService.Create(id, CreateTeacherIn.Seed("Manuela Abath Valença"));

        // Direito Students
        await createStudentService.Create(id, CreateStudentIn.Seed("Maria Júlia de Oliveira Melo", command.DireitoCourseOfferingId));
        await createStudentService.Create(id, CreateStudentIn.Seed("Everton Ian de Galhardo Filho", command.DireitoCourseOfferingId));
        await createStudentService.Create(id, CreateStudentIn.Seed("Alisson Aranda de Aguiar", command.DireitoCourseOfferingId));
        await createStudentService.Create(id, CreateStudentIn.Seed("Alma Celeste Maldonado Mendonça", command.DireitoCourseOfferingId));

        // ADS Students
        await createStudentService.Create(id, CreateStudentIn.Seed("Zaqueu do Vale Cavalcante", command.AdsCourseOfferingId));
        await createStudentService.Create(id, CreateStudentIn.Seed("Marlene de Oliveira", command.AdsCourseOfferingId));
        await createStudentService.Create(id, CreateStudentIn.Seed("Simone Bezerra", command.AdsCourseOfferingId));
        await createStudentService.Create(id, CreateStudentIn.Seed("Marcelo Lima Filho", command.AdsCourseOfferingId));
        await createStudentService.Create(id, CreateStudentIn.Seed("Josilda Aragão Paz", command.AdsCourseOfferingId));
        await createStudentService.Create(id, CreateStudentIn.Seed("Miguel Gomes da Silva", command.AdsCourseOfferingId));

        ctx.AddCommand(id, new SeedInstitutionClassesCommand(id), parentId: commandId);
    }
}
