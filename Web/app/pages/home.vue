<script setup lang="ts">
const { account } = useUserAccount()
</script>

<template>
  <UDashboardPanel id="home">
    <template #header>
      <UDashboardNavbar title="Home" :ui="{ right: 'gap-3' }">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <HomeManager v-if="account?.userType === 'Manager'" />
      <HomeTeacher v-else-if="account?.userType === 'Teacher'" />
      <HomeStudent v-else-if="account?.userType === 'Student'" />
    </template>
  </UDashboardPanel>
</template>
