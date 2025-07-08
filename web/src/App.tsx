import { useState } from 'react'
import { useNavigate } from 'react-router'
import { Button } from './components/ui/button'

function App() {
  const nav = useNavigate()
  const [count, setCount] = useState(0)

  return (
    <>
      <div className="flex min-h-svh flex-col items-center justify-center">
        <Button className="px-10 py-6" onClick={() => setCount((x) => x + 1)}>
          {count} clicks
        </Button>

        <Button className="px-10 py-6 mt-6" onClick={() => nav("/login")}>
          LOGIN
        </Button>
      </div>
    </>
  )
}

export default App
