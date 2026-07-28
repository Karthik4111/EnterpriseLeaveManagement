export const API_ROUTES = {
	AUTH: {
		LOGIN: "/Authentication/login",
		REFRESH_TOKEN: "/Authentication/refresh-token",
		REGISTER: "/Authentication/register",
	},
	DASHBOARD: {
		SUMMARY: "/Dashboard/summary",
		LEAVE_STATUS: "/Dashboard/leave-status",
	},
	EMPLOYEES: "/Employees",
	DEPARTMENTS: "/Departments",
	LEAVE_REQUESTS: "/LeaveRequests",
	LEAVE_TYPES: "/LeaveTypes",
	LEAVE_ALLOCATIONS: "/LeaveAllocations",
	NOTIFICATIONS: "/Notifications",
} as const;
