export const API = {
    Auth: {
        Login: '/Authentication/login',
        Register: '/Authentication/register',
        RefreshToken: '/Authentication/refresh-token',
    },

    Dashboard: {
        Summary: '/Dashboard/summary',
    },

    Employees: {
        Base: '/Employees',
    },

    Departments: {
        Base: '/Departments',
    },

    LeaveTypes: {
        Base: '/LeaveTypes',
    },

    LeaveRequests: {
        Base: '/LeaveRequests',
    },

    LeaveAllocations: {
        Base: '/LeaveAllocations',
    },

    Notifications: {
        Base: '/Notifications',
    },

    AuditLogs: {
        Base: '/AuditLogs',
    },

    FileUploads: {
        Base: '/FileUploads',
    },
} as const;