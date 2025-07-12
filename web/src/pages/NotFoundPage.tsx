
import { useNavigate } from 'react-router'
import Layout from '@/components/custom/Layout';
import { Button } from '@/components/ui/button';
import { useAuth } from '@/components/custom/AuthProvider';

export default function NotFoundPage() {
  const nav = useNavigate()
  const { user } = useAuth();

  return (
    <>
      {user ? (
        <Layout>
          <main className="flex justify-center pt-[8%] px-2">
            <div className="grid items-center gap-3">
              <h1>404 NOT FOUND</h1>
              <Button onClick={() => nav("/")}>Home</Button>
            </div>
          </main>
        </Layout>
      ) : (
        <main className="flex justify-center pt-[8%] px-2">
          <div className="grid items-center gap-3">
            <h1>404 NOT FOUND</h1>
            <Button onClick={() => nav("/")}>Home</Button>
          </div>
        </main>
      )}
    </>
  )
}
