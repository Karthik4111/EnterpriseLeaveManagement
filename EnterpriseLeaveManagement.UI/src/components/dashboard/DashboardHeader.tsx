import { Box, Typography } from "@mui/material";

import { useAuth } from "@/context/AuthContext";

export default function DashboardHeader() {
    const { auth } = useAuth();

    return (
        <Box
            sx={{
                mb: 4,
            }}
        >
            <Typography
                variant="h4"
                sx={{
                    fontWeight: 700,
                }}
            >
                Welcome, {auth.user?.fullName}
            </Typography>

            <Typography
                variant="body1"
                sx={{
                    mt: 1,
                    color: "text.secondary",
                }}
            >
                Here's today's overview.
            </Typography>
        </Box>
    );
}