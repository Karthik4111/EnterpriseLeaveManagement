import {
    Button,
    Stack,
    Typography,
} from "@mui/material";
import { useNavigate } from "react-router-dom";

function UnauthorizedPage() {
    const navigate = useNavigate();

    return (
        <Stack
            sx={{
                minHeight: "100vh",
                justifyContent: "center",
                alignItems: "center",
                gap: 2,
            }}
        >
            <Typography variant="h2">
                403
            </Typography>

            <Typography variant="h5">
                Unauthorized
            </Typography>

            <Typography color="text.secondary">
                You don't have permission to access this page.
            </Typography>

            <Button
                variant="contained"
                onClick={() => navigate("/")}
            >
                Go Home
            </Button>
        </Stack>
    );
}

export default UnauthorizedPage;