export const ROLES = {
	ADMIN: "Admin",
	MANAGER: "Manager",
	EMPLOYEE: "Employee",
} as const;

export type Role =
	(typeof ROLES)[keyof typeof ROLES];
