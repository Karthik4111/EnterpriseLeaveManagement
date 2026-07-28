import { useState } from "react";

import {
    Box,
    Button,
    Card,
    CardContent,
    Checkbox,
    FormControlLabel,
    IconButton,
    InputAdornment,
    Stack,
    TextField,
    Typography,
} from "@mui/material";

import Visibility from "@mui/icons-material/Visibility";
import VisibilityOff from "@mui/icons-material/VisibilityOff";

import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";

import { useNavigate } from "react-router-dom";

import { useAuth } from "@/context/AuthContext";
import { ROUTES } from "@/constants/routes";
import { useLogin } from "@/hooks/useLogin";
import useSnackbar from "@/hooks/useSnackbar";

const loginSchema = z.object({
    email: z
        .string()
        .min(1, "Email is required.")
        .email("Invalid email address."),

    password: z
        .string()
        .min(6, "Password must be at least 6 characters."),
});

type LoginForm = z.infer<typeof loginSchema>;

export default function LoginPage() {
    const [showPassword, setShowPassword] = useState(false);

    const navigate = useNavigate();
    const { login } = useAuth();
    const { mutateAsync: loginRequest } = useLogin();
    const { showError, showSuccess } = useSnackbar();

    const {
        register,
        handleSubmit,
        formState: { errors, isSubmitting },
    } = useForm<LoginForm>({
        resolver: zodResolver(loginSchema),
        defaultValues: {
            email: "",
            password: "",
        },
    });

    const onSubmit = async (data: LoginForm) => {
        try {
            const response = await loginRequest({
                email: data.email,
                password: data.password,
            });

            login(
                response.accessToken,
                response.refreshToken
            );

            showSuccess("Login successful.");

            navigate(ROUTES.DASHBOARD);
        } catch (error) {
            console.error("Login failed:", error);
            showError("Invalid credentials. Please try again.");
        }
    };

    return (
        <Box
            sx={{
                minHeight: "100vh",
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                bgcolor: "background.default",
                p: 2,
            }}
        >
            <Card
                elevation={6}
                sx={{
                    width: 420,
                    borderRadius: 3,
                }}
            >
                <CardContent sx={{ p: 4 }}>
                    <Typography
                        variant="h4"
                        sx={{
                            fontWeight: 700,
                            textAlign: "center",
                            mb: 1,
                        }}
                    >
                        Welcome Back
                    </Typography>

                    <Typography
                        variant="body2"
                        sx={{
                            color: "text.secondary",
                            textAlign: "center",
                            mb: 4,
                        }}
                    >
                        Sign in to continue
                    </Typography>

                    {import.meta.env.DEV && (
                        <Typography
                            variant="caption"
                            sx={{
                                display: "block",
                                mb: 2,
                                color: "text.secondary",
                                textAlign: "center",
                            }}
                        >
                            Dev users: admin@company.com / Admin@123
                        </Typography>
                    )}

                    <Box
                        component="form"
                        onSubmit={handleSubmit(onSubmit)}
                    >
                        <Stack spacing={3}>
                            <TextField
                                label="Email"
                                placeholder="Enter your email"
                                {...register("email")}
                                error={!!errors.email}
                                helperText={errors.email?.message}
                            />

                            <TextField
                                label="Password"
                                placeholder="Enter your password"
                                type={showPassword ? "text" : "password"}
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

                            <FormControlLabel
                                control={<Checkbox />}
                                label="Remember Me"
                            />

                            <Button
                                type="submit"
                                fullWidth
                                size="large"
                                disabled={isSubmitting}
                            >
                                {isSubmitting
                                    ? "Signing In..."
                                    : "Sign In"}
                            </Button>
                        </Stack>
                    </Box>
                </CardContent>
            </Card>
        </Box>
    );
}