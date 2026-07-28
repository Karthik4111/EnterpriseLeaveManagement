import axiosClient from "@/api/axiosConfig";

import type {
    LoginRequest,
    LoginResponse,
} from "@/types/auth";

const AUTH_URL = "/Authentication";

class AuthService {
    async login(request: LoginRequest) {
        const response =
            await axiosClient.post<LoginResponse>(
                `${AUTH_URL}/login`,
                request
            );

        return response.data;
    }

    async logout() {}

    async refreshToken(
        refreshToken: string
    ) {
        const response =
            await axiosClient.post<LoginResponse>(
                `${AUTH_URL}/refresh-token`,
                {
                    refreshToken,
                }
            );

        return response.data;
    }
}

export default new AuthService();