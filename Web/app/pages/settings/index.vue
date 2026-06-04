<script setup lang="ts">
const { account, fetchAccount } = useUserAccount()

if (!account.value) await fetchAccount()
</script>

<template>
  <UPageCard
    title="Geral"
    description="Informações da sua conta."
    variant="subtle"
  >
    <div class="flex flex-col divide-y divide-default">
      <div class="flex max-sm:flex-col justify-between items-start gap-4 py-4 first:pt-0 last:pb-0">
        <div>
          <p class="text-sm font-medium text-highlighted">Nome</p>
        </div>
        <p class="text-sm text-muted">{{ account?.name }}</p>
      </div>

      <div class="flex max-sm:flex-col justify-between items-start gap-4 py-4 first:pt-0 last:pb-0">
        <div>
          <p class="text-sm font-medium text-highlighted">Email</p>
        </div>
        <p class="text-sm text-muted">{{ account?.email }}</p>
      </div>

      <div class="flex max-sm:flex-col justify-between items-start gap-4 py-4 first:pt-0 last:pb-0">
        <div>
          <p class="text-sm font-medium text-highlighted">Instituição</p>
        </div>
        <p class="text-sm text-muted">{{ account?.institution }}</p>
      </div>

      <div class="flex max-sm:flex-col justify-between items-start gap-4 py-4 first:pt-0 last:pb-0">
        <div>
          <p class="text-sm font-medium text-highlighted">Perfil de acesso</p>
        </div>
        <p class="text-sm text-muted">{{ account?.role }}</p>
      </div>

      <div v-if="account?.course" class="flex max-sm:flex-col justify-between items-start gap-4 py-4 first:pt-0 last:pb-0">
        <div>
          <p class="text-sm font-medium text-highlighted">Curso</p>
        </div>
        <p class="text-sm text-muted">{{ account?.course }}</p>
      </div>
    </div>
  </UPageCard>
</template>
