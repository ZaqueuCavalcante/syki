<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

interface StudentItem {
  id: number
  currentCourseOfferingId: number | null
}

interface CourseOfferingItem {
  id: number
  course: string
  period: string
  session: string
}

const sessionLabels: Record<string, string> = {
  Morning: 'Matutino',
  Afternoon: 'Vespertino',
  Evening: 'Noturno',
}

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{ student: StudentItem | null }>()
const emit = defineEmits<{ enrolled: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)
const fetchLoading = ref(false)

const offerings = ref<CourseOfferingItem[]>([])

const offeringOptions = computed(() =>
  offerings.value.map(o => ({
    id: o.id,
    name: `${o.course} - ${o.period} - ${sessionLabels[o.session] ?? o.session}`,
  }))
)

const schema = z.object({
  courseOfferingId: z.number({ error: 'Oferta de curso obrigatória' }),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  courseOfferingId: undefined,
})

async function fetchData() {
  fetchLoading.value = true
  try {
    const [studentRes, offeringsRes] = await Promise.all([
      $fetch<{ currentCourseOfferingId: number | null }>(`${config.public.backendUrl}/students/${props.student!.id}`, { credentials: 'include' }),
      $fetch<{ items: CourseOfferingItem[] }>(`${config.public.backendUrl}/course-offerings`, { credentials: 'include' }),
    ])
    offerings.value = offeringsRes.items
    formState.courseOfferingId = studentRes.currentCourseOfferingId ?? undefined
  } finally {
    fetchLoading.value = false
  }
}

function resetForm() {
  formState.courseOfferingId = undefined
  offerings.value = []
}

watch(open, (val) => {
  if (val && props.student) fetchData()
  else resetForm()
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/students/${props.student!.id}/course-offerings`, {
      method: 'POST',
      body: { courseOfferingId: event.data.courseOfferingId },
      credentials: 'include',
    })
    toast.add({ title: 'Oferta de curso vinculada com sucesso', color: 'success' })
    open.value = false
    emit('enrolled')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao vincular oferta de curso.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Oferta de curso"
    :fullscreen="isMobile"
    description="Selecione a oferta de curso para o aluno."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Oferta de curso" name="courseOfferingId">
          <USelectMenu
            v-model="formState.courseOfferingId"
            :items="offeringOptions"
            label-key="name"
            value-key="id"
            class="w-full"
            placeholder="Selecione a oferta de curso"
            :loading="fetchLoading"
          />
        </UFormField>

        <div class="flex justify-end gap-2 pt-2">
          <UButton
            label="Cancelar"
            color="neutral"
            variant="subtle"
            :disabled="loading"
            @click="open = false"
          />
          <UButton
            label="Salvar"
            type="submit"
            :loading="loading"
          />
        </div>
      </UForm>
    </template>
  </UModal>
</template>
