import type { ContentNavigationItem } from '@nuxt/content'

export function useDocsNav() {
  function findBreadcrumbs(
    items: ContentNavigationItem[],
    path: string,
    ancestors: { label: string; to: string }[] = []
  ): { label: string; to: string }[] | null {
    for (const item of items) {
      const current = [...ancestors, { label: item.title ?? '', to: item.path ?? '' }]
      if (item.path === path) return current
      if (item.children) {
        const found = findBreadcrumbs(item.children, path, current)
        if (found) return found
      }
    }
    return null
  }

  return { findBreadcrumbs }
}
