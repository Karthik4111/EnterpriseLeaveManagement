import api from "@/api/axiosConfig";
import type { LoginRequest, LoginResponse } from "../types/login";

export const login = async (
    request: LoginRequest
): Promise<LoginResponse> => {

    const response = await api.post<LoginResponse>(
        "/authentication/login",
        request
    );

    return response.data;
};