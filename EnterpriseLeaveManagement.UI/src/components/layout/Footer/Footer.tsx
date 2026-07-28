import { Box, Divider, Typography } from "@mui/material";

export default function Footer() {
    return (
        <Box
            component="footer"
            sx={{
                flexShrink: 0,
                px: { xs: 2, sm: 3, md: 4 },
                py: 2,
                maxWidth: 1440,
                width: "100%",
                mx: "auto",
                boxSizing: "border-box",
            }}
        >
            <Divider sx={{ borderColor: "#E5E7EB", mb: 2 }} />

            <Box
                sx={{
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "space-between",
                    flexWrap: "wrap",
                    gap: 1,
                }}
            >
                <Typography
                    sx={{
                        fontSize: "0.8125rem",
                        color: "#9CA3AF",
                        fontWeight: 400,
                    }}
                >
                    © {new Date().getFullYear()} Enterprise Leave Management
                    System. All rights reserved.
                </Typography>

                <Typography
                    sx={{
                        fontSize: "0.8125rem",
                        color: "#9CA3AF",
                        fontWeight: 400,
                    }}
                >
                    Version 1.0
                </Typography>
            </Box>
        </Box>
    );
}