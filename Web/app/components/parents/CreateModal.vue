<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'
import type { ParentRelationship } from '~/composables/useParentChildren'

interface StudentOption {
  id: number
  name: string
}

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ created: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)
const fetchLoading = ref(false)

const students = ref<StudentOption[]>([])

function onlyDigits(value: string) {
  return value.replace(/\D/g, '')
}

function formatPhoneNumber(value: string) {
  const digits = onlyDigits(value).slice(0, 11)

  if (digits.length === 0) return ''
  if (digits.length <= 2) return `(${digits}`
  if (digits.length <= 6) return `(${digits.slice(0, 2)}) ${digits.slice(2)}`
  if (digits.length <= 10) return `(${digits.slice(0, 2)}) ${digits.slice(2, 6)}-${digits.slice(6)}`

  return `(${digits.slice(0, 2)}) ${digits.slice(2, 7)}-${digits.slice(7)}`
}

// Escreve o valor mascarado de volta no elemento: sem isso, quando a máscara descarta
// o que foi digitado (uma letra), o model não muda, o Vue não re-renderiza e o caractere fica no DOM
function maskInput(event: Event, format: (value: string) => string) {
  const input = event.target as HTMLInputElement
  const formatted = format(input.value)
  input.value = formatted

  return formatted
}

const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório').max(100, 'Máximo 100 caracteres'),
  email: z.string().min(1, 'Email obrigatório').email('Email inválido'),
  phoneNumber: z.string()
    .refine(value => !value || /^\d{10,11}$/.test(onlyDigits(value)), 'Telefone inválido')
    .optional(),
  students: z.array(z.object({
    studentId: z.number({ error: 'Aluno obrigatório' }),
    relationship: z.string({ error: 'Parentesco obrigatório' }).min(1, 'Parentesco obrigatório'),
  })).min(1, 'Vincule pelo menos um aluno'),
})

type Schema = z.output<typeof schema>

interface StudentRow {
  studentId: number | undefined
  relationship: ParentRelationship | undefined
}

const formState = reactive({
  name: '',
  email: '',
  phoneNumber: '',
  students: [{ studentId: undefined, relationship: undefined }] as StudentRow[],
})

// Cada linha só oferece os alunos ainda não escolhidos nas outras linhas
function studentOptionsFor(index: number) {
  const usedIds = formState.students
    .filter((_, i) => i !== index)
    .map(row => row.studentId)

  return students.value.filter(s => !usedIds.includes(s.id))
}

function addRow() {
  formState.students.push({ studentId: undefined, relationship: undefined })
}

function removeRow(index: number) {
  formState.students.splice(index, 1)
}

async function fetchStudents() {
  fetchLoading.value = true
  try {
    const data = await $fetch<{ items: StudentOption[] }>(`${config.public.backendUrl}/students`, {
      credentials: 'include',
      query: { pageSize: 100 },
    })
    students.value = data.items
  } finally {
    fetchLoading.value = false
  }
}

function resetForm() {
  formState.name = ''
  formState.email = ''
  formState.phoneNumber = ''
  formState.students = [{ studentId: undefined, relationship: undefined }]
}

watch(open, (val) => {
  if (val) fetchStudents()
  else resetForm()
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    const phoneNumber = onlyDigits(event.data.phoneNumber ?? '')
    await $fetch(`${config.public.backendUrl}/parents`, {
      method: 'POST',
      body: {
        name: event.data.name,
        email: event.data.email,
        phoneNumber: phoneNumber || null,
        students: event.data.students,
      },
      credentials: 'include',
    })
    toast.add({ title: 'Responsável criado com sucesso', color: 'success' })
    open.value = false
    emit('created')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao criar responsável.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Novo responsável"
    :fullscreen="isMobile"
    description="Preencha os dados e vincule os alunos sob responsabilidade."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Nome" name="name">
          <UInput v-model="formState.name" class="w-full" placeholder="Ex: Ana Souza" />
        </UFormField>

        <UFormField label="Email" name="email">
          <UInput v-model="formState.email" type="email" class="w-full" placeholder="Ex: responsavel@email.com" />
        </UFormField>

        <UFormField label="Telefone" name="phoneNumber" hint="Opcional">
          <UInput
            :model-value="formState.phoneNumber"
            inputmode="numeric"
            class="w-full"
            placeholder="Ex: (81) 98570-6838"
            @input="(e) => { formState.phoneNumber = maskInput(e, formatPhoneNumber) }"
          />
        </UFormField>

        <UFormField label="Alunos vinculados" name="students">
          <div class="space-y-2">
            <div
              v-for="(row, index) in formState.students"
              :key="index"
              class="flex items-start gap-2"
            >
              <UFormField :name="`students.${index}.studentId`" class="flex-1">
                <USelectMenu
                  v-model="row.studentId"
                  :items="studentOptionsFor(index)"
                  label-key="name"
                  value-key="id"
                  class="w-full"
                  placeholder="Aluno"
                  :loading="fetchLoading"
                />
              </UFormField>

              <UFormField :name="`students.${index}.relationship`" class="w-44">
                <USelectMenu
                  v-model="row.relationship"
                  :items="relationshipOptions"
                  value-key="value"
                  class="w-full"
                  placeholder="Parentesco"
                  :search-input="false"
                />
              </UFormField>

              <UButton
                v-if="formState.students.length > 1"
                icon="i-lucide-trash-2"
                color="neutral"
                variant="ghost"
                aria-label="Remover aluno"
                @click="() => { removeRow(index) }"
              />
            </div>

            <UButton
              icon="i-lucide-plus"
              label="Adicionar aluno"
              color="neutral"
              variant="subtle"
              size="sm"
              @click="addRow"
            />
          </div>
        </UFormField>

        <div class="flex justify-end gap-2 pt-2">
          <UButton
            label="Cancelar"
            color="neutral"
            variant="subtle"
            :disabled="loading"
            @click="() => { open = false }"
          />
          <UButton
            label="Criar responsável"
            type="submit"
            :loading="loading"
          />
        </div>
      </UForm>
    </template>
  </UModal>
</template>
