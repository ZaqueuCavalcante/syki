import Link from "next/link"
import { ArrowRight, CheckCircle } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card } from "@/components/ui/card"

export default function Home() {
  return (
    <div className="container max-w-6xl mx-auto px-4 py-8">
      <main>
        <div className="grid grid-cols-1 md:grid-cols-12 gap-8 mb-16">
          <div className="md:col-span-5 flex flex-col justify-center">
            <h1 className="text-4xl font-bold mb-6">Gerencie sua instituição de ensino com maestria</h1>
            <p className="text-xl mb-8">
              Acadêmico, financeiro, secretaria, professores e alunos: tudo integrado de maneira fácil e intuitiva.
            </p>
            <Button asChild size="lg" className="h-16 text-lg font-semibold w-full md:w-auto px-8">
              <Link href="/register">
                Cadastre-se agora
                <ArrowRight className="ml-2 h-5 w-5" />
              </Link>
            </Button>
          </div>

          <div className="md:col-span-7">
            <Card className="overflow-hidden px-2 py-2">
              <div className="relative aspect-video">
                <img
                  src="syki_home_light.png"
                  alt="Sistema Syki em ação"
                  className="w-full h-full"
                />
              </div>
            </Card>
          </div>
        </div>

        <div className="mb-16">
          <h2 className="text-2xl font-semibold mb-6">
            Syki é um sistema de gerenciamento acadêmico, que possui diversas funcionalidades para agilizar sua gestão:
          </h2>

          <div className="space-y-4">
            {features.map((feature, index) => (
              <div key={index} className="flex items-start gap-3">
                <CheckCircle className="h-6 w-6 text-green-500 flex-shrink-0 mt-0.5" />
                <p className="text-lg">{feature}</p>
              </div>
            ))}
          </div>
        </div>

        <div className="bg-muted rounded-lg p-8 text-center">
          <h2 className="text-2xl font-bold mb-4">Pronto para transformar a gestão da sua instituição?</h2>
          <p className="text-lg mb-6">
            Comece hoje mesmo e descubra como o Syki pode otimizar seus processos acadêmicos.
          </p>
          <Button asChild size="lg" className="h-14 text-lg font-semibold px-8">
            <Link href="/register">
              Iniciar agora
              <ArrowRight className="ml-2 h-5 w-5" />
            </Link>
          </Button>
        </div>
      </main>

      <footer className="mt-16 text-center text-muted-foreground">
        <p>© {new Date().getFullYear()} Syki - Sistema de Gerenciamento Acadêmico</p>
      </footer>
    </div>
  )
}

const features = [
  "Organize os campus, cursos e disciplinas como os fundamentos da sua estrutura acadêmica",
  "Crie novas grades curriculares a cada semestre ou reutilize grades em novas ofertas de curso",
  "Dê autonomia para seus professores realizarem chamadas e avaliações de maneira simples e efetiva",
  "Acompanhe o andamento de cada turma em detalhes, tanto das aulas quanto dos alunos",
  "Seus alunos terão acesso direto a agenda, notas, frequência e matrícula",
  "Envie notificações para todos os usuários dentro do próprio sistema",
]
