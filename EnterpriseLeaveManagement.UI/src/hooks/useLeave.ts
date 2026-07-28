import {
	useMutation,
	useQuery,
	useQueryClient,
} from "@tanstack/react-query";

import leaveService from "@/services/leaveService";
import type {
	ApplyLeaveRequest,
	ApproveLeaveRequest,
	RejectLeaveRequest,
} from "@/types/leave";

const LEAVE_REQUESTS_QUERY_KEY = [
	"leave-requests",
] as const;

const LEAVE_TYPES_QUERY_KEY = [
	"leave-types",
] as const;

const LEAVE_ALLOCATIONS_QUERY_KEY = [
	"leave-allocations",
] as const;

const NOTIFICATIONS_QUERY_KEY = [
	"notifications",
] as const;

export function useLeaveRequests() {
	return useQuery({
		queryKey: LEAVE_REQUESTS_QUERY_KEY,
		queryFn: leaveService.getRequests,
	});
}

export function useLeaveTypes() {
	return useQuery({
		queryKey: LEAVE_TYPES_QUERY_KEY,
		queryFn: leaveService.getTypes,
	});
}

export function useLeaveAllocations() {
	return useQuery({
		queryKey: LEAVE_ALLOCATIONS_QUERY_KEY,
		queryFn: leaveService.getAllocations,
	});
}

export function useNotifications() {
	return useQuery({
		queryKey: NOTIFICATIONS_QUERY_KEY,
		queryFn: leaveService.getNotifications,
		staleTime: 1000 * 60,
	});
}

export function useApplyLeave() {
	const queryClient = useQueryClient();

	return useMutation({
		mutationFn: (
			request: ApplyLeaveRequest
		) => leaveService.apply(request),
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: LEAVE_REQUESTS_QUERY_KEY,
			});
			queryClient.invalidateQueries({
				queryKey: LEAVE_ALLOCATIONS_QUERY_KEY,
			});
		},
	});
}

export function useCancelLeave() {
	const queryClient = useQueryClient();

	return useMutation({
		mutationFn: (id: string) =>
			leaveService.cancel(id),
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: LEAVE_REQUESTS_QUERY_KEY,
			});
		},
	});
}

export function useApproveLeave() {
	const queryClient = useQueryClient();

	return useMutation({
		mutationFn: ({
			id,
			request,
		}: {
			id: string;
			request: ApproveLeaveRequest;
		}) => leaveService.approve(id, request),
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: LEAVE_REQUESTS_QUERY_KEY,
			});
		},
	});
}

export function useRejectLeave() {
	const queryClient = useQueryClient();

	return useMutation({
		mutationFn: ({
			id,
			request,
		}: {
			id: string;
			request: RejectLeaveRequest;
		}) => leaveService.reject(id, request),
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: LEAVE_REQUESTS_QUERY_KEY,
			});
		},
	});
}
