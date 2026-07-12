export type DayType = 'Default' | 'Vacation' | 'Recess' | 'Holiday'

export interface CalendarItem {
  id: number | null // nulo quando o dia ainda não foi customizado pela instituição
  date: string // ex: "2026-01-01T00:00:00"
  dayType: DayType
  description: string | null
}

export interface GetInstitutionCalendarOut {
  year: number
  total: number
  items: CalendarItem[]
}
