export function usePopoverMode() {
  const hasHover = useMediaQuery('(hover: hover)')
  return computed(() => hasHover.value ? 'hover' as const : 'click' as const)
}
