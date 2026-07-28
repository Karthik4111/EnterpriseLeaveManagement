import { useMemo } from "react";

import {
	Button,
	Chip,
	Stack,
	Typography,
} from "@mui/material";

import PageHeader from "@/components/common/PageHeader/PageHeader";
import DataTable, {
	type DataColumn,
} from "@/components/common/DataTable/DataTable";

import {
	useCancelLeave,
	useLeaveRequests,
	useLeaveTypes,
} from "@/hooks/useLeave";
import useAuth from "@/hooks/useAuth";
import useSnackbar from "@/hooks/useSnackbar";
import {
	LEAVE_STATUS_LABELS,
	type LeaveRequest,
} from "@/types/leave";

function getStatusColor(status: number) {
	switch (status) {
		case 2:
			return "success" as const;
		case 3:
			return "error" as const;
		case 4:
			return "default" as const;
		default:
			return "warning" as const;
	}
}

export default function MyLeavesPage() {
	const { auth } = useAuth();
	const { showError, showSuccess } = useSnackbar();

	const { data: leaveRequests = [], isLoading } =
		useLeaveRequests();

	const { data: leaveTypes = [] } = useLeaveTypes();

	const { mutateAsync: cancelLeave, isPending } =
		useCancelLeave();

	const leaveTypeMap = useMemo(() => {
		return new Map(
			leaveTypes.map((type) => [
				type.id,
				type.name,
			])
		);
	}, [leaveTypes]);

	const myLeaveRequests = useMemo(() => {
		if (!auth.user?.id) {
			return [];
		}

		return leaveRequests.filter(
			(leave) => leave.employeeId === auth.user?.id
		);
	}, [leaveRequests, auth.user?.id]);

	const handleCancel = async (id: string) => {
		try {
			await cancelLeave(id);
			showSuccess("Leave request cancelled.");
		} catch {
			showError("Unable to cancel leave request.");
		}
	};

	const columns: DataColumn<LeaveRequest>[] = [
		{
			field: "startDate",
			headerName: "Start Date",
			render: (row) =>
				new Date(row.startDate).toLocaleDateString(),
		},
		{
			field: "endDate",
			headerName: "End Date",
			render: (row) =>
				new Date(row.endDate).toLocaleDateString(),
		},
		{
			field: "leaveTypeId",
			headerName: "Leave Type",
			render: (row) =>
				leaveTypeMap.get(row.leaveTypeId) ?? "Unknown",
		},
		{
			field: "numberOfDays",
			headerName: "Days",
		},
		{
			field: "status",
			headerName: "Status",
			render: (row) => (
				<Chip
					size="small"
					label={
						LEAVE_STATUS_LABELS[row.status] ??
						"Unknown"
					}
					color={getStatusColor(row.status)}
				/>
			),
		},
		{
			field: "id",
			headerName: "Actions",
			sortable: false,
			align: "center",
			render: (row) => (
				<Stack direction="row" spacing={1}>
					<Button
						size="small"
						variant="outlined"
						color="error"
						disabled={
							row.status !== 1 || isPending
						}
						onClick={() =>
							handleCancel(row.id)
						}
					>
						Cancel
					</Button>
				</Stack>
			),
		},
	];

	return (
		<>
			<PageHeader
				title="My Leaves"
				subtitle="Track your leave requests and statuses."
			/>

			{!auth.user?.id && (
				<Typography color="warning.main" sx={{ mb: 2 }}>
					Employee id is not available in your token. Leave list cannot be filtered.
				</Typography>
			)}

			<DataTable
				columns={columns}
				rows={myLeaveRequests}
				loading={isLoading}
			/>
		</>
	);
}
