import { useMemo, useState } from "react";
import {
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	Stack,
	Switch,
	TextField,
	Typography,
} from "@mui/material";

import AddIcon from "@mui/icons-material/Add";

import PageHeader from "@/components/common/PageHeader/PageHeader";
import SearchBar from "@/components/common/SearchBar/SearchBar";
import ConfirmDialog from "@/components/common/ConfirmDialog/ConfirmDialog";
import DataTable, {
	type DataColumn,
} from "@/components/common/DataTable/DataTable";
import TableActions from "@/components/common/TableActions/TableActions";

import {
	useCreateDepartment,
	useDeleteDepartment,
	useDepartments,
	useUpdateDepartment,
} from "@/hooks/useDepartments";
import useSnackbar from "@/hooks/useSnackbar";
import type {
	CreateDepartmentRequest,
	Department,
	UpdateDepartmentRequest,
} from "@/types/department";

interface DepartmentFormState {
	name: string;
	code: string;
	description: string;
	isActive: boolean;
}

const emptyForm: DepartmentFormState = {
	name: "",
	code: "",
	description: "",
	isActive: true,
};

export default function DepartmentListPage() {
	const { data: departments = [], isLoading } =
		useDepartments();

	const { mutateAsync: createDepartment, isPending: isCreating } =
		useCreateDepartment();

	const { mutateAsync: updateDepartment, isPending: isUpdating } =
		useUpdateDepartment();

	const { mutateAsync: deleteDepartment, isPending: isDeleting } =
		useDeleteDepartment();

	const { showError, showSuccess } = useSnackbar();

	const [search, setSearch] = useState("");
	const [dialogOpen, setDialogOpen] = useState(false);
	const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
	const [editingDepartment, setEditingDepartment] =
		useState<Department | null>(null);
	const [selectedDepartment, setSelectedDepartment] =
		useState<Department | null>(null);
	const [form, setForm] =
		useState<DepartmentFormState>(emptyForm);

	const isSubmitting = isCreating || isUpdating;

	const filteredDepartments = useMemo(() => {
		const keyword = search.trim().toLowerCase();

		if (!keyword) {
			return departments;
		}

		return departments.filter(
			(department) =>
				department.name
					.toLowerCase()
					.includes(keyword) ||
				department.code
					.toLowerCase()
					.includes(keyword) ||
				department.description
					.toLowerCase()
					.includes(keyword)
		);
	}, [departments, search]);

	const resetForm = () => {
		setForm(emptyForm);
		setEditingDepartment(null);
	};

	const openCreateDialog = () => {
		resetForm();
		setDialogOpen(true);
	};

	const openEditDialog = (department: Department) => {
		setEditingDepartment(department);
		setForm({
			name: department.name,
			code: department.code,
			description: department.description,
			isActive: department.isActive,
		});
		setDialogOpen(true);
	};

	const handleSave = async () => {
		if (!form.name.trim() || !form.code.trim()) {
			showError("Name and code are required.");
			return;
		}

		try {
			if (editingDepartment) {
				const request: UpdateDepartmentRequest = {
					id: editingDepartment.id,
					name: form.name.trim(),
					code: form.code.trim(),
					description: form.description.trim(),
					managerId: null,
				};

				await updateDepartment(request);
				showSuccess("Department updated successfully.");
			} else {
				const request: CreateDepartmentRequest = {
					name: form.name.trim(),
					code: form.code.trim(),
					description: form.description.trim(),
					managerId: null,
				};

				await createDepartment(request);
				showSuccess("Department created successfully.");
			}

			setDialogOpen(false);
			resetForm();
		} catch {
			showError("Unable to save department.");
		}
	};

	const handleDelete = async () => {
		if (!selectedDepartment) {
			return;
		}

		try {
			await deleteDepartment(selectedDepartment.id);
			showSuccess("Department deleted.");
			setDeleteDialogOpen(false);
			setSelectedDepartment(null);
		} catch {
			showError("Unable to delete department.");
		}
	};

	const columns: DataColumn<Department>[] = [
		{
			field: "name",
			headerName: "Name",
		},
		{
			field: "code",
			headerName: "Code",
		},
		{
			field: "description",
			headerName: "Description",
		},
		{
			field: "isActive",
			headerName: "Status",
			render: (department) =>
				department.isActive ? "Active" : "Inactive",
		},
		{
			field: "id",
			headerName: "Actions",
			sortable: false,
			align: "center",
			render: (department) => (
				<TableActions
					onEdit={() =>
						openEditDialog(department)
					}
					onDelete={() => {
						setSelectedDepartment(department);
						setDeleteDialogOpen(true);
					}}
				/>
			),
		},
	];

	return (
		<>
			<PageHeader
				title="Departments"
				subtitle="Create, update, and maintain organization departments."
				actionText="Add Department"
				actionIcon={<AddIcon />}
				onActionClick={openCreateDialog}
			/>

			<SearchBar
				value={search}
				onChange={setSearch}
				placeholder="Search departments..."
			/>

			<DataTable
				columns={columns}
				rows={filteredDepartments}
				loading={isLoading}
			/>

			<Dialog
				open={dialogOpen}
				onClose={
					isSubmitting
						? undefined
						: () => setDialogOpen(false)
				}
				fullWidth
				maxWidth="sm"
			>
				<DialogTitle>
					{editingDepartment
						? "Edit Department"
						: "Create Department"}
				</DialogTitle>

				<DialogContent>
					<Stack spacing={2} sx={{ mt: 1 }}>
						<TextField
							label="Name"
							value={form.name}
							onChange={(event) =>
								setForm((previous) => ({
									...previous,
									name: event.target.value,
								}))
							}
							required
						/>

						<TextField
							label="Code"
							value={form.code}
							onChange={(event) =>
								setForm((previous) => ({
									...previous,
									code: event.target.value,
								}))
							}
							required
						/>

						<TextField
							label="Description"
							value={form.description}
							onChange={(event) =>
								setForm((previous) => ({
									...previous,
									description: event.target.value,
								}))
							}
							multiline
							minRows={3}
						/>

						{editingDepartment && (
							<Stack
								direction="row"
								sx={{
									display: "flex",
									alignItems: "center",
									gap: 1,
								}}
							>
								<Switch
									checked={form.isActive}
									onChange={(event) =>
										setForm((previous) => ({
											...previous,
											isActive: event.target.checked,
										}))
									}
								/>

								<Typography>
									Active
								</Typography>
							</Stack>
						)}
					</Stack>
				</DialogContent>

				<DialogActions>
					<Button
						onClick={() => setDialogOpen(false)}
						disabled={isSubmitting}
					>
						Cancel
					</Button>

					<Button
						variant="contained"
						onClick={handleSave}
						disabled={isSubmitting}
					>
						{isSubmitting
							? "Saving..."
							: "Save"}
					</Button>
				</DialogActions>
			</Dialog>

			<ConfirmDialog
				open={deleteDialogOpen}
				title="Delete Department"
				message={
					selectedDepartment
						? `Are you sure you want to delete \"${selectedDepartment.name}\"?`
						: ""
				}
				confirmText="Delete"
				loading={isDeleting}
				onConfirm={handleDelete}
				onCancel={() => {
					setDeleteDialogOpen(false);
					setSelectedDepartment(null);
				}}
			/>
		</>
	);
}
