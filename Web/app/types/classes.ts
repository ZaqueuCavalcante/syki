export interface ClassSchedule {
  day: string // 'Monday' | 'Tuesday' | ...
  startAt: string // ex: "H07_00"
  endAt: string // ex: "H10_00"
}

export interface ClassStudentItem {
  id: number
  name: string
  status: string
}

export interface GetClassOut {
  id: number
  discipline: string
  teacher: string
  period: string
  vacancies: number
  workload: number
  status: string
  schedules: ClassSchedule[]
  students: ClassStudentItem[]
}

export interface GetTeacherClassOut {
  id: number
  discipline: string
  period: string
  vacancies: number
  workload: number
  status: string
  schedules: ClassSchedule[]
  students: ClassStudentItem[]
}

export interface GetStudentClassOut {
  id: number
  discipline: string
  teacher: string
  period: string
  workload: number
  status: string
  myStatus: string
  schedules: ClassSchedule[]
}
