<script setup lang="ts">
const route = useRoute()
const { account } = useUserAccount()

const teacherId = route.params.teacherId as string
</script>

<template>
  <LazyTeachersDetailManager v-if="account?.userType === 'Manager'" :teacher-id="teacherId" />
</template>
