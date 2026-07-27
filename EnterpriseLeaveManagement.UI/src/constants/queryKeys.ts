export const queryKeys = {
    auth: {
        profile: ["auth", "profile"] as const,
    },

    employees: {
        all: ["employees"] as const,
        details: (id: string) =>
            ["employees", id] as const,
    },

    leaveTypes: {
        all: ["leave-types"] as const,
        details: (id: string) =>
            ["leave-types", id] as const,
    },

    leaveRequests: {
        all: ["leave-requests"] as const,
        details: (id: string) =>
            ["leave-requests", id] as const,
    },

    holidays: {
        all: ["holidays"] as const,
    },

    departments: {
        all: ["departments"] as const,
    },

    designations: {
        all: ["designations"] as const,
    },

    roles: {
        all: ["roles"] as const,
    },

    dashboard: {
        summary: ["dashboard", "summary"] as const,
    },
};