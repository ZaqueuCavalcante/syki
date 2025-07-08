import { useNavigate } from "react-router"
import { Button } from "@/components/ui/button"

export default function NotFoundPage() {
  const nav = useNavigate()

  return (
    <main className="flex justify-center pt-[8%] px-2">
      <div className="grid items-center gap-3">
        <h1>404 NOT FOUND</h1>
        <Button onClick={() => nav("/")}>Home</Button>
      </div>
    </main>
  )
}
