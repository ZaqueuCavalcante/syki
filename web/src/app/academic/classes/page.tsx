import { UUID } from "crypto";
import { Suspense } from "react";

interface AcademicClass {
  id: UUID;
  discipline: string;
  teacher: string;
  period: string;
  vacancies: number;
  frequency: number;
  status: string;
  fillRatio: string;
  isSelected: boolean;
}

export default function ClassesPage() {
    return (
        <>
            <div>CLASSES PAGE</div>
            <Suspense fallback={ <div>LOADING...</div> }>
                <ClassesTable />
            </Suspense>
        </>
    )
}

async function ClassesTable() {
    const authToken = '';
    const apiUrl = process.env.API_URL;

    const res = await fetch(`${apiUrl}/academic/classes`, {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${authToken}`,
        'Content-Type': 'application/json'
      },
    });

    if (!res.ok) {
        return (<></>);
    }

    const classes : AcademicClass[] = await res.json();

    return (
        <div>
            { classes.map(c => (<li key={c.id}>{c.discipline} | {c.teacher}</li>) ) }
        </div>
    );
}
