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

const initials = computed(() => {
    const name = account.value?.name ?? ''
    return name.split(' ').map((w: string) => w[0]).slice(0, 2).join('').toUpperCase()
})

const items = computed<DropdownMenuItem[][]>(() => ([[
{
    label: 'Configurações',
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
    <UDropdownMenu
        :items="items"
        :content="{ align: 'center', collisionPadding: 12 }"
        :ui="{ content: collapsed ? 'w-48' : 'w-(--reka-dropdown-menu-trigger-width)' }"
    >
        <UButton
            v-if="collapsed"
            color="neutral"
            variant="ghost"
            square
            icon="i-lucide-user"
            class="data-[state=open]:bg-elevated"
        />
        <UButton
            v-else
            color="neutral"
            variant="ghost"
            block
            class="data-[state=open]:bg-elevated justify-start"
        >
            <div class="flex flex-col items-start min-w-0 w-full overflow-hidden text-left">
                <span class="truncate w-full text-sm font-medium">{{ account?.name }}</span>
                <span class="truncate w-full text-xs text-muted">{{ account?.role }}</span>
            </div>
        </UButton>
    </UDropdownMenu>
</template>
