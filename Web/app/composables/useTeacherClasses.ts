import { createSharedComposable } from '@vueuse/core'

export interface TeacherClassItem {
  id: number
  name: string
}

interface GetTeacherCurrentClassesOut {
  classes: TeacherClassItem[]
}

const _useTeacherClasses = () => {
  const config = useRuntimeConfig()
  const { can } = usePolicy()

  const classes = ref<TeacherClassItem[]>([])

  async function fetchClasses() {
    if (!can('GetTeacherCurrentClasses').value) {
      classes.value = []
      return
    }
    try {
      const data = await $fetch<GetTeacherCurrentClassesOut>(
        `${config.public.backendUrl}/teachers/current-classes`,
        { credentials: 'include' }
      )
      classes.value = data.classes
    } catch { /* ignore */ }
  }

  return { classes, fetchClasses }
}

export const useTeacherClasses = createSharedComposable(_useTeacherClasses)
