import api from "@/api/axios";

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
			await api.get<DashboardSummary>(
				"/Dashboard/summary"
			);

		return response.data;
	},
};

export default dashboardService;
