import type { UUID } from "crypto";
import { createContext, useContext, useEffect, useState, type PropsWithChildren } from "react";

export interface User {
    id: UUID;
    name: string;
    email: string;
    role: 'Adm' | 'Academic' | 'Teacher' | 'Student';
}

type AuthContext = {
    user?: User | null,
    setUser: React.Dispatch<React.SetStateAction<User | null | undefined>>;
}

const AuthContext = createContext<AuthContext | undefined>(undefined);

type AuthProviderProps = PropsWithChildren;

export default function AuthProvider({ children }: AuthProviderProps) {
    const [user, setUser] = useState<User | null>();

    useEffect(() => {
        if (user !== undefined) {
            if (user) {
                localStorage.setItem('user', JSON.stringify(user));
            } else {
                localStorage.removeItem('user');
            }
        }
    }, [user]);

    useEffect(() => {
        const userAsString = localStorage.getItem('user');
        if (userAsString) {
            setUser(JSON.parse(userAsString) as User);
        } else {
            setUser(null);
        }
    }, []);

    return (
        <AuthContext.Provider value={{ user: user, setUser: setUser }}>
            {children}
        </AuthContext.Provider>
    )
}

export function useAuth() {
    const context = useContext(AuthContext);

    if (context === undefined) {
        throw new Error('Wrap the code with AuthProvider!')
    }

    return context;
}
