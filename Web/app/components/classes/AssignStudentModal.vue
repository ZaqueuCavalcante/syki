<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

interface StudentItem { id: number; name: string }
interface GetStudentsOut { total: number; items: StudentItem[] }

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{ classId: number; enrolledIds: number[] }>()
const emit = defineEmits<{ assigned: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

const { data: studentsData } = await useFetch<GetStudentsOut>(
  `${config.public.backendUrl}/students`,
  { credentials: 'include', server: false },
)

const studentOptions = computed(() =>
  (studentsData.value?.items ?? [])
    .filter(s => !props.enrolledIds.includes(s.id))
    .map(s => ({ label: s.name, value: s.id }))
)

const schema = z.object({
  studentId: z.number({ error: 'Campo obrigatório' }),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({ studentId: undefined })

watch(open, (val) => { if (!val) formState.studentId = undefined })

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/students/${event.data.studentId}/classes`, {
      method: 'POST',
      body: { classId: props.classId },
      credentials: 'include',
    })
    toast.add({ title: 'Aluno matriculado com sucesso', color: 'success' })
    open.value = false
    emit('assigned')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao matricular aluno.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Matricular Aluno"
    :fullscreen="isMobile"
    description="Selecione o aluno a ser matriculado nesta turma."
  >
    <template #body>
      <UForm :schema="schema" :state="formState" class="space-y-4" @submit="onSubmit">

        <UFormField label="Aluno" name="studentId" required>
          <USelectMenu
            v-model="formState.studentId"
            :items="studentOptions"
            value-key="value"
            class="w-full"
            placeholder="Selecione o aluno"
            :search-input="{ placeholder: 'Buscar por nome...' }"
          />
        </UFormField>

        <div class="flex justify-end gap-2 pt-2">
          <UButton label="Cancelar" color="neutral" variant="subtle" :disabled="loading" @click="() => { open = false }" />
          <UButton label="Matricular" type="submit" :loading="loading" />
        </div>

      </UForm>
    </template>
  </UModal>
</template>
