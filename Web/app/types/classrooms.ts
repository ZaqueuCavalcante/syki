export interface ClassroomScheduleItem {
  classId: number
  discipline: string
  period: string
  status: string // 'OnPreEnrollment' | 'OnEnrollment' | 'AwaitingStart' | 'Started' | 'Finalized'
  students: number
  teachers: string[]
  day: string // 'Monday' | 'Tuesday' | ...
  startAt: string // ex: "H07_00"
  endAt: string // ex: "H10_00"
}

export interface GetClassroomOut {
  id: number
  name: string
  campusId: number
  campus: string
  capacity: number
  classesCount: number
  weeklyHours: number
  peakStudents: number
  schedules: ClassroomScheduleItem[]
}
