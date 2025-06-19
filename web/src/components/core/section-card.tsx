import {
  Card,
  CardAction,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Icon } from "@tabler/icons-react";

export function SectionCard(
{
  data,
}: {
  data: {
    title: string
    value: number
    icon: Icon
  }
}
) {
    return (
      <Card className="@container/card">
        <CardHeader>
          <CardDescription>{data.title}</CardDescription>
          <CardTitle className="text-2xl font-semibold tabular-nums @[250px]/card:text-3xl">
            {data.value.toLocaleString('pt-BR')}
          </CardTitle>
          <CardAction>
            {data.icon && <data.icon />}
          </CardAction>
        </CardHeader>
      </Card>
    );
}
