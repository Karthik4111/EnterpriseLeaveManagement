import {
    createContext,
    useContext,
    useEffect,
    useMemo,
    useState,
    type ReactNode,
} from "react";

import type { AuthUser } from "@/types";

export interface AuthContextType {
    user: AuthUser | null;
    isAuthenticated: boolean;
    login: (user: AuthUser) => void;
    logout: () => void;
}

export const AuthContext = createContext<AuthContextType | undefined>(
    undefined
);

interface Props {
    children: ReactNode;
}

const STORAGE_KEY = "auth";

export function AuthProvider({ children }: Props) {
    const [user, setUser] = useState<AuthUser | null>(null);

    useEffect(() => {
        const stored = localStorage.getItem(STORAGE_KEY);

        if (!stored) {
            return;
        }

        const auth = JSON.parse(stored) as AuthUser;

        if (new Date(auth.expiresAt) <= new Date()) {
            localStorage.removeItem(STORAGE_KEY);
            return;
        }

        setUser(auth);
    }, []);

    const login = (user: AuthUser) => {
        localStorage.setItem(STORAGE_KEY, JSON.stringify(user));
        setUser(user);
    };

    const logout = () => {
        localStorage.removeItem(STORAGE_KEY);
        setUser(null);
    };

    const value = useMemo<AuthContextType>(
        () => ({
            user,
            isAuthenticated: user !== null,
            login,
            logout,
        }),
        [user]
    );

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    const context = useContext(AuthContext);

    if (!context) {
        throw new Error("useAuth must be used within AuthProvider.");
    }

    return context;
}