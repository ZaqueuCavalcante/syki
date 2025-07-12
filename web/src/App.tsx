import { useNavigate } from 'react-router'
import { Button } from './components/ui/button'
import Layout from './components/custom/Layout'
import { useAuth } from './components/custom/AuthProvider';

function App() {
  const nav = useNavigate()
  const { user } = useAuth();

  return (
    <>
      {user ? (
        <Layout>
          <h1>Bem-vindo ao seu Dashboard!</h1>
          <p>Aqui você encontrará suas informações e ferramentas.</p>
        </Layout>
      ) : (
        <div className="flex min-h-svh flex-col items-center justify-center">
          <Button className="px-10 py-6 mt-6" onClick={() => nav("/login")}>
            LOGIN
          </Button>
        </div>
      )}
    </>
  )
}

export default App
