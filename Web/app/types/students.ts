export interface StudentCourse {
  courseOfferingId: number
  course: string
  campus: string
  period: string
  session: string // 'Morning' | 'Afternoon' | 'Evening'
  enrolledAt: string
}

export interface StudentClassItem {
  id: number
  discipline: string
  period: string
  workload: number
  status: string // 'OnPreEnrollment' | 'OnEnrollment' | 'OnReview' | 'Started' | 'Finalized'
  myStatus: string // 'Pendente' | 'Matriculado' | 'Aprovado' | ...
  averageGrade: number // nota média do aluno na turma (de 0 a 10)
  averageAttendance: number // frequência média do aluno na turma (de 0% a 100%)
}

export interface GetStudentDetailsOut {
  id: number
  name: string
  email: string
  enrollmentCode: string
  status: string // 'Enrolled' | 'Transferred' | 'Deferred' | 'Completed'
  yieldCoefficient: number
  averageGrade: number
  averageAttendance: number
  course: StudentCourse | null
  classes: StudentClassItem[]
}
