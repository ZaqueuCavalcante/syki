<script setup lang="ts">
definePageMeta({
  layout: 'landing'
})

useSeoMeta({
  title: 'Contato — Estud',
  description: 'Fale com a equipe do Estud. Tire dúvidas, peça suporte ou converse com nosso time comercial.'
})

const channels = [
  {
    icon: 'i-lucide-life-buoy',
    title: 'Suporte',
    description: 'Dúvidas sobre uso, problemas técnicos ou sua conta.',
    email: 'suporte@estud.com.br',
  },
  {
    icon: 'i-lucide-briefcase',
    title: 'Comercial',
    description: 'Planos, contratação e parcerias com instituições.',
    email: 'comercial@estud.com.br',
  },
  {
    icon: 'i-lucide-shield-check',
    title: 'Privacidade (DPO)',
    description: 'Solicitações relacionadas aos seus dados pessoais (LGPD).',
    email: 'privacidade@estud.com.br',
  },
]
</script>

<template>
  <UContainer class="py-16 max-w-3xl">
    <h1 class="text-3xl font-bold text-gray-900 dark:text-white mb-2">
      Contato
    </h1>
    <p class="text-muted mb-12">
      Fale com a gente. Respondemos em até 1 dia útil.
    </p>

    <div class="grid gap-4 sm:grid-cols-1">
      <UCard v-for="channel in channels" :key="channel.email">
        <div class="flex items-start gap-4">
          <div class="flex items-center justify-center size-10 rounded-lg bg-primary/10 text-primary shrink-0">
            <UIcon :name="channel.icon" class="size-5" />
          </div>
          <div class="min-w-0">
            <h2 class="text-base font-semibold text-gray-900 dark:text-white">{{ channel.title }}</h2>
            <p class="text-sm text-muted mt-0.5">{{ channel.description }}</p>
            <a :href="`mailto:${channel.email}`" class="inline-block mt-2 text-sm text-primary hover:underline">
              {{ channel.email }}
            </a>
          </div>
        </div>
      </UCard>
    </div>

    <p class="text-sm text-muted mt-10">
      Você também pode falar com nosso time pelo e-mail
      <a href="mailto:contato@estud.com.br" class="text-primary hover:underline">contato@estud.com.br</a>.
    </p>
  </UContainer>
</template>
