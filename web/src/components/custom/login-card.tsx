'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';

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

export function LoginCard() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const router = useRouter();

  const handleSubmit = async () => {
    setLoading(true);
    setError(null); // Clear any previous errors

    try {
      // Replace with your actual ASP.NET login API endpoint
      const apiUrl = process.env.NEXT_PUBLIC_API_URL;
      const response = await fetch(`${apiUrl}/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
      });

      if (response.ok) {
        // Redirect to a protected dashboard or home page
        router.push('/dashboard'); // Or whatever your protected route is
      } else {
        const errorData = await response.json(); // Assuming your API sends error messages in JSON
        setError(errorData.message || 'Credenciais inválidas. Por favor, tente novamente.');
        console.error('Login failed:', response.status, errorData);
      }
    } catch (err) {
      console.error('Network error during login:', err);
      setError('Ocorreu um erro de rede. Por favor, tente novamente mais tarde.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Card className="w-full max-w-sm">
      <CardHeader>
        <CardTitle>Faça login na sua conta</CardTitle>
        <CardDescription>
          Insira duas credenciais abaixo para realizar o login
        </CardDescription>
      </CardHeader>
      <CardContent>
        <form>
          <div className="flex flex-col gap-6">
            <div className="grid gap-2">
              <Label htmlFor="email">Email</Label>
              <Input
                id="email"
                type="email"
                placeholder="zezinho@gmail.com"
                required
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>
            <div className="grid gap-2">
              <div className="flex items-center">
                <Label htmlFor="password">Password</Label>
                <a
                  href="#"
                  className="ml-auto inline-block text-sm underline-offset-4 hover:underline"
                >
                  Esqueceu sua senha?
                </a>
              </div>
              <Input
                id="password"
                type="password"
                required
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>
          </div>
        </form>
      </CardContent>
      {error && <p className="text-red-500 text-sm pl-8">{error}</p>}
      <CardFooter className="flex-col gap-6">
        <Button type="button" onClick={handleSubmit} className="w-full" disabled={loading}>
          {loading ? 'Entrando...' : 'Login'}
        </Button>
      </CardFooter>
    </Card>
  )
}
