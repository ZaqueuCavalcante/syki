"use client"

import * as React from "react"
import {
  IconBell,
  IconCalendarShare,
  IconCategoryMinus,
  IconChalkboardTeacher,
  IconContract,
  IconHelp,
  IconInnerShadowTop,
  IconLayoutBoardSplitFilled,
  IconNotebook,
  IconSchool,
  IconSearch,
  IconSettings,
  IconShieldCheck,
  IconSitemap,
  IconUsers,
  IconWebhook,
} from "@tabler/icons-react"

import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar"
import { NavMain } from "./nav-main"
import { NavSecondary } from "./nav-secondary"
import { NavUser } from "./nav-user"
import { Link } from "react-router"

const data = {
  user: {
    name: "Zaqueu C.",
    email: "zaqueu@syki.com",
    avatar: "https://avatars.githubusercontent.com/u/52336768",
  },
  navMain: [
    {
      title: "Campi",
      url: "/academic/campi",
      icon: IconCategoryMinus,
    },
    {
      title: "Cursos",
      url: "/academic/courses",
      icon: IconNotebook,
    },
    {
      title: "Disciplinas",
      url: "#",
      icon: IconLayoutBoardSplitFilled,
    },
    {
      title: "Grades",
      url: "#",
      icon: IconSitemap,
    },
    {
      title: "Ofertas",
      url: "#",
      icon: IconShieldCheck,
    },
    {
      title: "Professores",
      url: "#",
      icon: IconChalkboardTeacher,
    },
    {
      title: "Turmas",
      url: "/academic/classes",
      icon: IconSchool,
    },
    {
      title: "Alunos",
      url: "#",
      icon: IconUsers,
    },
        {
      title: "Períodos",
      url: "#",
      icon: IconCalendarShare,
    },
        {
      title: "Matrículas",
      url: "#",
      icon: IconContract,
    },
    {
      title: "Notificações",
      url: "#",
      icon: IconBell,
    },
    {
      title: "Webhooks",
      url: "#",
      icon: IconWebhook,
    },
  ],
  navSecondary: [
    {
      title: "Settings",
      url: "#",
      icon: IconSettings,
    },
    {
      title: "Get Help",
      url: "#",
      icon: IconHelp,
    },
    {
      title: "Search",
      url: "#",
      icon: IconSearch,
    },
  ]
}

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  return (
    <Sidebar collapsible="offcanvas" {...props}>
      <SidebarHeader>
        <SidebarMenu>
          <SidebarMenuItem>
            <SidebarMenuButton
              asChild
              className="data-[slot=sidebar-menu-button]:!p-1.5"
            >
              <Link to="/">
                <IconInnerShadowTop className="!size-5" />
                <span className="text-base font-semibold">Syki</span>
              </Link>
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarHeader>
      <SidebarContent>
        <NavMain items={data.navMain} />
        <NavSecondary items={data.navSecondary} className="mt-auto" />
      </SidebarContent>
      <SidebarFooter>
        <NavUser user={data.user} />
      </SidebarFooter>
    </Sidebar>
  )
}
