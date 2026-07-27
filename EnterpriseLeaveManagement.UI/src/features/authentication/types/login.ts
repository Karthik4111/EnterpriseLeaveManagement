export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    token: string;
    expiration: string;
    userId: string;
    email: string;
    roles: string[];
}