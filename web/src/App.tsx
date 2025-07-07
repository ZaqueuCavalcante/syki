import './App.css'
import { useState } from 'react'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <div className="card">
        <button onClick={() => setCount((x) => x + 1)}>
          <h3>
            {count} clicks
          </h3>
        </button>
      </div>
    </>
  )
}

export default App
