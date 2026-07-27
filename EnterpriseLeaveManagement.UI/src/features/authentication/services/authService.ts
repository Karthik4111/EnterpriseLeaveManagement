import api from "@/api/interceptors";
import { API } from "@/constants/api";
import type {
    LoginRequest,
    LoginResponse,
    RefreshTokenRequest,
    RefreshTokenResponse,
    RegisterRequest,
} from "@/types";

class AuthService {
    async login(request: LoginRequest): Promise<LoginResponse> {
        const { data } = await api.post<LoginResponse>(
            API.Auth.Login,
            request
        );

        return data;
    }

    async register(request: RegisterRequest): Promise<void> {
        await api.post(API.Auth.Register, request);
    }

    async refreshToken(
        request: RefreshTokenRequest
    ): Promise<RefreshTokenResponse> {
        const { data } = await api.post<RefreshTokenResponse>(
            API.Auth.RefreshToken,
            request
        );

        return data;
    }
}

export const authService = new AuthService();