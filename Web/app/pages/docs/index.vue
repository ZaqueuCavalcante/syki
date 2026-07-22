<script setup lang="ts">
definePageMeta({ layout: 'docs' })

const tocLinks = useState<any[]>('docs-toc', () => [])
tocLinks.value = []

useSeoMeta({
  title: 'Documentação - Estud',
  description: 'Guia completo para configurar e usar a plataforma Estud.',
})

const sections = [
  {
    title: 'Introdução',
    description: 'Entenda o que é o Estud, seus conceitos fundamentais e casos de uso.',
    to: '/docs/introducao',
    icon: 'i-lucide-book-open',
  },
  {
    title: 'Como Começar',
    description: 'Crie sua conta, configure sua instituição e importe seus dados.',
    to: '/docs/como-comecar',
    icon: 'i-lucide-rocket',
  },
  {
    title: 'Funcionalidades',
    description: 'Explore as ferramentas de gestão acadêmica disponíveis na plataforma.',
    to: '/docs/funcionalidades',
    icon: 'i-lucide-layout-grid',
  },
  {
    title: 'Segurança',
    description: 'Autenticação em dois fatores, perfis de acesso e boas práticas.',
    to: '/docs/seguranca',
    icon: 'i-lucide-shield',
  },
]
</script>

<template>
  <div class="py-8 max-w-3xl">
    <div class="mb-12">
      <h1 class="text-4xl font-bold text-default mb-4">
        Documentação
      </h1>
      <p class="text-xl text-muted">
        Tudo que você precisa para começar a usar e aproveitar ao máximo a plataforma <strong>Estud</strong>.
      </p>
    </div>

    <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
      <NuxtLink
        v-for="section in sections"
        :key="section.to"
        :to="section.to"
        class="group block"
      >
        <UCard class="h-full transition-colors hover:ring-primary">
          <div class="flex items-start gap-4">
            <UIcon :name="section.icon" class="text-primary mt-0.5 shrink-0 size-6" />
            <div>
              <h2 class="font-semibold text-default group-hover:text-primary transition-colors mb-1">
                {{ section.title }}
              </h2>
              <p class="text-sm text-muted leading-relaxed">
                {{ section.description }}
              </p>
            </div>
          </div>
        </UCard>
      </NuxtLink>
    </div>
  </div>
</template>
