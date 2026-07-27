import { zodResolver } from "@hookform/resolvers/zod";
import {
    Alert,
    IconButton,
    InputAdornment,
    Paper,
    Stack,
    TextField,
    Typography,
} from "@mui/material";
import { LoadingButton } from "@mui/lab";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

import { useAuth } from "@/context/AuthContext";
import type { LoginRequest } from "@/types";

import { authService } from "../services/authService";
import { loginSchema } from "../validation/loginSchema";

function LoginForm() {
    const navigate = useNavigate();

    const { login } = useAuth();

    const [error, setError] = useState("");
    const [showPassword, setShowPassword] = useState(false);

    const {
        register,
        handleSubmit,
        formState: { errors, isSubmitting },
    } = useForm<LoginRequest>({
        resolver: zodResolver(loginSchema),
        defaultValues: {
            email: "",
            password: "",
        },
    });

    const onSubmit = async (data: LoginRequest) => {
        try {
            setError("");

            const response = await authService.login(data);

            login(response);

            toast.success("Login successful.");

            navigate("/");
        } catch {
            setError("Invalid email or password.");
            toast.error("Login failed.");
        }
    };

    return (
        <Paper
            elevation={4}
            sx={{
                width: 450,
                p: 5,
                borderRadius: 3,
            }}
        >
            <Typography
                variant="h4"
                gutterBottom
                sx={{ fontWeight: 700 }}
            >
                Sign In
            </Typography>

            <Typography
                color="text.secondary"
                sx={{ mb: 4 }}
            >
                Welcome back to Enterprise Leave Management
            </Typography>

            <Stack
                component="form"
                spacing={3}
                onSubmit={handleSubmit(onSubmit)}
            >
                {error && (
                    <Alert severity="error">
                        {error}
                    </Alert>
                )}

                <TextField
                    label="Email"
                    fullWidth
                    {...register("email")}
                    error={!!errors.email}
                    helperText={errors.email?.message}
                />

                <TextField
                    label="Password"
                    type={showPassword ? "text" : "password"}
                    fullWidth
                    {...register("password")}
                    error={!!errors.password}
                    helperText={errors.password?.message}
                    slotProps={{
                        input: {
                            endAdornment: (
                                <InputAdornment position="end">
                                    <IconButton
                                        edge="end"
                                        onClick={() =>
                                            setShowPassword((prev) => !prev)
                                        }
                                    >
                                        {showPassword ? (
                                            <VisibilityOff />
                                        ) : (
                                            <Visibility />
                                        )}
                                    </IconButton>
                                </InputAdornment>
                            ),
                        },
                    }}
                />

                <LoadingButton
                    type="submit"
                    variant="contained"
                    size="large"
                    loading={isSubmitting}
                    fullWidth
                >
                    Sign In
                </LoadingButton>
            </Stack>
        </Paper>
    );
}

export default LoginForm;