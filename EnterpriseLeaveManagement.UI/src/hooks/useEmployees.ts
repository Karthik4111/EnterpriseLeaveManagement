import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

import employeeService from "@/services/employeeService";

import type { Employee } from "@/types/employee";
import type { CreateEmployeeApiRequest } from "@/types/employeeApi";

const EMPLOYEE_QUERY_KEY = ["employees"] as const;

export function useEmployees() {
    return useQuery<Employee[]>({
        queryKey: EMPLOYEE_QUERY_KEY,
        queryFn: employeeService.getAll,
    });
}

export function useCreateEmployee() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (req: CreateEmployeeApiRequest) =>
            employeeService.create(req),
        onSuccess: () => {
            queryClient.invalidateQueries({
                queryKey: EMPLOYEE_QUERY_KEY,
            });
        },
    });
}