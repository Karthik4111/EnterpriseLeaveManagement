export interface EmployeeApiDto {
    id: string;
    employeeCode: string;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    designation: string;
    departmentName: string;
    isActive: boolean;
}

/** Payload for creating a new employee (maps to RegisterCommand). */
export interface CreateEmployeeApiRequest {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    password: string;
    confirmPassword: string;
    role: string;
    departmentId: string;
    employeeCode: string;
    phoneNumber: string;
    dateOfBirth?: string | null;
    dateOfJoining: string;
    designation: string;
}