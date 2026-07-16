<script setup lang="ts">
const { children, selectedChildId, loading } = useParentChildren()
</script>

<template>
  <UDashboardPanel id="children">
    <template #header>
      <UDashboardNavbar title="Filhos">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="loading" class="flex justify-center pt-16">
        <AppSpinner class="size-8 text-primary" />
      </div>

      <div v-else-if="children.length === 0" class="flex flex-col items-center gap-3 pt-16 text-center">
        <UIcon name="i-lucide-users-round" class="size-10 text-muted" />
        <p class="text-muted">
          Nenhum aluno vinculado à sua conta. Fale com a secretaria da instituição.
        </p>
      </div>

      <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 pt-4">
        <UCard
          v-for="child in children"
          :key="child.id"
          class="cursor-pointer transition-colors"
          :class="child.id === selectedChildId ? 'ring-2 ring-primary' : 'hover:bg-elevated/50'"
          @click="() => { selectedChildId = child.id }"
        >
          <div class="flex items-start justify-between gap-2">
            <div class="flex items-center gap-3">
              <UAvatar :alt="child.name" size="lg" />
              <div>
                <p class="font-medium">
                  {{ child.name }}
                </p>
                <p class="text-sm text-muted">
                  Matrícula: {{ child.enrollmentCode }}
                </p>
              </div>
            </div>
            <UBadge v-if="child.id === selectedChildId" color="primary" variant="subtle">
              Selecionado
            </UBadge>
          </div>

          <div class="mt-4">
            <UBadge color="neutral" variant="subtle">
              {{ relationshipLabel(child.relationship) }}
            </UBadge>
          </div>
        </UCard>
      </div>
    </template>
  </UDashboardPanel>
</template>
