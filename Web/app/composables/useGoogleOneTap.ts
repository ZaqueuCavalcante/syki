declare global {
  interface Window {
    google?: {
      accounts: {
        id: {
          initialize: (config: {
            client_id: string
            callback: (response: { credential: string }) => void
            auto_select?: boolean
            cancel_on_tap_outside?: boolean
          }) => void
          prompt: (notification?: (status: { isNotDisplayed: () => boolean, isSkippedMoment: () => boolean }) => void) => void
          renderButton: (element: HTMLElement, config: {
            type?: 'standard' | 'icon'
            theme?: 'outline' | 'filled_blue' | 'filled_black'
            size?: 'large' | 'medium' | 'small'
            text?: 'signin_with' | 'signup_with' | 'continue_with' | 'signin'
            locale?: string
            width?: number
          }) => void
        }
      }
    }
  }
}

const SCRIPT_SRC = 'https://accounts.google.com/gsi/client'

function loadGsiScript(): Promise<void> {
  return new Promise((resolve, reject) => {
    if (window.google?.accounts?.id) {
      resolve()
      return
    }

    const existing = document.querySelector(`script[src="${SCRIPT_SRC}"]`)
    if (existing) {
      existing.addEventListener('load', () => resolve())
      existing.addEventListener('error', () => reject(new Error('Failed to load Google Identity Services')))
      return
    }

    const script = document.createElement('script')
    script.src = SCRIPT_SRC
    script.async = true
    script.defer = true
    script.onload = () => resolve()
    script.onerror = () => reject(new Error('Failed to load Google Identity Services'))
    document.head.appendChild(script)
  })
}

export function useGoogleOneTap() {
  const config = useRuntimeConfig()
  const toast = useToast()
  const router = useRouter()
  const { fetchUser } = useAuth()

  const loggingIn = ref(false)

  async function handleCredential(credential: string) {
    loggingIn.value = true

    try {
      await $fetch<{ userId: string, orgId: string, permissions: number[] }>(
        `${config.public.backendUrl}/identity/social-login/google-one-tap`,
        {
          method: 'POST',
          credentials: 'include',
          body: { credential }
        }
      )

      await fetchUser()
      router.push('/')
    } catch (error: any) {
      loggingIn.value = false

      const errorData = typeof error?.data === 'string' ? JSON.parse(error.data) : error?.data

      const errorMessages: Record<string, string> = {
        GoogleOneTapLoginInvalidToken: 'Token do Google inválido. Tente novamente.',
        GoogleOneTapLoginDisabled: 'Login com Google não está disponível.',
        SocialLoginEmailNotVerified: 'Email não verificado pelo Google.',
        SocialLoginSsoRequired: 'Sua organização requer login via SSO corporativo.'
      }

      toast.add({
        title: 'Erro no login',
        description: errorMessages[errorData?.code] || errorData?.message || 'Falha na autenticação com Google. Tente novamente.',
        icon: 'i-lucide-x',
        color: 'error'
      })
    }
  }

  async function initOneTap(clientId: string) {
    try {
      await loadGsiScript()

      window.google!.accounts.id.initialize({
        client_id: clientId,
        callback: response => handleCredential(response.credential),
        auto_select: false,
        cancel_on_tap_outside: true
      })

      window.google!.accounts.id.prompt()
    } catch {
      // Google Identity Services not available — fallback to redirect flow
    }
  }

  async function renderButton(element: HTMLElement, clientId: string) {
    try {
      await loadGsiScript()

      window.google!.accounts.id.initialize({
        client_id: clientId,
        callback: response => handleCredential(response.credential),
        auto_select: false
      })

      window.google!.accounts.id.renderButton(element, {
        type: 'standard',
        theme: 'outline',
        size: 'large',
        text: 'signin_with',
        locale: 'pt-BR'
      })
    } catch {
      // Google Identity Services not available
    }
  }

  return {
    loggingIn: readonly(loggingIn),
    initOneTap,
    renderButton
  }
}
