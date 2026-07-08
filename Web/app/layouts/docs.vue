<script setup lang="ts">
const route = useRoute()
const isMobile = useIsMobile()
const tocLinks = useState<any[]>('docs-toc', () => [])
const mobileNavOpen = ref(false)

const { data: navigation } = await useAsyncData('docs-nav', () =>
  queryCollectionNavigation('docs')
)

const { data: searchFiles } = await useAsyncData('search-sections', () =>
  queryCollectionSearchSections('docs')
)

watch(() => route.path, () => {
  mobileNavOpen.value = false
})
</script>

<template>
  <div>
    <UHeader :toggle="false">
      <template #left>
        <NuxtLink to="/" class="flex items-center gap-2 font-semibold text-default">
          Estud
          <UBadge label="Docs" variant="subtle" size="sm" />
        </NuxtLink>
      </template>

      <template #right>
        <UContentSearchButton />
        <UColorModeButton />
        <UButton
          icon="i-lucide-align-left"
          color="neutral"
          variant="ghost"
          size="sm"
          class="lg:hidden"
          @click="mobileNavOpen = true"
        />
      </template>
    </UHeader>

    <UMain>
      <UContainer>
        <UPage>
          <template #left>
            <UPageAside>
              <UContentNavigation :navigation="navigation?.[0]?.children" highlight />
            </UPageAside>
          </template>

          <slot />

          <template #right>
            <UPageAside>
              <UContentToc :links="tocLinks" title="Nesta página" />
            </UPageAside>
          </template>
        </UPage>
      </UContainer>
    </UMain>

    <USlideover v-model:open="mobileNavOpen" side="left" class="lg:hidden p-4">
      <template #content>
        <div class="flex flex-col gap-4 p-4 overflow-y-auto">
          <div class="flex items-center justify-between">
            <span class="font-semibold text-default">Documentação</span>
            <UButton
              icon="i-lucide-x"
              color="neutral"
              variant="ghost"
              size="sm"
              @click="mobileNavOpen = false"
            />
          </div>
          <UContentNavigation :navigation="navigation?.[0]?.children" highlight />
        </div>
      </template>
    </USlideover>

    <UContentSearch
      v-if="searchFiles"
      :files="searchFiles"
      :navigation="navigation ?? []"
      :fullscreen="isMobile"
      shortcut="meta_k"
    />
  </div>
</template>
