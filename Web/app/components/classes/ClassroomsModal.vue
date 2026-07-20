<script setup lang="ts">
import type { ClassSchedule } from '~/types/classes'

interface ClassroomOption {
  id: number
  name: string
  campusId: number
  campus: string
  capacity: number
}

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{
  classId: number
  campusId: number | null
  vacancies: number
  schedules: ClassSchedule[]
}>()
const emit = defineEmits<{ saved: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()

const classrooms = ref<ClassroomOption[]>([])
const loadingOptions = ref(false)
const saving = ref(false)

// 0 = "Sem sala" — sentinela para o horário sem sala alocada.
const NO_CLASSROOM = 0

interface Row {
  key: number
  day: string
  startAt: string
  endAt: string
  classroomId: number
}

let nextKey = 0
const rows = ref<Row[]>([])

// Só as salas do campus da turma podem sediar seus horários.
const campusClassrooms = computed(() =>
  props.campusId == null ? [] : classrooms.value.filter(c => c.campusId === props.campusId),
)

const classroomOptions = computed(() => [
  { label: 'Sem sala', value: NO_CLASSROOM },
  ...campusClassrooms.value.map(c => ({
    label: `${c.name} · ${c.capacity} lugares`,
    value: c.id,
  })),
])

async function fetchClassrooms() {
  loadingOptions.value = true
  try {
    classrooms.value = await $fetch<ClassroomOption[]>(
      `${config.public.backendUrl}/classrooms`,
      { credentials: 'include' },
    )
  } catch {
    classrooms.value = []
    toast.add({ title: 'Erro', description: 'Erro ao carregar as salas.', color: 'error' })
  } finally {
    loadingOptions.value = false
  }
}

function rowTooSmall(row: Row) {
  const classroom = campusClassrooms.value.find(c => c.id === row.classroomId)
  return !!classroom && classroom.capacity < props.vacancies
}

const hasErrors = computed(() => rows.value.some(rowTooSmall))

async function save() {
  if (hasErrors.value) return
  saving.value = true
  try {
    await $fetch(`${config.public.backendUrl}/classes/${props.classId}/classrooms`, {
      method: 'PUT',
      body: {
        classrooms: rows.value
          .filter(r => r.classroomId !== NO_CLASSROOM)
          .map(r => ({
            day: r.day,
            start: r.startAt,
            end: r.endAt,
            classroomId: r.classroomId,
          })),
      },
      credentials: 'include',
    })
    toast.add({ title: 'Salas atualizadas com sucesso', color: 'success' })
    open.value = false
    emit('saved')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao atualizar as salas.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    saving.value = false
  }
}

watch(open, (val) => {
  if (val) {
    rows.value = props.schedules.map(s => ({
      key: nextKey++,
      day: s.day,
      startAt: s.startAt,
      endAt: s.endAt,
      classroomId: s.classroomId ?? NO_CLASSROOM,
    }))
    fetchClassrooms()
  } else {
    rows.value = []
    classrooms.value = []
  }
})
</script>

<template>
  <UModal
    v-model:open="open"
    title="Salas da turma"
    :fullscreen="isMobile"
    description="Aloque uma sala para cada horário da turma."
  >
    <template #body>
      <div class="space-y-4">
        <div v-if="campusId == null" class="flex flex-col items-center gap-3 py-8 text-muted">
          <UIcon name="i-lucide-building-2" class="size-10" />
          <p class="text-sm text-center">
            Turmas sem campus não utilizam salas.
          </p>
        </div>

        <div v-else-if="!rows.length" class="flex flex-col items-center gap-3 py-8 text-muted">
          <UIcon name="i-lucide-clock" class="size-10" />
          <p class="text-sm text-center">
            Defina os horários da turma antes de alocar as salas.
          </p>
        </div>

        <div v-else-if="loadingOptions" class="flex justify-center py-8">
          <UIcon name="i-lucide-loader-circle" class="size-6 animate-spin text-muted" />
        </div>

        <div v-else-if="!campusClassrooms.length" class="flex flex-col items-center gap-3 py-8 text-muted">
          <UIcon name="i-lucide-door-closed" class="size-10" />
          <p class="text-sm text-center">
            Nenhuma sala cadastrada no campus desta turma.
          </p>
        </div>

        <div v-else class="flex flex-col gap-3">
          <div v-for="row in rows" :key="row.key" class="flex flex-col gap-1">
            <div class="flex flex-col gap-2 rounded-lg border border-default p-3">
              <span class="flex items-center gap-1.5 text-xs text-muted">
                <UIcon name="i-lucide-clock" class="size-3.5" />
                {{ formatClassSchedule({ day: row.day, startAt: row.startAt, endAt: row.endAt }) }}
              </span>
              <USelect
                v-model="row.classroomId"
                :items="classroomOptions"
                value-key="value"
                class="w-full"
                placeholder="Sala"
                icon="i-lucide-door-open"
              />
            </div>
            <p v-if="rowTooSmall(row)" class="text-xs text-error">
              A capacidade da sala é menor que o número de vagas da turma.
            </p>
          </div>
        </div>

        <div class="flex justify-end gap-2 pt-2">
          <UButton label="Cancelar" color="neutral" variant="subtle" :disabled="saving" @click="() => { open = false }" />
          <UButton
            v-if="campusId != null && rows.length && campusClassrooms.length"
            label="Salvar"
            :loading="saving"
            :disabled="loadingOptions || hasErrors"
            @click="() => { save() }"
          />
        </div>
      </div>
    </template>
  </UModal>
</template>
