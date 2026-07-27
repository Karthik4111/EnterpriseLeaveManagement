import {
    Button,
    Paper,
    Stack,
    TextField,
    Typography
} from "@mui/material";

function LoginForm() {
    return (
        <Paper
            elevation={4}
            sx={{
                width: 450,
                p: 5,
                borderRadius: 3
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

            <Stack spacing={3}>
                <TextField
                    label="Email"
                    fullWidth
                />

                <TextField
                    label="Password"
                    type="password"
                    fullWidth
                />

                <Button
                    variant="contained"
                    size="large"
                >
                    Login
                </Button>
            </Stack>
        </Paper>
    );
}

export default LoginForm;