import { Box, Typography } from "@mui/material";
import { useAuth } from "@/context/AuthContext";

function getGreeting(): string {
    const hour = new Date().getHours();
    if (hour < 12) return "Good Morning";
    if (hour < 17) return "Good Afternoon";
    return "Good Evening";
}

export default function DashboardHeader() {
    const { auth } = useAuth();

    const today = new Date().toLocaleDateString("en-US", {
        weekday: "long",
        year: "numeric",
        month: "long",
        day: "numeric",
    });

    return (
        <Box
            sx={{
                p: { xs: 2.5, md: 3.5 },
                borderRadius: "20px",
                background:
                    "linear-gradient(135deg, #EFF6FF 0%, #F0F9FF 50%, #F5F3FF 100%)",
                border: "1px solid #DBEAFE",
                position: "relative",
                overflow: "hidden",
            }}
        >
            {/* Decorative background circles */}
            <Box
                sx={{
                    position: "absolute",
                    top: -30,
                    right: -30,
                    width: 200,
                    height: 200,
                    borderRadius: "50%",
                    bgcolor: "rgba(37,99,235,0.05)",
                    pointerEvents: "none",
                }}
            />
            <Box
                sx={{
                    position: "absolute",
                    bottom: -40,
                    right: 80,
                    width: 160,
                    height: 160,
                    borderRadius: "50%",
                    bgcolor: "rgba(99,102,241,0.05)",
                    pointerEvents: "none",
                }}
            />

            <Typography
                sx={{
                    fontSize: "0.875rem",
                    fontWeight: 500,
                    color: "#6366F1",
                    mb: 0.75,
                    letterSpacing: "0.01em",
                    position: "relative",
                }}
            >
                {getGreeting()} 👋
            </Typography>

            <Typography
                variant="h4"
                sx={{
                    fontWeight: 700,
                    color: "#111827",
                    mb: 0.75,
                    letterSpacing: "-0.01em",
                    position: "relative",
                }}
            >
                {auth.user?.fullName ?? "Welcome back"}
            </Typography>

            <Typography
                sx={{
                    fontSize: "0.9375rem",
                    color: "#6B7280",
                    position: "relative",
                }}
            >
                Here’s what’s happening today · {today}
            </Typography>
        </Box>
    );
}