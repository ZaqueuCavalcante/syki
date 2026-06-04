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

  async function updateAccount(name: string) {
    await $fetch(`${config.public.backendUrl}/users/account`, {
      method: 'PUT',
      credentials: 'include',
      body: { name }
    })
    if (account.value) account.value.name = name
  }

  return { account, fetchAccount, updateAccount }
}

export const useUserAccount = createSharedComposable(_useUserAccount)
