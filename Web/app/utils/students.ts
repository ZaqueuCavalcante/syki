type BadgeColor = 'neutral' | 'primary' | 'success' | 'warning' | 'error' | 'info'

export const studentStatusLabels: Record<string, string> = {
  Enrolled: 'Matriculado',
  Transferred: 'Transferido',
  Deferred: 'Trancado',
  Completed: 'Concluído',
}

export const studentStatusColors: Record<string, BadgeColor> = {
  Enrolled: 'info',
  Transferred: 'warning',
  Deferred: 'neutral',
  Completed: 'success',
}

export const courseSessionLabels: Record<string, string> = {
  Morning: 'Matutino',
  Afternoon: 'Vespertino',
  Evening: 'Noturno',
}
