export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    accessToken: string;
    refreshToken: string;
    expiresAt: string;
}

export interface AuthUser {
    id: string;
    fullName: string;
    email: string;
    role: string;
}

export interface AuthState {
    isAuthenticated: boolean;
    accessToken: string | null;
    refreshToken: string | null;
    user: AuthUser | null;
}