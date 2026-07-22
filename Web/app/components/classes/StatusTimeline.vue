<script setup lang="ts">
import type { TimelineItem } from '@nuxt/ui'

const props = defineProps<{ status: string }>()

const isMobile = useIsMobile()
const orientation = computed(() => (isMobile.value ? 'vertical' : 'horizontal'))

const steps: TimelineItem[] = [
  { value: 'OnPreEnrollment', title: 'Pré-matrícula', icon: 'i-lucide-file-plus' },
  { value: 'OnEnrollment', title: 'Matrícula', icon: 'i-lucide-clipboard-list' },
  { value: 'OnReview', title: 'Revisão', icon: 'i-lucide-clipboard-check' },
  { value: 'Started', title: 'Iniciada', icon: 'i-lucide-circle-play' },
  { value: 'Finalized', title: 'Finalizada', icon: 'i-lucide-circle-check' },
]

// Explicação de cada status, exibida no popover ancorado no nome (mesmo texto da doc)
const descriptions: Record<string, string> = {
  OnPreEnrollment: 'Status inicial, logo após a criação',
  OnEnrollment: 'Aberta para matrícula de alunos, dentro do período de matrícula',
  OnReview: 'Período de matrícula encerrado, turma em revisão antes do início',
  Started: 'Turma em andamento — não é possível retroceder a partir daqui',
  Finalized: 'Encerrada ao fim do semestre, com notas e frequências salvas',
}

// Status cujo popover está aberto. Tanto a bolinha/ícone quanto o nome apontam
// para este mesmo estado, e o popover é sempre ancorado no nome — assim o hover
// no ícone abre a mesma tooltip, logo abaixo do nome do status.
const openStatus = ref<string | null>(null)

function openFor(value: string) {
  openStatus.value = value
}

function close() {
  openStatus.value = null
}

function toggle(value: string) {
  openStatus.value = openStatus.value === value ? null : value
}

// No modo horizontal o último passo não tem separador; sem encolhê-lo (flex-none)
// ele ainda ocupa 1/5 da largura, deixando um vão vazio à direita da timeline.
const items = computed<TimelineItem[]>(() =>
  steps.map((step, i) =>
    !isMobile.value && i === steps.length - 1
      ? { ...step, ui: { item: 'flex-none' } }
      : step,
  ),
)
</script>

<template>
  <UTimeline
    :model-value="props.status"
    :items="items"
    :orientation="orientation"
    color="primary"
    size="sm"
    class="w-full"
  >
    <template #indicator="{ item }">
      <UIcon
        :name="item.icon as string"
        class="size-4 cursor-default"
        @mouseenter="() => { openFor(item.value as string) }"
        @mouseleave="() => { close() }"
        @click="() => { toggle(item.value as string) }"
      />
    </template>

    <template #title="{ item }">
      <UPopover
        :open="openStatus === (item.value as string)"
        :content="{ side: 'bottom', align: 'start' }"
        @update:open="(v: boolean) => { if (!v) close() }"
      >
        <template #anchor>
          <span
            class="cursor-default"
            @mouseenter="() => { openFor(item.value as string) }"
            @mouseleave="() => { close() }"
            @click="() => { toggle(item.value as string) }"
          >{{ item.title }}</span>
        </template>
        <template #content>
          <div class="p-3 w-64">
            <p class="text-xs font-semibold text-highlighted">{{ item.title }}</p>
            <p class="text-xs text-muted mt-1">{{ descriptions[item.value as string] }}</p>
          </div>
        </template>
      </UPopover>
    </template>
  </UTimeline>
</template>
