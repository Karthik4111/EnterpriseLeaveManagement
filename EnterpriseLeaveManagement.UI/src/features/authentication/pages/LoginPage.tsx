import { Box } from "@mui/material";
import LoginForm from "../components/LoginForm";

function LoginPage() {
    return (
        <Box
            sx={{
                minHeight: "100vh",
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                backgroundColor: "#f5f5f5"
            }}
        >
            <LoginForm />
        </Box>
    );
}

export default LoginPage;