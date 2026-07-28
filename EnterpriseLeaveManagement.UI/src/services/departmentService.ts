import axiosClient from "@/api/axiosConfig";
import { API_ROUTES } from "@/constants/apiRoutes";

import type { PagedResult } from "@/types/api";
import type {
	CreateDepartmentRequest,
	Department,
	DepartmentApiDto,
	UpdateDepartmentRequest,
} from "@/types/department";

function mapDepartment(
	department: DepartmentApiDto
): Department {
	return {
		id: department.id,
		name: department.name,
		code: department.code,
		description: department.description,
		isActive: department.isActive,
	};
}

const departmentService = {
	async getAll() {
		const response =
			await axiosClient.get<
				PagedResult<DepartmentApiDto>
			>(API_ROUTES.DEPARTMENTS);

		return response.data.items.map(
			mapDepartment
		);
	},

	async create(
		request: CreateDepartmentRequest
	) {
		await axiosClient.post(
			API_ROUTES.DEPARTMENTS,
			request
		);
	},

	async update(
		request: UpdateDepartmentRequest
	) {
		await axiosClient.put(
			`${API_ROUTES.DEPARTMENTS}/${request.id}`,
			request
		);
	},

	async remove(id: string) {
		await axiosClient.delete(
			`${API_ROUTES.DEPARTMENTS}/${id}`
		);
	},
};

export default departmentService;
