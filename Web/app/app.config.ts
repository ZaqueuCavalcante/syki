export default defineAppConfig({
  ui: {
    colors: {
      primary: 'violet',
      neutral: 'zinc'
    },
    dashboardPanel: {
      slots: {
        body: 'flex flex-col gap-4 sm:gap-6 flex-1 overflow-y-auto px-4 py-4 sm:p-6'
      }
    }
  }
})
