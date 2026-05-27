interface GetTwoFactorKeyResponse {
  key: string
  qrCodeBase64: string
  twoFactorEnabled: boolean
}

export function useAuth() {
  const config = useRuntimeConfig()
  const { fetchAccount } = useUserAccount()

  async function fetchUser() {
    await fetchAccount()
  }

  async function twoFactorLogin(token: string): Promise<boolean> {
    try {
      await $fetch(`${config.public.backendUrl}/identity/2fa-login`, {
        method: 'POST',
        credentials: 'include',
        body: { token }
      })
      return true
    } catch {
      return false
    }
  }

  async function getTwoFactorKey(): Promise<GetTwoFactorKeyResponse> {
    return await $fetch<GetTwoFactorKeyResponse>(
      `${config.public.backendUrl}/identity/2fa-key`,
      { credentials: 'include' }
    )
  }

  async function setupTwoFactor(token: string): Promise<boolean> {
    try {
      await $fetch(`${config.public.backendUrl}/identity/2fa-setup`, {
        method: 'POST',
        credentials: 'include',
        body: { token }
      })
      return true
    } catch {
      return false
    }
  }

  return { fetchUser, twoFactorLogin, getTwoFactorKey, setupTwoFactor }
}
