import type { ParentRelationship } from '~/composables/useParentChildren'

export interface ParentStudentItem {
  id: number
  name: string
  email: string
  enrollmentCode: string
  status: string // 'Enrolled' | 'Transferred' | 'Deferred' | 'Completed'
  relationship: ParentRelationship
  linkStatus: string // 'Pending' | 'Active' | 'Revoked'
  revokedByStudent: boolean
  linkedAt: string
  course: string | null
  campus: string | null
  period: string | null
}

export interface GetParentDetailsOut {
  id: number
  name: string
  email: string
  phoneNumber: string | null
  createdAt: string
  students: ParentStudentItem[]
}
