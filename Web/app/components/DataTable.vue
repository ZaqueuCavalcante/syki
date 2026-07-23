<script setup lang="ts" generic="T">
import type { TableColumn } from '@nuxt/ui'

defineProps<{
  data: T[]
  columns: TableColumn<T>[]
  loading?: boolean
}>()

defineSlots<{
  empty(): any
}>()
</script>

<template>
  <MobileDataCards
    class="sm:hidden"
    :data="data"
    :columns="columns"
    :loading="loading"
  >
    <template #empty>
      <slot name="empty" />
    </template>
  </MobileDataCards>

  <UTable
    class="hidden sm:block"
    :data="data"
    :columns="columns"
    :loading="loading"
    :ui="{
      base: 'table-fixed border-separate border-spacing-0',
      thead: '[&>tr]:bg-elevated/50 [&>tr]:after:content-none',
      tbody: '[&>tr]:last:[&>td]:border-b-0',
      th: 'py-2 first:rounded-l-lg last:rounded-r-lg border-y border-default first:border-l last:border-r',
      td: 'border-b border-default whitespace-normal break-words',
    }"
  >
    <template #empty>
      <slot name="empty" />
    </template>
  </UTable>
</template>
