<script setup lang="ts">
const { account } = useUserAccount()
const { children, selectedChildId } = useParentChildren()

const items = computed(() => children.value.map(c => ({ label: c.name, value: c.id })))
</script>

<template>
  <USelectMenu
    v-if="account?.userType === 'Parent' && children.length > 1"
    v-model="selectedChildId"
    :items="items"
    value-key="value"
    icon="i-lucide-graduation-cap"
    :search-input="false"
    class="w-48"
  />
</template>
