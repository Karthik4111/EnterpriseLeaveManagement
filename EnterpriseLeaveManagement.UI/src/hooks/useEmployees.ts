import { useQuery } from "@tanstack/react-query";

import employeeService from "@/services/employeeService";

import type { Employee } from "@/types/employee";

const EMPLOYEE_QUERY_KEY = ["employees"] as const;

export function useEmployees() {
    return useQuery<Employee[]>({
        queryKey: EMPLOYEE_QUERY_KEY,
        queryFn: employeeService.getAll,
    });
}