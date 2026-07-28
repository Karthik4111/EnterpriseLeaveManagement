import axiosClient from "@/api/axiosConfig";
import { API_ROUTES } from "@/constants/apiRoutes";

import type { PagedResult } from "@/types/api";
import type {
	ApplyLeaveRequest,
	ApproveLeaveRequest,
	LeaveAllocation,
	LeaveRequest,
	LeaveType,
	NotificationItem,
	RejectLeaveRequest,
} from "@/types/leave";

const leaveService = {
	async getRequests() {
		const response =
			await axiosClient.get<
				PagedResult<LeaveRequest>
			>(API_ROUTES.LEAVE_REQUESTS);

		return response.data.items;
	},

	async apply(request: ApplyLeaveRequest) {
		const response =
			await axiosClient.post<string>(
				API_ROUTES.LEAVE_REQUESTS,
				request
			);

		return response.data;
	},

	async cancel(id: string) {
		await axiosClient.put(
			`${API_ROUTES.LEAVE_REQUESTS}/${id}/cancel`,
			{}
		);
	},

	async approve(
		id: string,
		request: ApproveLeaveRequest
	) {
		await axiosClient.put(
			`${API_ROUTES.LEAVE_REQUESTS}/${id}/approve`,
			request
		);
	},

	async reject(
		id: string,
		request: RejectLeaveRequest
	) {
		await axiosClient.put(
			`${API_ROUTES.LEAVE_REQUESTS}/${id}/reject`,
			request
		);
	},

	async getTypes() {
		const response =
			await axiosClient.get<
				PagedResult<LeaveType>
			>(API_ROUTES.LEAVE_TYPES);

		return response.data.items;
	},

	async getAllocations() {
		const response =
			await axiosClient.get<
				LeaveAllocation[]
			>(API_ROUTES.LEAVE_ALLOCATIONS);

		return response.data;
	},

	async getNotifications() {
		const response =
			await axiosClient.get<
				NotificationItem[]
			>(API_ROUTES.NOTIFICATIONS);

		return response.data;
	},
};

export default leaveService;
