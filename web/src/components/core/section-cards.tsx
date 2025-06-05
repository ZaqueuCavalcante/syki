import { SectionCard } from "./section-card"

export function SectionCards() {
  return (
    <div className="*:data-[slot=card]:from-primary/5 *:data-[slot=card]:to-card dark:*:data-[slot=card]:bg-card grid grid-cols-1 gap-4 px-4 *:data-[slot=card]:bg-gradient-to-t *:data-[slot=card]:shadow-xs lg:px-6 @xl/main:grid-cols-3 @5xl/main:grid-cols-4">
      <SectionCard title='Campus' value={13} change='+1.5%' />
      <SectionCard title='Cursos' value={4559} change='+1.5%' />
      <SectionCard title='Disciplinas' value={1597524} change='+1.5%' />

      <SectionCard title='Grades' value={49} change='+1.5%' />
      <SectionCard title='Ofertas' value={479} change='+1.5%' />
      <SectionCard title='Turmas' value={98} change='+1.5%' />
     
      <SectionCard title='Professores' value={158} change='+1.5%' />
      <SectionCard title='Alunos' value={74} change='+1.5%' />
      <SectionCard title='Notificações' value={9} change='+1.5%' />

      <SectionCard title='Webhooks' value={55} change='+1.5%' />
    </div>
  )
}
