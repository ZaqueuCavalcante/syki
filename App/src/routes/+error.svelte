<script lang="ts">
    import { page } from "$app/stores";
    import * as Empty from "$lib/components/ui/empty/index.js";
    import * as InputGroup from "$lib/components/ui/input-group/index.js";
    import * as Kbd from "$lib/components/ui/kbd/index.js";
    import SearchIcon from "@lucide/svelte/icons/search";
</script>

{#if $page.status === 404}
    <Empty.Root>
        <Empty.Header>
            <Empty.Title>404 - Not Found</Empty.Title>
            <Empty.Description>
                The page you're looking for doesn't exist. Try searching for
                what you need below.
            </Empty.Description>
        </Empty.Header>
        <Empty.Content>
            <InputGroup.Root class="sm:w-3/4">
                <InputGroup.Input placeholder="Try searching for pages..." />
                <InputGroup.Addon>
                    <SearchIcon />
                </InputGroup.Addon>
                <InputGroup.Addon align="inline-end">
                    <Kbd.Root>/</Kbd.Root>
                </InputGroup.Addon>
            </InputGroup.Root>
            <Empty.Description>
                Need help? <a href="#/">Contact support</a>
            </Empty.Description>
        </Empty.Content>
    </Empty.Root>
{:else}
    <Empty.Root>
        <Empty.Header>
            <Empty.Title>Error {$page.status}</Empty.Title>
            <Empty.Description>{$page.error?.message}</Empty.Description>
        </Empty.Header>
    </Empty.Root>
{/if}
