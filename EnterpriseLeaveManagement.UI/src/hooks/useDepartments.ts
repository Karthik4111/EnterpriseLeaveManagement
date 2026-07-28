import {
	useMutation,
	useQuery,
	useQueryClient,
} from "@tanstack/react-query";

import departmentService from "@/services/departmentService";
import type {
	CreateDepartmentRequest,
	UpdateDepartmentRequest,
} from "@/types/department";

const DEPARTMENT_QUERY_KEY = [
	"departments",
] as const;

export function useDepartments() {
	return useQuery({
		queryKey: DEPARTMENT_QUERY_KEY,
		queryFn: departmentService.getAll,
	});
}

export function useCreateDepartment() {
	const queryClient = useQueryClient();

	return useMutation({
		mutationFn: (
			request: CreateDepartmentRequest
		) => departmentService.create(request),
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: DEPARTMENT_QUERY_KEY,
			});
		},
	});
}

export function useUpdateDepartment() {
	const queryClient = useQueryClient();

	return useMutation({
		mutationFn: (
			request: UpdateDepartmentRequest
		) => departmentService.update(request),
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: DEPARTMENT_QUERY_KEY,
			});
		},
	});
}

export function useDeleteDepartment() {
	const queryClient = useQueryClient();

	return useMutation({
		mutationFn: (id: string) =>
			departmentService.remove(id),
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: DEPARTMENT_QUERY_KEY,
			});
		},
	});
}
