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
import WorkspacesOutlinedIcon from "@mui/icons-material/WorkspacesOutlined";

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

            login(response.accessToken, response.refreshToken);

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
                background:
                    "linear-gradient(135deg, #0F172A 0%, #1E3A5F 50%, #1E1B4B 100%)",
                position: "relative",
                overflow: "hidden",
            }}
        >
            {/* Decorative circles */}
            <Box
                sx={{
                    position: "absolute",
                    top: -120,
                    left: -120,
                    width: 400,
                    height: 400,
                    borderRadius: "50%",
                    background: "rgba(37,99,235,0.12)",
                    pointerEvents: "none",
                }}
            />
            <Box
                sx={{
                    position: "absolute",
                    bottom: -100,
                    right: -100,
                    width: 350,
                    height: 350,
                    borderRadius: "50%",
                    background: "rgba(99,102,241,0.10)",
                    pointerEvents: "none",
                }}
            />

            {/* Left branding panel — desktop only */}
            <Box
                sx={{
                    display: { xs: "none", md: "flex" },
                    flex: 1,
                    flexDirection: "column",
                    justifyContent: "center",
                    alignItems: "flex-start",
                    px: 8,
                    position: "relative",
                    zIndex: 1,
                }}
            >
                <Box
                    sx={{
                        display: "flex",
                        alignItems: "center",
                        gap: 1.5,
                        mb: 5,
                    }}
                >
                    <Box
                        sx={{
                            width: 44,
                            height: 44,
                            borderRadius: "14px",
                            bgcolor: "#2563EB",
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "center",
                            boxShadow: "0 4px 16px rgba(37,99,235,0.4)",
                        }}
                    >
                        <WorkspacesOutlinedIcon
                            sx={{ color: "#fff", fontSize: 22 }}
                        />
                    </Box>
                    <Typography
                        sx={{
                            fontSize: "1.125rem",
                            fontWeight: 700,
                            color: "#F9FAFB",
                            letterSpacing: "-0.01em",
                        }}
                    >
                        Enterprise Leave
                    </Typography>
                </Box>

                <Typography
                    sx={{
                        fontSize: "2.5rem",
                        fontWeight: 700,
                        color: "#F9FAFB",
                        lineHeight: 1.2,
                        letterSpacing: "-0.02em",
                        mb: 2,
                    }}
                >
                    HR Management
                    <br />
                    made simple.
                </Typography>

                <Typography
                    sx={{
                        fontSize: "1rem",
                        color: "rgba(249,250,251,0.6)",
                        lineHeight: 1.6,
                        maxWidth: 380,
                    }}
                >
                    Manage employee leaves, approvals, and HR operations
                    from one powerful platform.
                </Typography>
            </Box>

            {/* Right login card panel */}
            <Box
                sx={{
                    display: "flex",
                    flex: { xs: 1, md: "0 0 480px" },
                    alignItems: "center",
                    justifyContent: "center",
                    p: { xs: 2, sm: 4 },
                    position: "relative",
                    zIndex: 1,
                    bgcolor: { md: "rgba(255,255,255,0.03)" },
                    backdropFilter: { md: "blur(20px)" },
                    borderLeft: { md: "1px solid rgba(255,255,255,0.08)" },
                }}
            >
                <Card
                    elevation={0}
                    sx={{
                        width: "100%",
                        maxWidth: 420,
                        borderRadius: "20px",
                        bgcolor: "#FFFFFF",
                        border: "1px solid #E5E7EB",
                        boxShadow:
                            "0 20px 60px rgba(0,0,0,0.35), 0 8px 20px rgba(0,0,0,0.2)",
                    }}
                >
                    <CardContent
                        sx={{
                            p: { xs: 3, sm: 4 },
                            "&:last-child": { pb: { xs: 3, sm: 4 } },
                        }}
                    >
                        {/* Mobile logo */}
                        <Box
                            sx={{
                                display: { xs: "flex", md: "none" },
                                alignItems: "center",
                                gap: 1.25,
                                mb: 3,
                                justifyContent: "center",
                            }}
                        >
                            <Box
                                sx={{
                                    width: 36,
                                    height: 36,
                                    borderRadius: "10px",
                                    bgcolor: "#2563EB",
                                    display: "flex",
                                    alignItems: "center",
                                    justifyContent: "center",
                                }}
                            >
                                <WorkspacesOutlinedIcon
                                    sx={{ color: "#fff", fontSize: 18 }}
                                />
                            </Box>
                            <Typography
                                sx={{
                                    fontSize: "1rem",
                                    fontWeight: 700,
                                    color: "#111827",
                                }}
                            >
                                Enterprise Leave
                            </Typography>
                        </Box>

                        <Typography
                            variant="h5"
                            sx={{ fontWeight: 700, color: "#111827", mb: 0.75 }}
                        >
                            Welcome back
                        </Typography>

                        <Typography
                            sx={{
                                fontSize: "0.9375rem",
                                color: "#6B7280",
                                mb: 3.5,
                            }}
                        >
                            Sign in to your account to continue
                        </Typography>

                        {import.meta.env.DEV && (
                            <Box
                                sx={{
                                    mb: 3,
                                    px: 2,
                                    py: 1.25,
                                    borderRadius: "10px",
                                    bgcolor: "#EFF6FF",
                                    border: "1px solid #BFDBFE",
                                }}
                            >
                                <Typography
                                    sx={{
                                        fontSize: "0.8125rem",
                                        color: "#1D4ED8",
                                        fontWeight: 500,
                                    }}
                                >
                                    Dev: admin@company.com / Admin@123
                                </Typography>
                            </Box>
                        )}

                        <Box
                            component="form"
                            onSubmit={handleSubmit(onSubmit)}
                        >
                            <Stack spacing={2.5}>
                                <TextField
                                    label="Email address"
                                    placeholder="you@company.com"
                                    {...register("email")}
                                    error={!!errors.email}
                                    helperText={errors.email?.message}
                                    autoComplete="email"
                                />

                                <TextField
                                    label="Password"
                                    placeholder="Enter your password"
                                    type={
                                        showPassword ? "text" : "password"
                                    }
                                    {...register("password")}
                                    error={!!errors.password}
                                    helperText={errors.password?.message}
                                    autoComplete="current-password"
                                    slotProps={{
                                        input: {
                                            endAdornment: (
                                                <InputAdornment position="end">
                                                    <IconButton
                                                        edge="end"
                                                        onClick={() =>
                                                            setShowPassword(
                                                                (p) => !p
                                                            )
                                                        }
                                                        sx={{
                                                            color: "#9CA3AF",
                                                        }}
                                                    >
                                                        {showPassword ? (
                                                            <VisibilityOff fontSize="small" />
                                                        ) : (
                                                            <Visibility fontSize="small" />
                                                        )}
                                                    </IconButton>
                                                </InputAdornment>
                                            ),
                                        },
                                    }}
                                />

                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            size="small"
                                            sx={{ py: 0 }}
                                        />
                                    }
                                    label={
                                        <Typography
                                            sx={{
                                                fontSize: "0.875rem",
                                                color: "#6B7280",
                                            }}
                                        >
                                            Remember me
                                        </Typography>
                                    }
                                />

                                <Button
                                    type="submit"
                                    fullWidth
                                    size="large"
                                    disabled={isSubmitting}
                                    sx={{
                                        py: 1.375,
                                        fontSize: "0.9375rem",
                                        fontWeight: 600,
                                        borderRadius: "10px",
                                        boxShadow:
                                            "0 4px 12px rgba(37,99,235,0.3)",
                                        "&:hover": {
                                            boxShadow:
                                                "0 6px 16px rgba(37,99,235,0.4)",
                                        },
                                    }}
                                >
                                    {isSubmitting
                                        ? "Signing in..."
                                        : "Sign in"}
                                </Button>
                            </Stack>
                        </Box>
                    </CardContent>
                </Card>
            </Box>
        </Box>
    );
}
