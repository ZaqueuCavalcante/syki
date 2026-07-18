<script setup lang="ts">
import type { ClassSchedule } from '~/types/classes'

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{
  classId: number
  schedules: ClassSchedule[]
}>()
const emit = defineEmits<{ saved: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const saving = ref(false)

const dayOptions = [
  { label: 'Segunda', value: 'Monday' },
  { label: 'Terça', value: 'Tuesday' },
  { label: 'Quarta', value: 'Wednesday' },
  { label: 'Quinta', value: 'Thursday' },
  { label: 'Sexta', value: 'Friday' },
  { label: 'Sábado', value: 'Saturday' },
]

function buildHourOptions() {
  const opts = []
  for (let h = 7; h <= 23; h++) {
    for (let m = 0; m < 60; m += 15) {
      const hh = h.toString().padStart(2, '0')
      const mm = m.toString().padStart(2, '0')
      opts.push({ label: `${hh}:${mm}`, value: `H${hh}_${mm}` })
    }
  }
  return opts
}
const hourOptions = buildHourOptions()

interface Row {
  key: number
  day: string | undefined
  start: string | undefined
  end: string | undefined
}

let nextKey = 0
const rows = ref<Row[]>([])

function addRow() {
  rows.value = [...rows.value, { key: nextKey++, day: undefined, start: undefined, end: undefined }]
}

function removeRow(key: number) {
  rows.value = rows.value.filter(r => r.key !== key)
}

function hourValue(h: string) {
  return Number(h.replace(/^H/, '').replace('_', ''))
}

function rowIncomplete(r: Row) {
  return !r.day || !r.start || !r.end
}

function rowBadRange(r: Row) {
  return !!r.start && !!r.end && hourValue(r.start) >= hourValue(r.end)
}

const hasErrors = computed(() => rows.value.some(r => rowIncomplete(r) || rowBadRange(r)))

async function save() {
  if (hasErrors.value) return
  saving.value = true
  try {
    await $fetch(`${config.public.backendUrl}/classes/${props.classId}/schedules`, {
      method: 'PUT',
      body: { schedules: rows.value.map(r => ({ day: r.day, start: r.start, end: r.end })) },
      credentials: 'include',
    })
    toast.add({ title: 'Horários atualizados com sucesso', color: 'success' })
    open.value = false
    emit('saved')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao atualizar os horários.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    saving.value = false
  }
}

watch(open, (val) => {
  if (val) {
    rows.value = props.schedules.map(s => ({ key: nextKey++, day: s.day, start: s.startAt, end: s.endAt }))
  } else {
    rows.value = []
  }
})
</script>

<template>
  <UModal
    v-model:open="open"
    title="Horários da turma"
    :fullscreen="isMobile"
    description="Defina os horários semanais da turma."
  >
    <template #body>
      <div class="space-y-4">
        <div v-if="!rows.length" class="flex flex-col items-center gap-3 py-8 text-muted">
          <UIcon name="i-lucide-clock" class="size-10" />
          <p class="text-sm text-center">
            Nenhum horário definido
          </p>
        </div>

        <div v-else class="flex flex-col gap-3">
          <div v-for="row in rows" :key="row.key" class="flex flex-col gap-1">
            <div class="flex items-end gap-2">
              <USelect
                v-model="row.day"
                :items="dayOptions"
                value-key="value"
                class="flex-1"
                placeholder="Dia"
              />
              <USelect
                v-model="row.start"
                :items="hourOptions"
                value-key="value"
                class="w-24"
                placeholder="Início"
              />
              <USelect
                v-model="row.end"
                :items="hourOptions"
                value-key="value"
                class="w-24"
                placeholder="Fim"
              />
              <UButton
                icon="i-lucide-trash-2"
                color="error"
                variant="ghost"
                @click="() => { removeRow(row.key) }"
              />
            </div>
            <p v-if="rowBadRange(row)" class="text-xs text-error">
              O horário de início deve ser menor que o de fim.
            </p>
          </div>
        </div>

        <UButton
          icon="i-lucide-plus"
          label="Adicionar horário"
          color="neutral"
          variant="subtle"
          size="sm"
          @click="() => { addRow() }"
        />

        <div class="flex justify-end gap-2 pt-2">
          <UButton label="Cancelar" color="neutral" variant="subtle" :disabled="saving" @click="() => { open = false }" />
          <UButton label="Salvar" :loading="saving" :disabled="hasErrors" @click="() => { save() }" />
        </div>
      </div>
    </template>
  </UModal>
</template>
