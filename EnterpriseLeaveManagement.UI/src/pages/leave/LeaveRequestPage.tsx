import { useState } from "react";
import {
	Box,
	Button,
	Card,
	CardContent,
	Grid,
	MenuItem,
	Stack,
	TextField,
	Typography,
} from "@mui/material";

import PageHeader from "@/components/common/PageHeader/PageHeader";
import {
	useApplyLeave,
	useLeaveTypes,
} from "@/hooks/useLeave";
import useAuth from "@/hooks/useAuth";
import useSnackbar from "@/hooks/useSnackbar";

interface LeaveRequestForm {
	leaveTypeId: string;
	startDate: string;
	endDate: string;
	leaveReason: string;
}

const initialForm: LeaveRequestForm = {
	leaveTypeId: "",
	startDate: "",
	endDate: "",
	leaveReason: "",
};

export default function LeaveRequestPage() {
	const { auth } = useAuth();
	const { showError, showSuccess } = useSnackbar();

	const { data: leaveTypes = [], isLoading } =
		useLeaveTypes();

	const { mutateAsync: applyLeave, isPending } =
		useApplyLeave();

	const [form, setForm] =
		useState<LeaveRequestForm>(initialForm);

	const handleSubmit = async () => {
		if (!auth.user?.id) {
			showError(
				"User id is missing from token. Please login again."
			);
			return;
		}

		if (
			!form.leaveTypeId ||
			!form.startDate ||
			!form.endDate ||
			!form.leaveReason.trim()
		) {
			showError("All fields are required.");
			return;
		}

		if (form.startDate > form.endDate) {
			showError("End date must be on or after start date.");
			return;
		}

		try {
			await applyLeave({
				employeeId: auth.user.id,
				leaveTypeId: form.leaveTypeId,
				startDate: form.startDate,
				endDate: form.endDate,
				leaveReason: form.leaveReason.trim(),
				attachmentPath: null,
			});

			showSuccess("Leave request submitted.");
			setForm(initialForm);
		} catch {
			showError("Unable to submit leave request.");
		}
	};

	return (
		<>
			<PageHeader
				title="Request Leave"
				subtitle="Submit a new leave request."
			/>

			<Card sx={{ maxWidth: 850 }}>
				<CardContent>
					<Stack spacing={2.5}>
						<Grid container spacing={2}>
							<Grid size={{ xs: 12, md: 6 }}>
								<TextField
									select
									fullWidth
									label="Leave Type"
									value={form.leaveTypeId}
									onChange={(event) =>
										setForm((previous) => ({
											...previous,
											leaveTypeId: event.target.value,
										}))
									}
									disabled={isLoading}
								>
									{leaveTypes
										.filter((type) => type.isActive)
										.map((type) => (
											<MenuItem
												key={type.id}
												value={type.id}
											>
												{type.name}
											</MenuItem>
										))}
								</TextField>
							</Grid>

							<Grid size={{ xs: 12, md: 3 }}>
								<TextField
									fullWidth
									type="date"
									label="Start Date"
									value={form.startDate}
									onChange={(event) =>
										setForm((previous) => ({
											...previous,
											startDate: event.target.value,
										}))
									}
									slotProps={{
										inputLabel: {
											shrink: true,
										},
									}}
								/>
							</Grid>

							<Grid size={{ xs: 12, md: 3 }}>
								<TextField
									fullWidth
									type="date"
									label="End Date"
									value={form.endDate}
									onChange={(event) =>
										setForm((previous) => ({
											...previous,
											endDate: event.target.value,
										}))
									}
									slotProps={{
										inputLabel: {
											shrink: true,
										},
									}}
								/>
							</Grid>
						</Grid>

						<TextField
							fullWidth
							multiline
							minRows={4}
							label="Reason"
							value={form.leaveReason}
							onChange={(event) =>
								setForm((previous) => ({
									...previous,
									leaveReason: event.target.value,
								}))
							}
						/>

						<Box
							sx={{
								display: "flex",
								justifyContent: "space-between",
								alignItems: "center",
								gap: 2,
							}}
						>
							<Typography variant="body2" color="text.secondary">
								Attachments are currently disabled in UI. Use backend upload API if needed.
							</Typography>

							<Button
								variant="contained"
								onClick={handleSubmit}
								disabled={isPending}
							>
								{isPending ? "Submitting..." : "Submit"}
							</Button>
						</Box>
					</Stack>
				</CardContent>
			</Card>
		</>
	);
}
