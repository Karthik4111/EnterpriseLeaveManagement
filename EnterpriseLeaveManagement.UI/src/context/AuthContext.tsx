import type { PropsWithChildren } from "react";
import {
    createContext,
    useContext,
    useMemo,
    useState,
} from "react";

import type {
    AuthState,
    AuthUser,
} from "@/types/auth";

import { tokenStorage } from "@/utils/tokenStorage";

interface AuthContextType {
    auth: AuthState;

    login: (
        accessToken: string,
        refreshToken: string
    ) => void;

    logout: () => void;
}

const AuthContext = createContext<AuthContextType | null>(null);

function decodeBase64Url(value: string): string {
    const normalized = value
        .replace(/-/g, "+")
        .replace(/_/g, "/");

    const padding = normalized.length % 4;
    const padded =
        padding === 0
            ? normalized
            : normalized + "=".repeat(4 - padding);

    return atob(padded);
}

function parseJwtPayload(token: string): Record<string, unknown> | null {
    const parts = token.split(".");

    if (parts.length < 2) {
        return null;
    }

    try {
        const payload = decodeBase64Url(parts[1]);
        return JSON.parse(payload) as Record<string, unknown>;
    } catch {
        return null;
    }
}

function getClaim(
    payload: Record<string, unknown>,
    keys: string[]
): string {
    for (const key of keys) {
        const value = payload[key];

        if (typeof value === "string" && value.trim().length > 0) {
            return value;
        }
    }

    return "";
}

function mapTokenToUser(accessToken: string): AuthUser | null {
    const payload = parseJwtPayload(accessToken);

    if (!payload) {
        return null;
    }

    const roleClaim = payload[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    ];

    const role = Array.isArray(roleClaim)
        ? String(roleClaim[0] ?? "")
        : String(roleClaim ?? payload.role ?? "");

    const id = getClaim(payload, [
        "sub",
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
    ]);

    const fullName = getClaim(payload, [
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
        "unique_name",
        "name",
    ]);

    const email = getClaim(payload, [
        "email",
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",
    ]);

    return {
        id,
        fullName: fullName || email || "User",
        email,
        role,
    };
}

function createAuthStateFromStorage(): AuthState {
    const accessToken = tokenStorage.getAccessToken();
    const refreshToken = tokenStorage.getRefreshToken();

    if (!accessToken || !refreshToken) {
        return {
            isAuthenticated: false,
            accessToken: null,
            refreshToken: null,
            user: null,
        };
    }

    const user = mapTokenToUser(accessToken);

    if (!user) {
        tokenStorage.clear();

        return {
            isAuthenticated: false,
            accessToken: null,
            refreshToken: null,
            user: null,
        };
    }

    return {
        isAuthenticated: true,
        accessToken,
        refreshToken,
        user,
    };
}

export function AuthProvider({
    children,
}: PropsWithChildren) {
    const [auth, setAuth] = useState<AuthState>(
        createAuthStateFromStorage
    );

    const login = (
        accessToken: string,
        refreshToken: string
    ) => {
        const user = mapTokenToUser(accessToken);

        if (!user) {
            throw new Error("Unable to parse access token.");
        }

        tokenStorage.setAccessToken(accessToken);
        tokenStorage.setRefreshToken(refreshToken);

        setAuth({
            isAuthenticated: true,
            accessToken,
            refreshToken,
            user,
        });
    };

    const logout = () => {
        tokenStorage.clear();

        setAuth({
            isAuthenticated: false,
            accessToken: null,
            refreshToken: null,
            user: null,
        });
    };

    const value = useMemo(
        () => ({
            auth,
            login,
            logout,
        }),
        [auth]
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
        throw new Error(
            "useAuth must be used inside AuthProvider."
        );
    }

    return context;
}