import axiosClient from "@/api/axiosConfig";
import { API_ROUTES } from "@/constants/apiRoutes";

export interface DashboardSummary {
	totalEmployees: number;
	totalDepartments: number;
	totalLeaveRequests: number;
	pendingRequests: number;
	approvedRequests: number;
	rejectedRequests: number;
	employeesOnLeaveToday: number;
}

const dashboardService = {
	async getSummary() {
		const response =
			await axiosClient.get<DashboardSummary>(
				API_ROUTES.DASHBOARD.SUMMARY
			);

		return response.data;
	},
};

export default dashboardService;
