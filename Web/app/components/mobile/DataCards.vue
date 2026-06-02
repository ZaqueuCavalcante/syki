<script setup lang="ts" generic="T">
import type { TableColumn } from '@nuxt/ui'
import type { ColumnDef, ExpandedState } from '@tanstack/vue-table'
import { FlexRender, useVueTable, getCoreRowModel, getExpandedRowModel } from '@tanstack/vue-table'

const props = defineProps<{
  data: T[]
  columns: TableColumn<T>[]
  loading?: boolean
}>()

const expanded = defineModel<ExpandedState>('expanded', { default: () => ({}) })

defineSlots<{
  empty(): any
  expanded(props: { row: any }): any
}>()

const table = useVueTable({
  get data() { return props.data },
  get columns() { return props.columns as unknown as ColumnDef<T, any>[] },
  getCoreRowModel: getCoreRowModel(),
  getExpandedRowModel: getExpandedRowModel(),
  state: {
    get expanded() { return expanded.value as ExpandedState }
  },
  onExpandedChange: (updaterOrValue) => {
    expanded.value = typeof updaterOrValue === 'function'
      ? updaterOrValue(expanded.value as Record<string, boolean>)
      : updaterOrValue
  },
  getRowCanExpand: () => true
})

const rows = computed(() => table.getRowModel().rows)

function isActionCell(cell: any): boolean {
  return !cell.column.columnDef.header
}

function getCellRender(cell: any) {
  return cell.column.columnDef.cell ?? String(cell.getValue() ?? '')
}

const columnIdLabels: Record<string, string> = {
  actions: 'Ações',
}

function getHeaderLabel(cell: any): string {
  const header = cell.column.columnDef.header
  if (typeof header === 'string') return header
  return columnIdLabels[cell.column.id] ?? cell.column.id
}
</script>

<template>
  <div>
    <!-- Loading skeleton -->
    <div v-if="loading" class="space-y-3">
      <div
        v-for="i in 3"
        :key="i"
        class="rounded-lg border border-default divide-y divide-default"
      >
        <div v-for="j in 4" :key="j" class="flex items-center justify-between px-3 py-2">
          <USkeleton class="h-3.5 w-16" />
          <USkeleton class="h-3.5 w-28" />
        </div>
      </div>
    </div>

    <!-- Empty state -->
    <div v-else-if="rows.length === 0">
      <slot name="empty">
        <div class="flex flex-col items-center justify-center py-8 text-center">
          <UIcon name="i-lucide-inbox" class="size-10 text-muted mb-2" />
          <p class="text-muted">
            Nenhum item encontrado
          </p>
        </div>
      </slot>
    </div>

    <!-- Cards -->
    <div v-else class="space-y-3">
      <div v-for="row in rows" :key="row.id">
        <div class="rounded-lg border border-default overflow-hidden">
          <!-- Key-value rows -->
          <div class="divide-y divide-default">
            <template v-for="cell in row.getVisibleCells()" :key="cell.id">
              <div
                v-if="!isActionCell(cell)"
                class="flex items-center justify-between gap-3 px-3 py-2 text-sm"
              >
                <span class="text-muted text-xs font-medium shrink-0">
                  {{ getHeaderLabel(cell) }}
                </span>
                <div class="text-right">
                  <FlexRender
                    :render="getCellRender(cell)"
                    :props="cell.getContext()"
                  />
                </div>
              </div>
            </template>
          </div>

          <!-- Actions footer -->
          <template v-if="row.getVisibleCells().some(isActionCell)">
            <div class="data-cards-actions px-3 py-3 border-t border-default bg-elevated/50">
              <template v-for="cell in row.getVisibleCells()" :key="cell.id">
                <template v-if="isActionCell(cell)">
                  <FlexRender
                    :render="getCellRender(cell)"
                    :props="cell.getContext()"
                  />
                </template>
              </template>
            </div>
          </template>
        </div>

        <!-- Expanded content -->
        <div v-if="row.getIsExpanded()">
          <slot name="expanded" :row="row" />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.data-cards-actions :deep(> div) {
  display: flex;
  gap: 0.75rem;
}

.data-cards-actions :deep(> div > *) {
  flex: 1;
  display: flex;
  align-items: stretch;
  justify-content: center;
}

.data-cards-actions :deep(> div > * + *) {
  border-left: 1px solid var(--ui-border);
}

.data-cards-actions :deep(button),
.data-cards-actions :deep(a) {
  width: 100%;
  justify-content: center;
}

.data-cards-actions :deep(a > span) {
  width: 100%;
  justify-content: center;
}
</style>
