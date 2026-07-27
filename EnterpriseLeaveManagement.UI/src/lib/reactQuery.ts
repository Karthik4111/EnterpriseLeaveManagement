import { QueryClient } from "@tanstack/react-query";

import { reactQueryOptions } from "@/constants/reactQuery";

export const queryClient = new QueryClient({
    defaultOptions: reactQueryOptions,
});