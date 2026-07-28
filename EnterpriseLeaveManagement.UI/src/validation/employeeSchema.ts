import { z } from "zod";

export const employeeSchema = z.object({
    employeeCode: z
        .string()
        .trim()
        .min(1, "Employee Code is required"),

    fullName: z
        .string()
        .trim()
        .min(3, "Full Name must be at least 3 characters"),

    department: z
        .string()
        .trim()
        .min(1, "Department is required"),

    designation: z
        .string()
        .trim()
        .min(1, "Designation is required"),

    email: z
        .string()
        .trim()
        .email("Please enter a valid email address"),
});

export type EmployeeFormData =
    z.infer<typeof employeeSchema>;