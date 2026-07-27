import axios from "axios";
import { toast } from "react-toastify";

import api from "./axiosConfig";

const STORAGE_KEY = "auth";

let isRefreshing = false;
let refreshPromise: Promise<void> | null = null;

const TOAST_IDS = {
    badRequest: "bad-request",
    forbidden: "forbidden",
    notFound: "not-found",
    serverError: "server-error",
    networkError: "network-error",
};

async function refreshAccessToken() {
    const auth = localStorage.getItem(STORAGE_KEY);

    if (!auth) {
        throw new Error("Authentication not found.");
    }

    const {
        accessToken,
        refreshToken,
    } = JSON.parse(auth);

    const response = await axios.post(
        `${import.meta.env.VITE_API_BASE_URL}/Auth/refresh-token`,
        {
            accessToken,
            refreshToken,
        }
    );

    localStorage.setItem(
        STORAGE_KEY,
        JSON.stringify(response.data)
    );
}

api.interceptors.request.use(
    (config) => {
        const auth = localStorage.getItem(STORAGE_KEY);

        if (auth) {
            const { accessToken } = JSON.parse(auth);

            if (accessToken) {
                config.headers.Authorization = `Bearer ${accessToken}`;
            }
        }

        return config;
    },
    (error) => Promise.reject(error)
);

api.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;

        if (
            axios.isAxiosError(error) &&
            error.response?.status === 401 &&
            !originalRequest?._retry
        ) {
            originalRequest._retry = true;

            try {
                if (!isRefreshing) {
                    isRefreshing = true;

                    refreshPromise = refreshAccessToken();

                    await refreshPromise;

                    isRefreshing = false;
                    refreshPromise = null;
                } else if (refreshPromise) {
                    await refreshPromise;
                }

                const auth = localStorage.getItem(STORAGE_KEY);

                if (auth) {
                    const { accessToken } = JSON.parse(auth);

                    originalRequest.headers.Authorization =
                        `Bearer ${accessToken}`;
                }

                return api(originalRequest);
            } catch {
                localStorage.removeItem(STORAGE_KEY);

                window.location.href = "/login";
            }
        }

        if (axios.isAxiosError(error)) {
            switch (error.response?.status) {
                case 400:
                    toast.error(
                        error.response?.data?.message ??
                            "Bad request.",
                        {
                            toastId: TOAST_IDS.badRequest,
                        }
                    );
                    break;

                case 403:
                    toast.error("Access denied.", {
                        toastId: TOAST_IDS.forbidden,
                    });
                    break;

                case 404:
                    toast.error("Resource not found.", {
                        toastId: TOAST_IDS.notFound,
                    });
                    break;

                case 500:
                    toast.error("Internal server error.", {
                        toastId: TOAST_IDS.serverError,
                    });
                    break;

                default:
                    if (!error.response) {
                        toast.error(
                            "Unable to connect to the server.",
                            {
                                toastId:
                                    TOAST_IDS.networkError,
                            }
                        );
                    }
                    break;
            }
        }

        return Promise.reject(error);
    }
);

export default api;