export const ROUTES = {
    ROOT: "/",

    LOGIN: "/login",

    DASHBOARD: "/dashboard",

    EMPLOYEES: "/employees",
    CREATE_EMPLOYEE: "/employees/create",
    EDIT_EMPLOYEE: "/employees/:id/edit",
    EMPLOYEE_DETAILS: "/employees/:id",

    DEPARTMENTS: "/departments",
    CREATE_DEPARTMENT: "/departments/create",
    EDIT_DEPARTMENT: "/departments/:id/edit",

    LEAVE_REQUEST: "/leave/request",
    MY_LEAVES: "/leave/my-leaves",
    LEAVE_APPROVALS: "/leave/approvals",
    LEAVE_BALANCE: "/leave/balance",

    PROFILE: "/profile",

    UNAUTHORIZED: "/unauthorized",

    NOT_FOUND: "*",
} as const;