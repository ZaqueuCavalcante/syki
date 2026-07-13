import type { ClassSchedule } from '~/types/classes'

type BadgeColor = 'neutral' | 'primary' | 'success' | 'warning' | 'error' | 'info'

export const classStatusLabels: Record<string, string> = {
  OnPreEnrollment: 'Pré-matrícula',
  OnEnrollment: 'Matrícula',
  AwaitingStart: 'Aguardando início',
  Started: 'Iniciada',
  Finalized: 'Finalizada',
}

export const classStatusColors: Record<string, BadgeColor> = {
  OnPreEnrollment: 'neutral',
  OnEnrollment: 'info',
  AwaitingStart: 'warning',
  Started: 'primary',
  Finalized: 'success',
}

export const studentClassStatusLabels: Record<string, string> = {
  Pendente: 'Pendente',
  Matriculado: 'Matriculado',
  Aprovado: 'Aprovado',
  Dispensado: 'Dispensado',
  ReprovadoPorNota: 'Reprovado por nota',
  ReprovadoPorFalta: 'Reprovado por falta',
}

export const studentClassStatusColors: Record<string, BadgeColor> = {
  Pendente: 'neutral',
  Matriculado: 'info',
  Aprovado: 'success',
  Dispensado: 'warning',
  ReprovadoPorNota: 'error',
  ReprovadoPorFalta: 'error',
}

const dayLabels: Record<string, string> = {
  Sunday: 'Domingo',
  Monday: 'Segunda',
  Tuesday: 'Terça',
  Wednesday: 'Quarta',
  Thursday: 'Quinta',
  Friday: 'Sexta',
  Saturday: 'Sábado',
}

export function formatClassHour(value: string) {
  return value.replace(/^H/, '').replace('_', ':')
}

export function formatClassSchedule(s: ClassSchedule) {
  return `${dayLabels[s.day] ?? s.day} · ${formatClassHour(s.startAt)} – ${formatClassHour(s.endAt)}`
}
