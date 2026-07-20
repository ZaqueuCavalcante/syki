import type { ClassSchedule } from '~/types/classes'

export interface TeacherCampusItem {
  id: number
  name: string
}

export interface TeacherDisciplineItem {
  id: number
  name: string
}

export interface TeacherClassItem {
  id: number
  discipline: string
  period: string
  vacancies: number
  students: number
  workload: number
  status: string
  // horários da turma cobertos por este professor
  schedules: ClassSchedule[]
}

export interface GetTeacherDetailsOut {
  id: number
  name: string
  email: string
  campi: TeacherCampusItem[]
  disciplines: TeacherDisciplineItem[]
  classes: TeacherClassItem[]
}
