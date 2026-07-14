<script setup lang="ts">
const route = useRoute()
const { account } = useUserAccount()

const classId = route.params.classId as string
</script>

<template>
  <LazyClassesDetailManager v-if="account?.userType === 'Manager'" :class-id="classId" />
  <LazyClassesDetailTeacher v-else-if="account?.userType === 'Teacher'" :class-id="classId" />
  <LazyClassesDetailStudent v-else-if="account?.userType === 'Student'" :class-id="classId" />
</template>
