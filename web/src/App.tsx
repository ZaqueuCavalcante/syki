import { useNavigate } from 'react-router'
import { Button } from './components/ui/button'

function App() {
  const nav = useNavigate()

  return (
    <>
      <div className="flex min-h-svh flex-col items-center justify-center">
        <Button className="px-10 py-6 mt-6" onClick={() => nav("/login")}>
          LOGIN
        </Button>
      </div>
    </>
  )
}

export default App
