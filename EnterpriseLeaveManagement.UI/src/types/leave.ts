export interface LeaveRequest {
	id: string;
	employeeId: string;
	leaveTypeId: string;
	startDate: string;
	endDate: string;
	numberOfDays: number;
	leaveReason: string;
	status: number;
	approvedBy?: string | null;
	approvedOn?: string | null;
	managerComments?: string | null;
	attachmentPath?: string | null;
}

export interface LeaveType {
	id: string;
	name: string;
	code: string;
	description: string;
	defaultDays: number;
	isPaidLeave: boolean;
	carryForwardAllowed: boolean;
	maximumCarryForwardDays: number;
	requiresApproval: boolean;
	isActive: boolean;
}

export interface LeaveAllocation {
	id: string;
	employeeId: string;
	employeeName: string;
	leaveTypeId: string;
	leaveTypeName: string;
	year: number;
	allocatedDays: number;
}

export interface NotificationItem {
	id: string;
	title: string;
	message: string;
	isRead: boolean;
	createdOn: string;
}

export interface ApplyLeaveRequest {
	employeeId: string;
	leaveTypeId: string;
	startDate: string;
	endDate: string;
	leaveReason: string;
	attachmentPath?: string | null;
}

export interface ApproveLeaveRequest {
	managerComments?: string;
}

export interface RejectLeaveRequest {
	managerComments: string;
}

export const LEAVE_STATUS_LABELS: Record<number, string> = {
	1: "Pending",
	2: "Approved",
	3: "Rejected",
	4: "Cancelled",
};
