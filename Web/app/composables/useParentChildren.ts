import { createSharedComposable } from '@vueuse/core'

export type ParentRelationship = 'Mother' | 'Father' | 'Guardian' | 'Other'

export interface ParentChild {
  id: number
  name: string
  enrollmentCode: string
  relationship: ParentRelationship
}

interface GetParentStudentsOut {
  items: ParentChild[]
}

const relationshipLabels: Record<ParentRelationship, string> = {
  Mother: 'Mãe',
  Father: 'Pai',
  Guardian: 'Responsável legal',
  Other: 'Outro',
}

export function relationshipLabel(relationship: ParentRelationship): string {
  return relationshipLabels[relationship] ?? relationship
}

const _useParentChildren = () => {
  const config = useRuntimeConfig()
  const { account } = useUserAccount()

  const children = ref<ParentChild[]>([])
  const selectedChildId = ref<number | undefined>(undefined)
  const loading = ref(false)

  const selectedChild = computed(() => children.value.find(c => c.id === selectedChildId.value) ?? null)

  async function fetchChildren() {
    if (account.value?.userType !== 'Parent') return
    loading.value = true
    try {
      const data = await $fetch<GetParentStudentsOut>(
        `${config.public.backendUrl}/parents/students`,
        { credentials: 'include' }
      )
      children.value = data.items
      if (!children.value.some(c => c.id === selectedChildId.value)) {
        selectedChildId.value = children.value[0]?.id
      }
    } catch { /* ignore */ }
    finally { loading.value = false }
  }

  watch(account, () => { fetchChildren() }, { immediate: true })

  return { children, selectedChild, selectedChildId, loading, fetchChildren }
}

export const useParentChildren = createSharedComposable(_useParentChildren)
