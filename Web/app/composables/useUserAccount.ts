import { createSharedComposable } from '@vueuse/core'

export type UserType = 'Manager' | 'Teacher' | 'Student'

interface UserAccount {
  id: string
  role: string
  name: string
  email: string
  userType: UserType
  institution: string
  permissions: number[]
  course: string | null
  profilePhoto: string | null
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
