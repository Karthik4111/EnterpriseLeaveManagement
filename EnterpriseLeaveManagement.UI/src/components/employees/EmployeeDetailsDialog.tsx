import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    Grid,
    Paper,
    Typography,
} from "@mui/material";

import type { Employee } from "@/types/employee";

interface EmployeeDetailsDialogProps {
    open: boolean;
    employee: Employee | null;
    onClose: () => void;
}

interface DetailItemProps {
    label: string;
    value: string;
}

function DetailItem({
    label,
    value,
}: DetailItemProps) {
    return (
        <Grid size={{ xs: 12, sm: 6 }}>
            <Paper
                variant="outlined"
                sx={{
                    p: 2,
                    height: "100%",
                }}
            >
                <Typography
                    variant="caption"
                    color="text.secondary"
                >
                    {label}
                </Typography>

                <Typography
                    variant="body1"
                    sx={{
                        mt: 0.5,
                        fontWeight: 500,
                    }}
                >
                    {value}
                </Typography>
            </Paper>
        </Grid>
    );
}

export default function EmployeeDetailsDialog({
    open,
    employee,
    onClose,
}: EmployeeDetailsDialogProps) {
    if (!employee) {
        return null;
    }

    return (
        <Dialog
            open={open}
            onClose={onClose}
            fullWidth
            maxWidth="md"
        >
            <DialogTitle>
                Employee Details
            </DialogTitle>

            <DialogContent>
                <Grid
                    container
                    spacing={2}
                    sx={{ mt: 1 }}
                >
                    <DetailItem
                        label="Employee Code"
                        value={employee.employeeCode}
                    />

                    <DetailItem
                        label="Full Name"
                        value={employee.fullName}
                    />

                    <DetailItem
                        label="Department"
                        value={employee.department}
                    />

                    <DetailItem
                        label="Designation"
                        value={employee.designation}
                    />

                    <DetailItem
                        label="Email"
                        value={employee.email}
                    />
                </Grid>
            </DialogContent>

            <DialogActions>
                <Button onClick={onClose}>
                    Close
                </Button>
            </DialogActions>
        </Dialog>
    );
}