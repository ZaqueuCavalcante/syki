// Contrato do endpoint GET /students/attendances/calendar
// (Back/Features/Students/GetStudentAttendanceCalendar).
// O enum é serializado pelo nome:
//   NoClass   = sem aula (fim de semana, feriado, férias, recesso ou dia sem aula do aluno)
//   Undefined = aula sem frequência definida (aula futura ou ainda não lançada)
//   Present   = presença
//   Absent    = falta
export type StudentDayAttendanceStatus = 'NoClass' | 'Undefined' | 'Present' | 'Absent'

export interface StudentAttendanceCalendarItem {
  date: string // ISO, ex: '2026-03-07T00:00:00'
  status: StudentDayAttendanceStatus
}

export interface GetStudentAttendanceCalendarOut {
  year: number
  total: number
  items: StudentAttendanceCalendarItem[]
}

// Forma já normalizada consumida pela grid (date reduzida a 'YYYY-MM-DD').
export interface AttendanceDay {
  date: string
  status: StudentDayAttendanceStatus
}
