import { Box, Button, Typography } from "@mui/material";
import { Link } from "react-router-dom";

function NotFoundPage() {
    return (
        <Box
            sx={{
                height: "100vh",
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                flexDirection: "column",
                gap: 2
            }}
        >
            <Typography variant="h2">
                404
            </Typography>

            <Typography>
                Page Not Found
            </Typography>

            <Button
                component={Link}
                to="/"
                variant="contained"
            >
                Go Home
            </Button>
        </Box>
    );
}

export default NotFoundPage;