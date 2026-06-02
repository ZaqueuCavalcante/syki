<script setup lang="ts">
definePageMeta({ layout: false })

const { account, fetchAccount } = useUserAccount()
const checking = ref(true)

onMounted(async () => {
  try {
    if (!account.value) await fetchAccount()
  } catch { /* not logged in */ }
  checking.value = false
})

const features = [
  {
    icon: 'i-lucide-graduation-cap',
    title: 'Gestão Acadêmica',
    description: 'Organize disciplinas, cursos e grades curriculares com facilidade. Tudo centralizado e sempre atualizado.',
  },
  {
    icon: 'i-lucide-users',
    title: 'Alunos e Professores',
    description: 'Cadastro completo de alunos e docentes, com histórico, notas e frequência em tempo real.',
  },
  {
    icon: 'i-lucide-calendar',
    title: 'Turmas e Horários',
    description: 'Monte grades de horários sem conflitos. Associe professores e salas automaticamente.',
  },
  {
    icon: 'i-lucide-bar-chart-2',
    title: 'Relatórios e Métricas',
    description: 'Acompanhe indicadores de desempenho da instituição com dashboards claros e exportáveis.',
  },
  {
    icon: 'i-lucide-shield-check',
    title: 'Segurança e Controle',
    description: 'Permissões por perfil (admin, professor, aluno) com autenticação segura e auditoria completa.',
  },
  {
    icon: 'i-lucide-zap',
    title: 'Rápido e Moderno',
    description: 'Interface responsiva, modo escuro e desempenho otimizado para qualquer dispositivo.',
  },
]
</script>

<template>
  <div v-if="checking" class="min-h-screen flex items-center justify-center">
    <UIcon name="i-lucide-loader-circle" class="size-10 animate-spin text-muted" />
  </div>

  <NuxtLayout v-else :name="account ? 'default' : 'landing'">
    <!-- Dashboard (logged in) -->
    <UDashboardPanel v-if="account" id="home">
      <template #header>
        <UDashboardNavbar title="Home" :ui="{ right: 'gap-3' }">
          <template #leading>
            <UDashboardSidebarCollapse />
          </template>

        </UDashboardNavbar>
      </template>

      <template #body>
        <HomeManager v-if="account?.userType === 'Manager'" />
        <HomeTeacher v-else-if="account?.userType === 'Teacher'" />
        <HomeStudent v-else-if="account?.userType === 'Student'" />
      </template>
    </UDashboardPanel>

    <!-- Landing (not logged in) -->
    <div v-else>
      <UPageHero
        headline="Plataforma educacional"
        title="Gestão acadêmica para quem leva a educação a sério"
        description="Do cadastro de alunos aos relatórios de desempenho - tudo em uma plataforma moderna, segura e fácil de usar."
        :links="[
          { label: 'Começar grátis', to: '/register', size: 'xl' },
          { label: 'Ver demonstração', to: '#features', size: 'xl', color: 'neutral', variant: 'outline' },
        ]"
      />

      <USeparator />

      <UPageSection
        id="features"
        headline="Funcionalidades"
        title="Tudo que sua instituição precisa"
        description="Uma plataforma completa para gestão acadêmica, do primeiro acesso ao diploma."
      >
        <UPageGrid>
          <UPageCard
            v-for="feature in features"
            :key="feature.title"
            :icon="feature.icon"
            :title="feature.title"
            :description="feature.description"
            spotlight
          />
        </UPageGrid>
      </UPageSection>

      <USeparator />

      <UPageSection
        headline="Como funciona"
        title="Simples de começar"
        description="Configure sua instituição em minutos e comece a usar imediatamente."
        orientation="horizontal"
        :features="[
          { icon: 'i-lucide-user-plus', title: 'Crie sua conta', description: 'Cadastre-se gratuitamente e configure sua instituição em poucos minutos.' },
          { icon: 'i-lucide-database', title: 'Importe seus dados', description: 'Importe alunos, professores e disciplinas via planilha ou cadastre manualmente.' },
          { icon: 'i-lucide-rocket', title: 'Comece a usar', description: 'Acesse todos os módulos imediatamente. Suporte disponível 24/7.' },
        ]"
      />

      <USeparator />

      <UPageCTA
        title="Pronto para transformar sua instituição?"
        description="Junte-se a centenas de instituições que já usam o Estud. Grátis para começar, sem cartão de crédito."
        variant="subtle"
        :links="[
          { label: 'Criar conta grátis', to: '/register', size: 'xl' },
          { label: 'Falar com a equipe', to: 'mailto:contato@estud.com.br', size: 'xl', color: 'neutral', variant: 'ghost' },
        ]"
      />
    </div>
  </NuxtLayout>
</template>
