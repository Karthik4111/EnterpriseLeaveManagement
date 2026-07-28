import axiosClient from "@/api/axiosConfig";

import type {
    EmployeeApiDto,
} from "@/types/employeeApi";
import type { PagedResult } from "@/types/api";
import type { Employee } from "@/types/employee";

function mapEmployee(apiEmployee: EmployeeApiDto): Employee {
    return {
        id: apiEmployee.id,
        employeeCode: apiEmployee.employeeCode,
        fullName: `${apiEmployee.firstName} ${apiEmployee.lastName}`.trim(),
        department: apiEmployee.departmentName,
        designation: apiEmployee.designation,
        email: apiEmployee.email,
    };
}

const employeeService = {
    async getAll() {
        const response =
            await axiosClient.get<PagedResult<EmployeeApiDto>>(
                "/Employees"
            );

        return response.data.items.map(mapEmployee);
    },
};

export default employeeService;