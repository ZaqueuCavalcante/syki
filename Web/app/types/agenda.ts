export interface AgendaDiscipline {
  classId: number | null
  name: string
  classroomName: string | null // null => aula online
  start: string // ex: "H07_00"
  end: string   // ex: "H08_45"
}

export interface AgendaDay {
  day: string // 'Monday' | 'Tuesday' | ...
  disciplines: AgendaDiscipline[]
}

export interface GetAgendaOut {
  days: AgendaDay[]
}
