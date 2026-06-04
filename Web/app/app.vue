<script setup lang="ts">
import { extendLocale } from '@nuxt/ui/composables'
import { pt_br as ptBR } from '@nuxt/ui/locale'

const locale = extendLocale(ptBR, {
  messages: {
    selectMenu: { noData: 'Nenhum dado encontrado' },
    inputMenu: { noData: 'Nenhum dado encontrado' },
    listbox: { noData: 'Nenhum dado encontrado' },
    commandPalette: { noData: 'Nenhum dado encontrado' },
    table: { noData: 'Nenhum dado encontrado' },
  },
})

const colorMode = useColorMode()

const color = computed(() => colorMode.value === 'dark' ? '#1b1718' : 'white')

useEventListener('keydown', (e: KeyboardEvent) => {
  if (e.ctrlKey && e.shiftKey && e.key === 'L') {
    colorMode.preference = colorMode.value === 'dark' ? 'light' : 'dark'
  }
})

useHead({
  meta: [
    { charset: 'utf-8' },
    { name: 'viewport', content: 'width=device-width, initial-scale=1' },
    { key: 'theme-color', name: 'theme-color', content: color }
  ],
  link: [
    { rel: 'icon', href: '/favicon.ico' }
  ],
  htmlAttrs: {
    lang: 'en'
  }
})

const title = 'Estud'
const description = 'A professional education platform.'

useSeoMeta({
  title,
  description,
  ogTitle: title,
  ogDescription: description,
  twitterCard: 'summary_large_image',
  ogImage: 'https://ui.nuxt.com/assets/templates/nuxt/dashboard-light.png',
})
</script>

<template>
  <UApp :locale="locale">
    <NuxtLoadingIndicator color="var(--ui-primary)" />
    <NuxtLayout>
      <NuxtPage />
    </NuxtLayout>
  </UApp>
</template>
