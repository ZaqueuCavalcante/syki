<script setup lang="ts">
// Spinner de CSS puro (sem SVG) para evitar o artefato de renderização do
// Firefox com ícones animados via `rotate` (nuxt/ui#633). Herda tamanho via
// classes `size-*` e cor via `text-*` (currentColor).
defineOptions({ inheritAttrs: false })
</script>

<template>
  <span
    role="status"
    aria-label="Carregando"
    :class="[
      'inline-block shrink-0 animate-spin rounded-full border-2 border-current border-r-transparent',
      $attrs.class as string,
    ]"
  />
</template>
