import { Box, Typography } from "@mui/material";

export default function Footer() {
    return (
        <Box
            component="footer"
            sx={{
                py: 2,
                textAlign: "center",
                borderTop: 1,
                borderColor: "divider",
                bgcolor: "background.paper",
            }}
        >
            <Typography variant="body2">
                Enterprise Leave Management System ©{" "}
                {new Date().getFullYear()}
            </Typography>
        </Box>
    );
}