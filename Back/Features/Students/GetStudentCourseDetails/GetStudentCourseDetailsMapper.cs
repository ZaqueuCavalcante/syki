namespace Estud.Back.Features.Students.GetStudentCourseDetails;

public static class GetStudentCourseDetailsMapper
{
    extension(StudentClassStatus status)
    {
        public StudentDisciplineStatus ToStudentDisciplineStatus()
        {
            return status switch
            {
                StudentClassStatus.Aprovado => StudentDisciplineStatus.Aprovada,
                StudentClassStatus.Dispensado => StudentDisciplineStatus.Dispensada,
                StudentClassStatus.ReprovadoPorNota => StudentDisciplineStatus.Reprovada,
                StudentClassStatus.ReprovadoPorFalta => StudentDisciplineStatus.Reprovada,
                StudentClassStatus.Matriculado => StudentDisciplineStatus.Cursando,
                _ => StudentDisciplineStatus.NaoCursada,
            };
        }
    }

    extension(IEnumerable<StudentClassStatus> statuses)
    {
        /// <summary>
        /// Um aluno pode ter cursado a mesma disciplina em várias turmas (ex: reprovou e refez).
        /// A prioridade escolhe o desfecho mais relevante: aprovação/dispensa (conclusão) primeiro,
        /// depois "cursando", depois reprovação, e por fim "não cursada".
        /// </summary>
        public StudentDisciplineStatus ToBestStudentDisciplineStatus()
        {
            var mapped = statuses.Select(s => s.ToStudentDisciplineStatus()).ToList();

            if (mapped.Contains(StudentDisciplineStatus.Aprovada)) return StudentDisciplineStatus.Aprovada;
            if (mapped.Contains(StudentDisciplineStatus.Dispensada)) return StudentDisciplineStatus.Dispensada;
            if (mapped.Contains(StudentDisciplineStatus.Cursando)) return StudentDisciplineStatus.Cursando;
            if (mapped.Contains(StudentDisciplineStatus.Reprovada)) return StudentDisciplineStatus.Reprovada;

            return StudentDisciplineStatus.NaoCursada;
        }
    }
}
