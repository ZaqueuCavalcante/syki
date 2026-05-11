import { createSharedComposable } from '@vueuse/core'

interface UserAccount {
  id: string
  name: string
  email: string
  institution: string
  role: number
  profilePhoto: string | null
  course: string | null
}

const _useUserAccount = () => {
  const config = useRuntimeConfig()
  const account = ref<UserAccount | null>(null)

  async function fetchAccount() {
    account.value = await $fetch<UserAccount>(`${config.public.backendUrl}/users/account`, {
      credentials: 'include'
    })
  }

  return { account, fetchAccount }
}

export const useUserAccount = createSharedComposable(_useUserAccount)
