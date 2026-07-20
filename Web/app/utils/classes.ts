import type { ClassLessonItem, ClassSchedule } from '~/types/classes'

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

export const classActivityTypeLabels: Record<string, string> = {
  Exam: 'Prova',
  Project: 'Projeto',
  Work: 'Trabalho',
  Presentation: 'Apresentação',
}

export const classActivityStatusLabels: Record<string, string> = {
  Pending: 'Pendente',
  Published: 'Publicada',
  Finalized: 'Finalizada',
}

export const classActivityStatusColors: Record<string, BadgeColor> = {
  Pending: 'neutral',
  Published: 'info',
  Finalized: 'success',
}

export const classActivityWorkStatusLabels: Record<string, string> = {
  Pending: 'Pendente',
  Delivered: 'Entregue',
  Finalized: 'Finalizado',
}

export const classActivityWorkStatusColors: Record<string, BadgeColor> = {
  Pending: 'neutral',
  Delivered: 'info',
  Finalized: 'success',
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

export function formatClassSchedule(s: Pick<ClassSchedule, 'day' | 'startAt' | 'endAt'>) {
  return `${dayLabels[s.day] ?? s.day} · ${formatClassHour(s.startAt)} – ${formatClassHour(s.endAt)}`
}

export function formatClassActivityDueDate(dueDate: string, dueHour: string) {
  const [year, month, day] = dueDate.split('-')
  return `${day}/${month}/${year} · ${formatClassHour(dueHour)}`
}

export const classLessonStatusLabels: Record<string, string> = {
  Pending: 'Pendente',
  Finalized: 'Concluída',
}

export const classLessonStatusColors: Record<string, BadgeColor> = {
  Pending: 'neutral',
  Finalized: 'success',
}

export function formatClassLessonDate(date: string) {
  const [year, month, day] = date.split('-')
  return `${day}/${month}/${year}`
}

export function formatClassLesson(lesson: ClassLessonItem) {
  return `${formatClassLessonDate(lesson.date)} · ${formatClassHour(lesson.startAt)} – ${formatClassHour(lesson.endAt)}`
}
