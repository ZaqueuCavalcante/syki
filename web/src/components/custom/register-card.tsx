import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Button } from "@/components/ui/button"

export function RegisterCard() {
  return (
    <Card className="w-full max-w-sm">
      <CardHeader>
        <CardTitle>Cadastre seu email para ter acesso ao sistema</CardTitle>
        <CardDescription>
          Vamos te enviar um link para que você possa confirmar seu email e criar sua conta
        </CardDescription>
      </CardHeader>
      <CardContent className="py-8">
        <form>
          <div className="flex flex-col gap-6">
            <div className="grid gap-2">
              <Label htmlFor="email">Email</Label>
              <Input
                id="email"
                type="email"
                placeholder="zezinho@gmail.com"
                required
              />
            </div>
          </div>
        </form>
      </CardContent>
      <CardFooter className="flex-col gap-6">
        <Button type="submit" className="w-full">
          Cadastrar
        </Button>
      </CardFooter>
    </Card>
  )
}
