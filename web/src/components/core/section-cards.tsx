import { Suspense } from "react";
import { cookies } from "next/headers";
import { SectionCard } from "./section-card"
import { 
  IconBell,
  IconCategoryMinus,
  IconChalkboardTeacher,
  IconLayoutBoardSplitFilled,
  IconNotebook,
  IconSchool,
  IconShieldCheck,
  IconSitemap,
  IconUsers,
  IconWebhook,
 } from "@tabler/icons-react";

export function SectionCards() {
  return (
    <div className="*:data-[slot=card]:from-primary/5 *:data-[slot=card]:to-card dark:*:data-[slot=card]:bg-card grid grid-cols-1 gap-4 px-4 *:data-[slot=card]:bg-gradient-to-t *:data-[slot=card]:shadow-xs lg:px-6 @xl/main:grid-cols-3 @5xl/main:grid-cols-4">
      <Suspense fallback={ <div>LOADING...</div> }>
          <GetAcademicInsights />
      </Suspense>
    </div>
  )
}

interface AcademicInsights {
  campus: number;
  courses: number;
  disciplines: number;
  courseCurriculums: number;
  courseOfferings: number;
  classes: number;
  teachers: number;
  students: number;
  notifications: number;
  webhooks: number;
}

async function GetAcademicInsights() {
    const cookieStore = await cookies();
    const authToken = cookieStore.get('syki_jwt')?.value;
    const apiUrl = process.env.API_URL;

    const res = await fetch(`${apiUrl}/academic/insights`, {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${authToken}`,
        'Content-Type': 'application/json'
      },
    });

    if (!res.ok) {
        return (<></>);
    }

    const insights : AcademicInsights = await res.json();

    return (
        <>
          <SectionCard data={ { title: 'Campus', value: insights.campus, icon: IconCategoryMinus }  } />
          <SectionCard data={ { title: 'Cursos', value: insights.courses, icon: IconNotebook }  } />
          <SectionCard data={ { title: 'Disciplinas', value: insights.disciplines, icon: IconLayoutBoardSplitFilled }  } />

          <SectionCard data={ { title: 'Grades', value: insights.courseCurriculums, icon: IconSitemap }  } />
          <SectionCard data={ { title: 'Ofertas', value: insights.courseOfferings, icon: IconShieldCheck }  } />
          <SectionCard data={ { title: 'Turmas', value: insights.classes, icon: IconSchool }  } />

          <SectionCard data={ { title: 'Professores', value: insights.teachers, icon: IconChalkboardTeacher }  } />
          <SectionCard data={ { title: 'Alunos', value: insights.students, icon: IconUsers }  } />
          <SectionCard data={ { title: 'Notificações', value: insights.notifications, icon: IconBell }  } />

          <SectionCard data={ { title: 'Webhooks', value: insights.webhooks, icon: IconWebhook }  } />
        </>
    );
}
