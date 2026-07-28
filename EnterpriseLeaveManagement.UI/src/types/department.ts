export interface Department {
	id: string;
	name: string;
	code: string;
	description: string;
	isActive: boolean;
}

export interface DepartmentApiDto {
	id: string;
	name: string;
	code: string;
	description: string;
	isActive: boolean;
}

export interface CreateDepartmentRequest {
	name: string;
	code: string;
	description: string;
	managerId?: string | null;
}

export interface UpdateDepartmentRequest
	extends CreateDepartmentRequest {
	id: string;
}
