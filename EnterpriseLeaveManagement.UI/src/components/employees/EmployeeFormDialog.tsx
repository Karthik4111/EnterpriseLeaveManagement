import { useEffect } from "react";

import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Grid,
    MenuItem,
    TextField,
} from "@mui/material";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import {
    employeeSchema,
    type EmployeeFormData,
} from "@/validation/employeeSchema";

import { departments } from "@/data/departments";
import { designations } from "@/data/designations";

import type { Employee } from "@/types/employee";

interface EmployeeFormDialogProps {
    open: boolean;
    employee?: Employee | null;
    onClose: () => void;
    onSave: (employee: Employee) => void;
}

const emptyEmployee: EmployeeFormData = {
    employeeCode: "",
    fullName: "",
    department: "",
    designation: "",
    email: "",
};

export default function EmployeeFormDialog({
    open,
    employee,
    onClose,
    onSave,
}: EmployeeFormDialogProps) {
    const {
        register,
        handleSubmit,
        reset,
        formState: {
            errors,
            isSubmitting,
        },
    } = useForm<EmployeeFormData>({
        resolver: zodResolver(employeeSchema),
        defaultValues: emptyEmployee,
    });

    useEffect(() => {
        if (employee) {
            reset({
                employeeCode: employee.employeeCode,
                fullName: employee.fullName,
                department: employee.department,
                designation: employee.designation,
                email: employee.email,
            });
        } else {
            reset(emptyEmployee);
        }
    }, [employee, reset]);

    const onSubmit = (data: EmployeeFormData) => {
        onSave({
            id: employee?.id ?? "",
            ...data,
        });

        reset(emptyEmployee);
    };

    const handleClose = () => {
        reset(emptyEmployee);
        onClose();
    };

    return (
        <Dialog
            open={open}
            onClose={handleClose}
            fullWidth
            maxWidth="sm"
        >
            <DialogTitle>
                {employee
                    ? "Edit Employee"
                    : "Add Employee"}
            </DialogTitle>

            <DialogContent sx={{ pt: 2 }}>
                <Grid
                    container
                    spacing={2}
                >
                    <Grid size={12}>
                        <TextField
                            fullWidth
                            required
                            label="Employee Code"
                            {...register("employeeCode")}
                            error={!!errors.employeeCode}
                            helperText={
                                errors.employeeCode?.message
                            }
                        />
                    </Grid>

                    <Grid size={12}>
                        <TextField
                            fullWidth
                            required
                            label="Full Name"
                            {...register("fullName")}
                            error={!!errors.fullName}
                            helperText={
                                errors.fullName?.message
                            }
                        />
                    </Grid>

                    <Grid size={12}>
                        <TextField
                            select
                            fullWidth
                            required
                            label="Department"
                            defaultValue=""
                            {...register("department")}
                            error={!!errors.department}
                            helperText={
                                errors.department?.message
                            }
                        >
                            {departments.map((department) => (
                                <MenuItem
                                    key={department}
                                    value={department}
                                >
                                    {department}
                                </MenuItem>
                            ))}
                        </TextField>
                    </Grid>

                    <Grid size={12}>
                        <TextField
                            select
                            fullWidth
                            required
                            label="Designation"
                            defaultValue=""
                            {...register("designation")}
                            error={!!errors.designation}
                            helperText={
                                errors.designation?.message
                            }
                        >
                            {designations.map((designation) => (
                                <MenuItem
                                    key={designation}
                                    value={designation}
                                >
                                    {designation}
                                </MenuItem>
                            ))}
                        </TextField>
                    </Grid>

                    <Grid size={12}>
                        <TextField
                            fullWidth
                            required
                            type="email"
                            label="Email"
                            {...register("email")}
                            error={!!errors.email}
                            helperText={
                                errors.email?.message
                            }
                        />
                    </Grid>
                </Grid>
            </DialogContent>

            <DialogActions sx={{ p: 2 }}>
                <Button onClick={handleClose}>
                    Cancel
                </Button>

                <Button
                    variant="contained"
                    disabled={isSubmitting}
                    onClick={handleSubmit(onSubmit)}
                >
                    {employee ? "Update" : "Save"}
                </Button>
            </DialogActions>
        </Dialog>
    );
}