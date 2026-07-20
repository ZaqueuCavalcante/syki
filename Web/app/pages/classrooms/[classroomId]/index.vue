<script setup lang="ts">
const route = useRoute()
const { account } = useUserAccount()

const classroomId = route.params.classroomId as string
</script>

<template>
  <LazyClassroomsDetailManager v-if="account?.userType === 'Manager'" :classroom-id="classroomId" />
</template>
