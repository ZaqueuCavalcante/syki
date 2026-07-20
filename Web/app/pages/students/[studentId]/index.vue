<script setup lang="ts">
const route = useRoute()
const { account } = useUserAccount()

const studentId = route.params.studentId as string
</script>

<template>
  <LazyStudentsDetailManager v-if="account?.userType === 'Manager'" :student-id="studentId" />
</template>
