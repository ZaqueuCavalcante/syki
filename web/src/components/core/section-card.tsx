import {
  Card,
  CardAction,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Badge } from "../ui/badge";
import { IconTrendingUp } from "@tabler/icons-react";

export function SectionCard({
  title = 'Title',
  value = 0,
  change = '0.0%'
}) {
    return (
      <Card className="@container/card">
        <CardHeader>
          <CardDescription>{title}</CardDescription>
          <CardTitle className="text-2xl font-semibold tabular-nums @[250px]/card:text-3xl">
            {value.toLocaleString('pt-BR')}
          </CardTitle>
          <CardAction>
            <Badge variant="outline">
              <IconTrendingUp />
              {change}
            </Badge>
          </CardAction>
        </CardHeader>
      </Card>
    );
}
