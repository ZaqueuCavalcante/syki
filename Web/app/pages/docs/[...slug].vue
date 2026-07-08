<script setup lang="ts">
definePageMeta({ layout: 'docs' })

const route = useRoute()
const tocLinks = useState<any[]>('docs-toc', () => [])

const { data: page } = await useAsyncData(
  () => route.path,
  () => queryCollection('docs').path(route.path).first()
)

if (!page.value) {
  throw createError({ statusCode: 404, statusMessage: 'Página não encontrada' })
}

watchEffect(() => {
  tocLinks.value = page.value?.body?.toc?.links ?? []
})

const { data: surround } = await useAsyncData(
  () => `${route.path}-surround`,
  () => queryCollectionItemSurroundings('docs', route.path, { fields: ['title', 'description'] })
)

useSeoMeta({
  title: () => page.value ? `${page.value.title} - Estud Docs` : 'Estud Docs',
  description: () => page.value?.description ?? '',
})
</script>

<template>
  <div class="py-8 max-w-3xl">
    <div v-if="page" class="mb-8">
      <h1 class="text-3xl font-bold text-default mb-2">
        {{ page.title }}
      </h1>
      <p v-if="page.description" class="text-lg text-muted">
        {{ page.description }}
      </p>
    </div>

    <ContentRenderer
      v-if="page"
      :value="page"
      class="prose dark:prose-invert max-w-none"
    />

    <USeparator v-if="surround" class="my-10" />

    <UContentSurround v-if="surround" :surround="surround" />
  </div>
</template>
