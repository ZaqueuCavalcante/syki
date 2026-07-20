type BadgeColor = 'neutral' | 'primary' | 'success' | 'warning' | 'error' | 'info'

export const parentStudentStatusLabels: Record<string, string> = {
  Pending: 'Pendente',
  Active: 'Ativo',
  Revoked: 'Revogado',
}

export const parentStudentStatusColors: Record<string, BadgeColor> = {
  Pending: 'warning',
  Active: 'success',
  Revoked: 'error',
}
