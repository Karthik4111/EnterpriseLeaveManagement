import {
    AppBar,
    Toolbar,
    Typography,
    Avatar,
    Box
} from "@mui/material";

function AppHeader() {
    return (
        <AppBar
            position="fixed"
            sx={{
                zIndex: 1201,
            }}
        >
            <Toolbar>

                <Typography
                    variant="h6"
                    sx={{ flexGrow: 1 }}
                >
                    Enterprise Leave Management
                </Typography>

                <Box
                    sx={{
                            display: "flex",
                            alignItems: "center",
                            gap: 2,
                        }}
                    >
                    <Typography>
                        Admin
                    </Typography>

                    <Avatar>
                        A
                    </Avatar>
                </Box>

            </Toolbar>
        </AppBar>
    );
}

export default AppHeader;