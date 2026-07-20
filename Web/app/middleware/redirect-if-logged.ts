// Nome do cookie httpOnly onde o backend guarda o JWT (ver JwtBearerScheme.Cookie no Back).
const BEARER_COOKIE = 'X-Estud-BearerCookie'

// Redireciona usuários já logados de / para /home, decidindo no servidor (SSR).
//
// O cookie de auth é httpOnly — invisível ao JS do browser —, mas o SSR do Nuxt enxerga
// o cookie da requisição. Usuário não logado não tem cookie, então caímos fora aqui sem
// nenhuma chamada ao backend: a landing é renderizada instantaneamente (caminho rápido).
//
// Só quando o cookie está presente confirmamos com o endpoint leve /users/logged, que
// valida o JWT em memória (sem tocar no banco). 204 => logado, redireciona; 401 => cookie
// expirado/inválido, renderiza a landing normalmente.
export default defineNuxtRouteMiddleware(async () => {
  if (import.meta.client) return

  const token = useCookie(BEARER_COOKIE).value
  if (!token) return

  const config = useRuntimeConfig()
  const baseURL = config.internalBackendUrl || config.public.backendUrl

  try {
    await $fetch('/users/logged', {
      baseURL,
      headers: useRequestHeaders(['cookie'])
    })
    return navigateTo('/home', { redirectCode: 302 })
  } catch { /* cookie inválido/expirado: segue para a landing */ }
})
