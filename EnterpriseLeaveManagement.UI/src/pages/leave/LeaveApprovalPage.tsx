import { useMemo, useState } from "react";

import {
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	Stack,
	TextField,
	Typography,
} from "@mui/material";

import PageHeader from "@/components/common/PageHeader/PageHeader";
import DataTable, {
	type DataColumn,
} from "@/components/common/DataTable/DataTable";

import {
	useApproveLeave,
	useLeaveRequests,
	useLeaveTypes,
	useRejectLeave,
} from "@/hooks/useLeave";
import useSnackbar from "@/hooks/useSnackbar";
import type { LeaveRequest } from "@/types/leave";

export default function LeaveApprovalPage() {
	const { showError, showSuccess } = useSnackbar();
	const { data: leaveRequests = [], isLoading } =
		useLeaveRequests();
	const { data: leaveTypes = [] } = useLeaveTypes();

	const { mutateAsync: approveLeave, isPending: isApproving } =
		useApproveLeave();

	const { mutateAsync: rejectLeave, isPending: isRejecting } =
		useRejectLeave();

	const [selectedLeave, setSelectedLeave] =
		useState<LeaveRequest | null>(null);
	const [comment, setComment] = useState("");
	const [dialogMode, setDialogMode] = useState<"approve" | "reject" | null>(null);

	const pendingRequests = useMemo(
		() => leaveRequests.filter((item) => item.status === 1),
		[leaveRequests]
	);

	const leaveTypeMap = useMemo(
		() =>
			new Map(
				leaveTypes.map((type) => [
					type.id,
					type.name,
				])
			),
		[leaveTypes]
	);

	const openDialog = (
		leave: LeaveRequest,
		mode: "approve" | "reject"
	) => {
		setSelectedLeave(leave);
		setDialogMode(mode);
		setComment("");
	};

	const closeDialog = () => {
		setSelectedLeave(null);
		setDialogMode(null);
		setComment("");
	};

	const handleSubmit = async () => {
		if (!selectedLeave || !dialogMode) {
			return;
		}

		try {
			if (dialogMode === "approve") {
				await approveLeave({
					id: selectedLeave.id,
					request: {
						managerComments:
							comment.trim() || undefined,
					},
				});

				showSuccess("Leave request approved.");
			} else {
				if (!comment.trim()) {
					showError("Rejection comment is required.");
					return;
				}

				await rejectLeave({
					id: selectedLeave.id,
					request: {
						managerComments: comment.trim(),
					},
				});

				showSuccess("Leave request rejected.");
			}

			closeDialog();
		} catch {
			showError("Unable to process leave request.");
		}
	};

	const columns: DataColumn<LeaveRequest>[] = [
		{
			field: "employeeId",
			headerName: "Employee Id",
		},
		{
			field: "leaveTypeId",
			headerName: "Leave Type",
			render: (row) =>
				leaveTypeMap.get(row.leaveTypeId) ?? "Unknown",
		},
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
			field: "numberOfDays",
			headerName: "Days",
		},
		{
			field: "leaveReason",
			headerName: "Reason",
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
						variant="contained"
						color="success"
						onClick={() =>
							openDialog(row, "approve")
						}
					>
						Approve
					</Button>
					<Button
						size="small"
						variant="contained"
						color="error"
						onClick={() =>
							openDialog(row, "reject")
						}
					>
						Reject
					</Button>
				</Stack>
			),
		},
	];

	return (
		<>
			<PageHeader
				title="Leave Approvals"
				subtitle="Review and process pending leave requests."
			/>

			{pendingRequests.length === 0 && !isLoading && (
				<Typography sx={{ mb: 2 }} color="text.secondary">
					No pending leave requests.
				</Typography>
			)}

			<DataTable
				columns={columns}
				rows={pendingRequests}
				loading={isLoading}
			/>

			<Dialog
				open={Boolean(selectedLeave && dialogMode)}
				onClose={
					isApproving || isRejecting
						? undefined
						: closeDialog
				}
				fullWidth
				maxWidth="sm"
			>
				<DialogTitle>
					{dialogMode === "approve"
						? "Approve Leave"
						: "Reject Leave"}
				</DialogTitle>

				<DialogContent>
					<TextField
						fullWidth
						multiline
						minRows={4}
						label="Manager Comments"
						value={comment}
						onChange={(event) =>
							setComment(event.target.value)
						}
						sx={{ mt: 1 }}
					/>
				</DialogContent>

				<DialogActions>
					<Button
						onClick={closeDialog}
						disabled={isApproving || isRejecting}
					>
						Cancel
					</Button>

					<Button
						variant="contained"
						color={
							dialogMode === "approve"
								? "success"
								: "error"
						}
						onClick={handleSubmit}
						disabled={isApproving || isRejecting}
					>
						{isApproving || isRejecting
							? "Saving..."
							: "Confirm"}
					</Button>
				</DialogActions>
			</Dialog>
		</>
	);
}
