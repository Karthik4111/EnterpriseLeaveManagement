import type { DefaultOptions } from "@tanstack/react-query";

export const reactQueryOptions: DefaultOptions = {
    queries: {
        retry: 1,
        refetchOnWindowFocus: false,
        refetchOnReconnect: true,
        staleTime: 1000 * 60 * 5,
        gcTime: 1000 * 60 * 30,
    },
    mutations: {
        retry: 0,
    },
};