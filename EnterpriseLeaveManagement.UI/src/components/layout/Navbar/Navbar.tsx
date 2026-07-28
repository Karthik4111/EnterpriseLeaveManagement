import MenuIcon from "@mui/icons-material/Menu";
import NotificationsNoneIcon from "@mui/icons-material/NotificationsNone";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";

import {
    AppBar,
    Badge,
    Box,
    Button,
    IconButton,
    Toolbar,
    Typography,
} from "@mui/material";
import { useNavigate } from "react-router-dom";

import useAuth from "@/hooks/useAuth";
import { useNotifications } from "@/hooks/useLeave";
import { ROUTES } from "@/constants/routes";

interface NavbarProps {
    onMenuClick?: () => void;
}

export default function Navbar({
    onMenuClick,
}: NavbarProps) {
    const { auth, logout } = useAuth();
    const { data: notifications = [] } =
        useNotifications();
    const navigate = useNavigate();

    const unreadCount = notifications.filter(
        (item) => !item.isRead
    ).length;

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

                <IconButton
                    color="inherit"
                    onClick={() =>
                        navigate(ROUTES.PROFILE)
                    }
                >
                    <Badge
                        badgeContent={unreadCount}
                        color="error"
                    >
                        <NotificationsNoneIcon />
                    </Badge>
                </IconButton>

                <IconButton color="inherit">
                    <AccountCircleIcon />
                </IconButton>

                <Box
                    sx={{
                        ml: 1,
                        display: "flex",
                        alignItems: "center",
                        gap: 1,
                    }}
                >
                    <Box>
                        {auth.user?.fullName ?? "User"}
                    </Box>
                    <Button
                        color="inherit"
                        variant="outlined"
                        size="small"
                        onClick={() => {
                            logout();
                            navigate(ROUTES.LOGIN);
                        }}
                    >
                        Logout
                    </Button>
                </Box>
            </Toolbar>
        </AppBar>
    );
}