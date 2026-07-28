import { useMemo, useState } from "react";
import type { ChangeEvent } from "react";

import AddIcon from "@mui/icons-material/Add";
import {
    Box,
    Button,
    Chip,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    MenuItem,
    Stack,
    TextField,
    Typography,
} from "@mui/material";

import PageHeader from "@/components/common/PageHeader/PageHeader";
import SearchBar from "@/components/common/SearchBar/SearchBar";
import DataTable, {
    type DataColumn,
} from "@/components/common/DataTable/DataTable";
import TableActions from "@/components/common/TableActions/TableActions";

import { useCreateEmployee, useEmployees } from "@/hooks/useEmployees";
import { useDepartments } from "@/hooks/useDepartments";
import useSnackbar from "@/hooks/useSnackbar";

import type { Employee } from "@/types/employee";
import type { CreateEmployeeApiRequest } from "@/types/employeeApi";
import { ROLES } from "@/constants/roles";

interface CreateForm {
    employeeCode: string;
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    phoneNumber: string;
    designation: string;
    departmentId: string;
    dateOfJoining: string;
    role: string;
}

const emptyForm: CreateForm = {
    employeeCode: "",
    firstName: "",
    lastName: "",
    email: "",
    password: "Employee@123",
    phoneNumber: "",
    designation: "",
    departmentId: "",
    dateOfJoining: new Date().toISOString().slice(0, 10),
    role: ROLES.EMPLOYEE,
};

const row3 = { display: "flex", gap: 2, flexWrap: "wrap" as const };
const col3 = { flex: "1 1 200px", minWidth: 180 };

