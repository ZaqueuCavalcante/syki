"use client"

import * as React from "react"
import { NavUser } from "@/components/nav-user"
import { HomeButton } from "@/components/home-button"
import { NavOperation } from "@/components/nav-operation"
import { NavFundation } from "@/components/nav-fundation"

import {
  School,
  BookMarked,
  Grid3x3,
  Network,
  BookCheck,
  CalendarRange,
  SquareChartGantt,
  GraduationCap,
  Users,
  UserRoundPen,
  Bell,
} from "lucide-react"

import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarRail,
} from "@/components/ui/sidebar"

const data = {
  user: {
    name: "Zaqueu",
    email: "zaqueudovale@gmail.com",
    avatar: "https://avatars.githubusercontent.com/u/52336768",
  }
}

const fundationItems = [
  {
    name: "Campi",
    url: "#",
    icon: School,
  },
  {
    name: "Cursos",
    url: "#",
    icon: BookMarked,
  },
  {
    name: "Disciplinas",
    url: "#",
    icon: Grid3x3,
  },
  {
    name: "Grades",
    url: "#",
    icon: Network,
  },
  {
    name: "Ofertas",
    url: "#",
    icon: BookCheck
  },
  {
    name: "Períodos",
    url: "#",
    icon: CalendarRange,
  },
  {
    name: "Matrículas",
    url: "#",
    icon: SquareChartGantt
  }
]

const operationItems = [
  {
    name: "Turmas",
    url: "#",
    icon: GraduationCap,
  },
  {
    name: "Professores",
    url: "#",
    icon: UserRoundPen,
  },
  {
    name: "Alunos",
    url: "#",
    icon: Users,
  },
  {
    name: "Notificações",
    url: "#",
    icon: Bell,
  }
]

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  return (
    <Sidebar collapsible="icon" {...props}>
      <SidebarHeader>
        <HomeButton />
      </SidebarHeader>
      <SidebarContent>
        <NavFundation items={fundationItems} />
        <NavOperation items={operationItems} />
      </SidebarContent>
      <SidebarFooter>
        <NavUser user={data.user} />
      </SidebarFooter>
      <SidebarRail />
    </Sidebar>
  )
}
