export interface AuthUser {
    accessToken: string;
    refreshToken: string;
    expiresAt: string;
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse extends AuthUser {}

export interface RegisterRequest {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
}

export interface RefreshTokenRequest {
    accessToken: string;
    refreshToken: string;
}

export interface RefreshTokenResponse extends AuthUser {}