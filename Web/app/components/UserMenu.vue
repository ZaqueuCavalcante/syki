<script setup lang="ts">
import type { DropdownMenuItem } from '@nuxt/ui'

defineProps<{
    collapsed?: boolean
}>()

const config = useRuntimeConfig()
const colorMode = useColorMode()
const { account } = useUserAccount()

async function logout() {
    await $fetch(`${config.public.backendUrl}/identity/logout`, {
        method: 'POST',
        credentials: 'include',
    })
    account.value = null
    await navigateTo('/')
}

const user = computed(() => ({
    name: account.value?.name ?? '',
    avatar: {
        src: account.value?.profilePhoto ?? undefined,
        alt: account.value?.name ?? ''
    }
}))

const items = computed<DropdownMenuItem[][]>(() => ([[{
    type: 'label',
    label: user.value.name,
    avatar: user.value.avatar
}], [{
    label: 'Profile',
    icon: 'i-lucide-user'
}, {
    label: 'Settings',
    icon: 'i-lucide-settings',
    to: '/settings'
},
{
    label: colorMode.value === 'dark' ? 'Light' : 'Dark',
    icon: colorMode.value === 'dark' ? 'i-lucide-sun' : 'i-lucide-moon',
    onSelect(e: Event) {
        e.preventDefault()
        colorMode.preference = colorMode.value === 'dark' ? 'light' : 'dark'
    }
},
{
    label: 'Sair',
    icon: 'i-lucide-log-out',
    onSelect: logout,
}]]))
</script>

<template>
    <UDropdownMenu :items="items" :content="{ align: 'center', collisionPadding: 12 }"
        :ui="{ content: collapsed ? 'w-48' : 'w-(--reka-dropdown-menu-trigger-width)' }">
        <UButton v-bind="{
            ...user,
            label: collapsed ? undefined : user?.name,
            trailingIcon: collapsed ? undefined : 'i-lucide-chevrons-up-down'
        }" color="neutral" variant="ghost" block :square="collapsed" class="data-[state=open]:bg-elevated" :ui="{
            trailingIcon: 'text-dimmed'
        }" />

    </UDropdownMenu>
</template>