export default function EmployeeListPage() {
    const { data: employeeList = [], isLoading } = useEmployees();
    const { data: departments = [] } = useDepartments();
    const { mutateAsync: createEmployee, isPending } = useCreateEmployee();
    const { showSuccess, showError } = useSnackbar();

    const [search, setSearch] = useState("");
    const [dialogOpen, setDialogOpen] = useState(false);
    const [viewEmployee, setViewEmployee] = useState<Employee | null>(null);
    const [form, setForm] = useState<CreateForm>(emptyForm);
    const [formErrors, setFormErrors] = useState<Partial<CreateForm>>({});

    const filteredEmployees = useMemo(() => {
        const keyword = search.trim().toLowerCase();
        if (!keyword) return employeeList;
        return employeeList.filter(
            (e) =>
                e.employeeCode.toLowerCase().includes(keyword) ||
                e.fullName.toLowerCase().includes(keyword) ||
                e.department.toLowerCase().includes(keyword) ||
                e.designation.toLowerCase().includes(keyword) ||
                e.email.toLowerCase().includes(keyword)
        );
    }, [employeeList, search]);

    const validate = (): boolean => {
        const errs: Partial<CreateForm> = {};
        if (!form.employeeCode.trim())                       errs.employeeCode  = "Required";
        if (!form.firstName.trim())                          errs.firstName     = "Required";
        if (!form.lastName.trim())                           errs.lastName      = "Required";
        if (!form.email.trim() || !/\S+@\S+\.\S+/.test(form.email))
                                                             errs.email         = "Valid email required";
        if (!form.password || form.password.length < 8)     errs.password      = "Min 8 characters";
        if (!form.phoneNumber.trim())                        errs.phoneNumber   = "Required";
        if (!form.designation.trim())                        errs.designation   = "Required";
        if (!form.departmentId)                              errs.departmentId  = "Required";
        if (!form.dateOfJoining)                             errs.dateOfJoining = "Required";
        setFormErrors(errs);
        return Object.keys(errs).length === 0;
    };

    const handleCreate = async () => {
        if (!validate()) return;
        try {
            const req: CreateEmployeeApiRequest = {
                employeeCode:    form.employeeCode.trim(),
                firstName:       form.firstName.trim(),
                lastName:        form.lastName.trim(),
                userName:        form.email.trim().split("@")[0],
                email:           form.email.trim(),
                password:        form.password,
                confirmPassword: form.password,
                role:            form.role,
                departmentId:    form.departmentId,
                phoneNumber:     form.phoneNumber.trim(),
                designation:     form.designation.trim(),
                dateOfJoining:   form.dateOfJoining,
                dateOfBirth:     null,
            };
            await createEmployee(req);
            showSuccess("Employee created successfully.");
            setDialogOpen(false);
            setForm(emptyForm);
            setFormErrors({});
        } catch {
            showError("Failed to create employee. Check all details and try again.");
        }
    };

    function field(key: keyof CreateForm) {
        return {
            value: form[key],
            onChange: (e: ChangeEvent<HTMLInputElement>) => {
                setForm((prev) => ({ ...prev, [key]: e.target.value }));
                setFormErrors((prev) => ({ ...prev, [key]: undefined }));
            },
            error: !!formErrors[key],
            helperText: formErrors[key],
        };
    }

    const columns: DataColumn<Employee>[] = [
        { field: "employeeCode", headerName: "ID",          sortable: true },
        { field: "fullName",     headerName: "Name",         sortable: true },
        { field: "department",   headerName: "Department",   sortable: true },
        { field: "designation",  headerName: "Designation",  sortable: true },
        { field: "email",        headerName: "Email",        sortable: true },
        {
            field: "id",
            headerName: "Actions",
            sortable: false,
            align: "center",
            render: (employee) => (
                <TableActions onView={() => setViewEmployee(employee)} />
            ),
        },
    ];

    return (
        <>
            <PageHeader
                title="Employees"
                subtitle="Manage all employees in the organisation."
                actionText="Add Employee"
                actionIcon={<AddIcon />}
                onActionClick={() => {
                    setForm(emptyForm);
                    setFormErrors({});
                    setDialogOpen(true);
                }}
            />

            <SearchBar
                value={search}
                onChange={setSearch}
                placeholder="Search by name, code, department..."
            />

            <DataTable columns={columns} rows={filteredEmployees} loading={isLoading} />

            {/* Create Employee */}
            <Dialog
                open={dialogOpen}
                onClose={isPending ? undefined : () => setDialogOpen(false)}
                fullWidth
                maxWidth="md"
            >
                <DialogTitle>Add New Employee</DialogTitle>
                <DialogContent dividers>
                    <Stack spacing={2} sx={{ pt: 1 }}>
                        <Box sx={row3}>
                            <TextField sx={col3} required label="Employee Code" {...field("employeeCode")} />
                            <TextField sx={col3} required label="First Name"    {...field("firstName")} />
                            <TextField sx={col3} required label="Last Name"     {...field("lastName")} />
                        </Box>
                        <Box sx={row3}>
                            <TextField sx={col3} required label="Email"        type="email"    {...field("email")} />
                            <TextField sx={col3} required label="Phone Number"                 {...field("phoneNumber")} />
                        </Box>
                        <Box sx={row3}>
                            <TextField sx={col3} required label="Designation" {...field("designation")} />
                            <TextField
                                select sx={col3} required label="Department"
                                value={form.departmentId}
                                onChange={(e) => {
                                    setForm((prev) => ({ ...prev, departmentId: e.target.value }));
                                    setFormErrors((prev) => ({ ...prev, departmentId: undefined }));
                                }}
                                error={!!formErrors.departmentId}
                                helperText={formErrors.departmentId}
                            >
                                {departments.map((d) => (
                                    <MenuItem key={d.id} value={d.id}>{d.name}</MenuItem>
                                ))}
                            </TextField>
                        </Box>
                        <Box sx={row3}>
                            <TextField
                                select sx={col3} label="Role"
                                value={form.role}
                                onChange={(e) => setForm((prev) => ({ ...prev, role: e.target.value }))}
                            >
                                {Object.values(ROLES).map((r) => (
                                    <MenuItem key={r} value={r}>{r}</MenuItem>
                                ))}
                            </TextField>
                            <TextField
                                sx={col3} required type="date" label="Date of Joining"
                                {...field("dateOfJoining")}
                                slotProps={{ inputLabel: { shrink: true } }}
                            />
                            <TextField
                                sx={col3} required type="password" label="Password"
                                {...field("password")}
                                helperText={formErrors.password ?? "Min 8 chars, 1 uppercase, 1 digit, 1 special"}
                            />
                        </Box>
                    </Stack>
                </DialogContent>
                <DialogActions sx={{ px: 3, py: 2 }}>
                    <Button onClick={() => setDialogOpen(false)} disabled={isPending}>Cancel</Button>
                    <Button variant="contained" onClick={handleCreate} disabled={isPending}>
                        {isPending ? "Creating..." : "Create Employee"}
                    </Button>
                </DialogActions>
            </Dialog>

            {/* View Employee */}
            <Dialog
                open={Boolean(viewEmployee)}
                onClose={() => setViewEmployee(null)}
                fullWidth maxWidth="sm"
            >
                <DialogTitle>Employee Details</DialogTitle>
                <DialogContent dividers>
                    {viewEmployee && (
                        <Stack spacing={1.5}>
                            {(
                                [
                                    ["Code",        viewEmployee.employeeCode],
                                    ["Name",        viewEmployee.fullName],
                                    ["Email",       viewEmployee.email],
                                    ["Department",  viewEmployee.department],
                                    ["Designation", viewEmployee.designation],
                                ] as [string, string][]
                            ).map(([label, value]) => (
                                <Box key={label} sx={{ display: "flex", gap: 1 }}>
                                    <Typography sx={{ fontWeight: 600, minWidth: 130 }}>{label}:</Typography>
                                    <Typography>{value}</Typography>
                                </Box>
                            ))}
                            <Box sx={{ display: "flex", gap: 1 }}>
                                <Typography sx={{ fontWeight: 600, minWidth: 130 }}>Status:</Typography>
                                <Chip size="small" color="success" label="Active" />
                            </Box>
                        </Stack>
                    )}
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setViewEmployee(null)}>Close</Button>
                </DialogActions>
            </Dialog>
        </>
    );
}