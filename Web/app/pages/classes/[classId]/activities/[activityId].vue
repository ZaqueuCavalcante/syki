<script setup lang="ts">
const route = useRoute()
const { account } = useUserAccount()

const classId = route.params.classId as string
const activityId = route.params.activityId as string
</script>

<template>
  <LazyActivitiesDetailTeacher
    v-if="account?.userType === 'Teacher'"
    :class-id="classId"
    :activity-id="activityId"
  />
  <LazyActivitiesDetailStudent
    v-else-if="account?.userType === 'Student'"
    :class-id="classId"
    :activity-id="activityId"
  />
</template>
