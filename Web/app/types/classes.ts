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

export interface ClassTeacherItem {
  id: number
  name: string
}

export interface GetClassOut {
  id: number
  disciplineId: number
  discipline: string
  teachers: ClassTeacherItem[]
  period: string
  vacancies: number
  workload: number
  status: string
  schedules: ClassSchedule[]
  students: ClassStudentItem[]
}

export interface ClassStatusImplication {
  icon: string
  class: string
  text: string
}

export interface ClassStatusTransition {
  // caminho do endpoint PUT (ex: 'start', 'release-for-enrollment')
  path: string
  title: string
  actionLabel: string
  actionIcon: string
  successTitle: string
  errorTitle: string
  fromStatus: string
  toStatus: string
  implications: ClassStatusImplication[]
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

export interface ClassLessonItem {
  id: number
  number: number
  date: string // ex: "2026-07-20"
  startAt: string // ex: "H19_00"
  endAt: string // ex: "H22_00"
  status: string // 'Pending' | 'Finalized'
  presentStudents: number[]
}

export interface GetTeacherClassLessonsOut {
  lessons: ClassLessonItem[]
}

export interface ClassActivityItem {
  id: number
  classId: number
  note: string // 'N1' | 'N2' | 'N3'
  title: string
  description: string
  type: string // 'Exam' | 'Project' | 'Work' | 'Presentation'
  status: string // 'Pending' | 'Published' | 'Finalized'
  weight: number
  createdAt: string
  dueDate: string // ex: "2026-07-20"
  dueHour: string // ex: "H19_00"
  deliveredWorks: number
  totalWorks: number
}

export interface GetTeacherClassActivitiesOut {
  activities: ClassActivityItem[]
}

export interface StudentClassActivityItem {
  id: number
  classId: number
  note: string // 'N1' | 'N2' | 'N3'
  title: string
  description: string
  type: string // 'Exam' | 'Project' | 'Work' | 'Presentation'
  status: string // 'Pending' | 'Published' | 'Finalized'
  weight: number
  createdAt: string
  dueDate: string // ex: "2026-07-20"
  dueHour: string // ex: "H19_00"
  workStatus: string // 'Pending' | 'Delivered' | 'Finalized'
  workLink: string | null
  value: number
  ponderedValue: number
}

export interface GetStudentClassActivitiesOut {
  activities: StudentClassActivityItem[]
}

export interface TeacherActivityWorkItem {
  id: number
  studentId: number
  student: string
  status: string // 'Pending' | 'Delivered' | 'Finalized'
  link: string | null
  value: number
}

export interface GetTeacherClassActivityOut {
  id: number
  classId: number
  note: string // 'N1' | 'N2' | 'N3'
  title: string
  description: string
  type: string // 'Exam' | 'Project' | 'Work' | 'Presentation'
  status: string // 'Pending' | 'Published' | 'Finalized'
  weight: number
  createdAt: string
  dueDate: string // ex: "2026-07-20"
  dueHour: string // ex: "H19_00"
  deliveredWorks: number
  totalWorks: number
  works: TeacherActivityWorkItem[]
}

export interface GetStudentClassActivityOut {
  id: number
  classId: number
  note: string // 'N1' | 'N2' | 'N3'
  title: string
  description: string
  type: string // 'Exam' | 'Project' | 'Work' | 'Presentation'
  status: string // 'Pending' | 'Published' | 'Finalized'
  weight: number
  createdAt: string
  dueDate: string // ex: "2026-07-20"
  dueHour: string // ex: "H19_00"
  workStatus: string // 'Pending' | 'Delivered' | 'Finalized'
  workLink: string | null
  value: number
  ponderedValue: number
}

export interface GetStudentClassOut {
  id: number
  discipline: string
  teachers: string[]
  period: string
  workload: number
  status: string
  myStatus: string
  schedules: ClassSchedule[]
}
