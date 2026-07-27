import {
    AppBar,
    Toolbar,
    Typography,
    Avatar,
    Box,
    IconButton,
} from "@mui/material";

import LogoutIcon from "@mui/icons-material/Logout";

import { APP_NAME } from "@/constants/app";
import { useAuth } from "@/context/AuthContext";

function AppHeader() {
    const { user, logout } = useAuth();

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
                    {APP_NAME}
                </Typography>

                <Box
                    sx={{
                        display: "flex",
                        alignItems: "center",
                        gap: 2,
                    }}
                >
                    <Typography>
                        {user?.email ?? "Guest"}
                    </Typography>

                    <Avatar>
                        {user?.email?.[0]?.toUpperCase() ?? "A"}
                    </Avatar>

                    <IconButton
                        color="inherit"
                        onClick={logout}
                    >
                        <LogoutIcon />
                    </IconButton>
                </Box>
            </Toolbar>
        </AppBar>
    );
}

export default AppHeader;