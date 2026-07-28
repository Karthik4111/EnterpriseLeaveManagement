import axiosClient from "./axiosConfig";
import { tokenStorage } from "@/utils/tokenStorage";

axiosClient.interceptors.request.use((config) => {
    const token = tokenStorage.getAccessToken();

    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
});

axiosClient.interceptors.response.use(
    (response) => response,

    async (error) => {
        if (error.response?.status === 401) {
            tokenStorage.clear();

            if (window.location.pathname !== "/login") {
                window.location.replace("/login");
            }
        }

        return Promise.reject(error);
    }
);

export default axiosClient;