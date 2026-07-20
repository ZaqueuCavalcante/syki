<script setup lang="ts">
const route = useRoute()
const { account } = useUserAccount()

const parentId = route.params.parentId as string
</script>

<template>
  <LazyParentsDetailManager v-if="account?.userType === 'Manager'" :parent-id="parentId" />
</template>
