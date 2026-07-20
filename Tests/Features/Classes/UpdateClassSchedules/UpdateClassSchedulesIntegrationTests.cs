namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateClassSchedules(1, []);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateClassSchedules(1, []);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_class_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateClassSchedules(999999, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_class_is_already_started()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        await using (var ctx = _back.GetDbContext())
        {
            var entity = await ctx.Classes.FirstAsync(c => c.Id == @class.Id);
            entity.Status = ClassStatus.Started;
            await ctx.SaveChangesAsync();
        }

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Assert
        result.ShouldBeError(ClassAlreadyStarted.I);
    }

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_class_is_finalized()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        await using (var ctx = _back.GetDbContext())
        {
            var entity = await ctx.Classes.FirstAsync(c => c.Id == @class.Id);
            entity.Status = ClassStatus.Finalized;
            await ctx.SaveChangesAsync();
        }

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Assert
        result.ShouldBeError(ClassAlreadyStarted.I);
    }

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_end_is_before_start()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H10_00, Hour.H07_00, null)]);

        // Assert
        result.ShouldBeError(InvalidSchedule.I);
    }

    // F2: início == fim → InvalidSchedule
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_start_equals_end()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H07_00, null)]);

        // Assert
        result.ShouldBeError(InvalidSchedule.I);
    }

    // F3: choque entre os próprios horários → ConflictingSchedules
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_schedules_conflict_with_each_other()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        // Act
        var result = await client.UpdateClassSchedules(@class.Id,
        [
            (Day.Monday, Hour.H07_00, Hour.H10_00, null),
            (Day.Monday, Hour.H08_00, Hour.H09_00, null),
        ]);

        // Assert
        result.ShouldBeError(ConflictingSchedules.I);
    }

    // F4: dia fora do enum → InvalidDay
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_day_is_invalid()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [((Day)99, Hour.H07_00, Hour.H10_00, null)]);

        // Assert
        result.ShouldBeError(InvalidDay.I);
    }

    // F5: hora fora do enum → InvalidHour
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_hour_is_invalid()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, (Hour)9999, Hour.H10_00, null)]);

        // Assert
        result.ShouldBeError(InvalidHour.I);
    }

    // P2c: turma com 2 professores e slot sem professor → ScheduleTeacherRequired
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_a_slot_has_no_teacher_and_the_class_has_two_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var chico = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();
        var ana = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(chico.Id, [discipline.Id]);
        await client.AssignDisciplinesToTeacher(ana.Id, [discipline.Id]);

        var @class = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(@class.Id, [chico.Id, ana.Id]);

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Assert
        result.ShouldBeError(ScheduleTeacherRequired.I);
    }

    // P2d: turma com 2 professores e slot com professor de fora → InvalidScheduleTeacher
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_a_slot_teacher_is_not_in_the_class()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var chico = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();
        var ana = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();
        var outsider = await client.CreateTeacher("João Alves", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(chico.Id, [discipline.Id]);
        await client.AssignDisciplinesToTeacher(ana.Id, [discipline.Id]);

        var @class = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(@class.Id, [chico.Id, ana.Id]);

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, outsider.Id)]);

        // Assert
        result.ShouldBeError(InvalidScheduleTeacher.I);
    }

    // C1: professor com horário chocando em outra turma não finalizada → TeacherScheduleConflict
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_they_conflict_with_another_class_of_the_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var teacher = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var classA = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(classA.Id, [teacher.Id]);
        await client.UpdateClassSchedules(classA.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        var classB = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(classB.Id, [teacher.Id]);

        // Act
        var result = await client.UpdateClassSchedules(classB.Id, [(Day.Monday, Hour.H08_00, Hour.H09_00, null)]);

        // Assert
        result.ShouldBeError(TeacherScheduleConflict.I);
    }

    #endregion

    #region Happy path

    // G4: turma OnPreEnrollment é editável (ordena por dia)
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_set_the_schedules_of_a_class_on_pre_enrollment()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        // Act
        var result = await client.UpdateClassSchedules(@class.Id,
        [
            (Day.Wednesday, Hour.H07_00, Hour.H10_00, null),
            (Day.Monday, Hour.H07_00, Hour.H10_00, null),
        ]);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClass(@class.Id).Success();
        updated.Schedules.Select(s => s.Day).Should().Equal(Day.Monday, Day.Wednesday);
    }

    // G5: turma OnEnrollment ainda é editável (só bloqueia a partir de Started)
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_set_the_schedules_of_a_class_on_enrollment()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        await using (var ctx = _back.GetDbContext())
        {
            var entity = await ctx.Classes.FirstAsync(c => c.Id == @class.Id);
            entity.Status = ClassStatus.OnEnrollment;
            await ctx.SaveChangesAsync();
        }

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClass(@class.Id).Success();
        updated.Schedules.Should().ContainSingle();
    }

    // P0a: turma sem professores → horário salvo sem professor
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_save_schedules_without_teacher_when_the_class_has_no_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClass(@class.Id).Success();
        updated.Schedules.Should().ContainSingle();
        updated.Schedules[0].TeacherId.Should().BeNull();
    }

    // P0b: turma sem professores ignora o professor informado no slot
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_ignore_the_slot_teacher_when_the_class_has_no_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        var teacher = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();

        // Act — professor informado, mas a turma não tem professores
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, teacher.Id)]);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClass(@class.Id).Success();
        updated.Schedules.Should().ContainSingle();
        updated.Schedules[0].TeacherId.Should().BeNull();
    }

    // P1a: turma com 1 professor preenche o professor automaticamente
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_auto_assign_the_only_teacher_to_every_slot()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var teacher = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var @class = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(@class.Id, [teacher.Id]);

        // Act — sem informar professor no slot
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClass(@class.Id).Success();
        updated.Schedules.Should().ContainSingle();
        updated.Schedules[0].TeacherId.Should().Be(teacher.Id);
        updated.Schedules[0].Teacher.Should().Be("Chico Ferreira");
    }

    // P1b: turma com 1 professor sobrescreve o professor informado pelo único professor
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_override_the_slot_teacher_with_the_only_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var teacher = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);
        var outsider = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();

        var @class = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(@class.Id, [teacher.Id]);

        // Act — informa um professor que nem é da turma; deve ser ignorado
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, outsider.Id)]);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClass(@class.Id).Success();
        updated.Schedules.Should().ContainSingle();
        updated.Schedules[0].TeacherId.Should().Be(teacher.Id);
    }

    // P1c: turma com 1 professor aceita o professor informado quando é o próprio
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_accept_the_slot_teacher_when_it_is_the_only_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var teacher = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var @class = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(@class.Id, [teacher.Id]);

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, teacher.Id)]);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClass(@class.Id).Success();
        updated.Schedules.Should().ContainSingle();
        updated.Schedules[0].TeacherId.Should().Be(teacher.Id);
    }

    // P2a: turma com 2 professores aceita slot só de um deles (o outro pode ficar sem horário)
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_allow_a_slot_for_only_one_of_the_two_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var chico = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();
        var ana = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(chico.Id, [discipline.Id]);
        await client.AssignDisciplinesToTeacher(ana.Id, [discipline.Id]);

        var @class = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(@class.Id, [chico.Id, ana.Id]);

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, chico.Id)]);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClass(@class.Id).Success();
        updated.Schedules.Should().ContainSingle();
        updated.Schedules[0].TeacherId.Should().Be(chico.Id);
    }

    // P2b: turma com 2 professores grava o professor informado em cada slot
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_assign_a_teacher_to_each_slot_when_the_class_has_two_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var chico = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();
        var ana = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(chico.Id, [discipline.Id]);
        await client.AssignDisciplinesToTeacher(ana.Id, [discipline.Id]);

        var @class = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(@class.Id, [chico.Id, ana.Id]);

        // Act
        var result = await client.UpdateClassSchedules(@class.Id,
        [
            (Day.Monday, Hour.H07_00, Hour.H10_00, chico.Id),
            (Day.Wednesday, Hour.H07_00, Hour.H10_00, ana.Id),
        ]);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClass(@class.Id).Success();
        updated.Schedules.Should().HaveCount(2);
        updated.Schedules.Single(s => s.Day == Day.Monday).TeacherId.Should().Be(chico.Id);
        updated.Schedules.Single(s => s.Day == Day.Wednesday).TeacherId.Should().Be(ana.Id);
    }

    // C2: professor livre em outro dia não gera conflito
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_conflict_when_the_teacher_is_free_on_another_day()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var teacher = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var classA = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(classA.Id, [teacher.Id]);
        await client.UpdateClassSchedules(classA.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        var classB = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(classB.Id, [teacher.Id]);

        // Act — mesmo professor, mas na quarta (livre)
        var result = await client.UpdateClassSchedules(classB.Id, [(Day.Wednesday, Hour.H07_00, Hour.H10_00, null)]);

        // Assert
        result.ShouldBeSuccess();
    }

    // C3: com 2 professores, o professor compartilhado livre naquele slot não gera conflito
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_conflict_when_the_shared_teacher_is_free_on_that_slot()
    {
        // Arrange — turma com 2 professores: Chico na segunda, Ana na quarta.
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var chico = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();
        var ana = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(chico.Id, [discipline.Id]);
        await client.AssignDisciplinesToTeacher(ana.Id, [discipline.Id]);

        var classA = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(classA.Id, [chico.Id, ana.Id]);
        await client.UpdateClassSchedules(classA.Id,
        [
            (Day.Monday, Hour.H07_00, Hour.H10_00, chico.Id),
            (Day.Wednesday, Hour.H07_00, Hour.H10_00, ana.Id),
        ]);

        // Outra turma só com o Chico, no mesmo horário da QUARTA (dia da Ana em classA).
        var classB = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(classB.Id, [chico.Id]);

        // Act — Chico está livre na quarta, então não deve haver conflito.
        var result = await client.UpdateClassSchedules(classB.Id, [(Day.Wednesday, Hour.H07_00, Hour.H10_00, null)]);

        // Assert
        result.ShouldBeSuccess();
    }

    // C4: turma finalizada não conta como conflito de agenda
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_conflict_with_a_finalized_class_of_the_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var teacher = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var classA = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(classA.Id, [teacher.Id]);
        await client.UpdateClassSchedules(classA.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        await using (var ctx = _back.GetDbContext())
        {
            var entity = await ctx.Classes.FirstAsync(c => c.Id == classA.Id);
            entity.Status = ClassStatus.Finalized;
            await ctx.SaveChangesAsync();
        }

        var classB = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(classB.Id, [teacher.Id]);

        // Act — mesmo horário da turma finalizada
        var result = await client.UpdateClassSchedules(classB.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Assert
        result.ShouldBeSuccess();
    }

    // C5: mesmo horário para professores diferentes entre turmas não gera conflito
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_allow_the_same_schedule_for_two_different_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var chico = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();
        var ana = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(chico.Id, [discipline.Id]);
        await client.AssignDisciplinesToTeacher(ana.Id, [discipline.Id]);

        var classA = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(classA.Id, [chico.Id]);
        await client.UpdateClassSchedules(classA.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        var classB = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassTeachers(classB.Id, [ana.Id]);

        // Act
        var result = await client.UpdateClassSchedules(classB.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Assert
        result.ShouldBeSuccess();
    }

    // R1: replace-all substitui a lista atual
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_replace_the_current_schedules_of_the_class()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Tuesday, Hour.H19_00, Hour.H22_00, null)]);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClass(@class.Id).Success();
        updated.Schedules.Should().HaveCount(1);
        updated.Schedules[0].Day.Should().Be(Day.Tuesday);
        updated.Schedules[0].StartAt.Should().Be(Hour.H19_00);
        updated.Schedules[0].EndAt.Should().Be(Hour.H22_00);
    }

    // R2: lista vazia remove todos os horários
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_remove_all_schedules_of_the_class_when_list_is_empty()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, []);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClass(@class.Id).Success();
        updated.Schedules.Should().BeEmpty();
    }

    // R3: reenviar a mesma lista é idempotente
    [Test]
    public async Task Classes_UpdateClassSchedules_Should_be_idempotent_when_resubmitting_the_same_schedules()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Act — mesma lista de novo
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClass(@class.Id).Success();
        updated.Schedules.Should().ContainSingle();
        updated.Schedules[0].Day.Should().Be(Day.Monday);
    }

    #endregion
}
