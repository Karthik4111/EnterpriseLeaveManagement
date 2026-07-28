import MenuIcon from "@mui/icons-material/Menu";
import NotificationsNoneIcon from "@mui/icons-material/NotificationsNone";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";

import {
    AppBar,
    Box,
    IconButton,
    Toolbar,
    Typography,
} from "@mui/material";

interface NavbarProps {
    onMenuClick?: () => void;
}

export default function Navbar({
    onMenuClick,
}: NavbarProps) {
    return (
        <AppBar
            position="fixed"
            elevation={1}
        >
            <Toolbar>
                <IconButton
                    color="inherit"
                    edge="start"
                    onClick={onMenuClick}
                    sx={{ mr: 2 }}
                >
                    <MenuIcon />
                </IconButton>

                <Typography
                    variant="h6"
                    sx={{ flexGrow: 1 }}
                >
                    Enterprise Leave Management
                </Typography>

                <IconButton color="inherit">
                    <NotificationsNoneIcon />
                </IconButton>

                <IconButton color="inherit">
                    <AccountCircleIcon />
                </IconButton>

                <Box sx={{ ml: 1 }}>
                    Admin
                </Box>
            </Toolbar>
        </AppBar>
    );
}