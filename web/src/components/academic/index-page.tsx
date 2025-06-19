import {
  SidebarInset,
  SidebarProvider,
} from "@/components/ui/sidebar"

import { AppSidebar } from "@/components/core/app-sidebar"
import { SectionCards } from "@/components/core/section-cards"
import { SiteHeader } from "@/components/core/site-header"

export default function AcademicIndexPage() {
  return (
    <SidebarProvider
      style={
        {
          "--sidebar-width": "calc(var(--spacing) * 62)",
          "--header-height": "calc(var(--spacing) * 12)",
        } as React.CSSProperties
      }
    >
      <AppSidebar variant="inset" />
      <SidebarInset>
        <SiteHeader />
        <div className="flex flex-1 flex-col">
          <div className="@container/main flex flex-1 flex-col gap-2">
            <div className="flex flex-col gap-4 py-4 md:gap-6 md:py-6">
              <SectionCards />
            </div>
          </div>
        </div>
      </SidebarInset>
    </SidebarProvider>
  )
}
