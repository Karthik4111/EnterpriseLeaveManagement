import { useState } from "react";

import MenuIcon from "@mui/icons-material/Menu";
import NotificationsNoneIcon from "@mui/icons-material/NotificationsNone";
import SearchIcon from "@mui/icons-material/Search";
import KeyboardArrowDownIcon from "@mui/icons-material/KeyboardArrowDown";
import LogoutIcon from "@mui/icons-material/Logout";
import PersonOutlinedIcon from "@mui/icons-material/PersonOutlined";

import {
    AppBar,
    Avatar,
    Badge,
    Box,
    Divider,
    IconButton,
    InputAdornment,
    InputBase,
    ListItemIcon,
    ListItemText,
    Menu,
    MenuItem,
    Toolbar,
    Tooltip,
    Typography,
} from "@mui/material";
import { useNavigate } from "react-router-dom";

import useAuth from "@/hooks/useAuth";
import { useNotifications } from "@/hooks/useLeave";
import { ROUTES } from "@/constants/routes";

interface NavbarProps {
    onMenuClick?: () => void;
}

function getInitials(name: string): string {
    return name
        .split(" ")
        .map((n) => n[0])
        .slice(0, 2)
        .join("")
        .toUpperCase();
}

export default function Navbar({ onMenuClick }: NavbarProps) {
    const { auth, logout } = useAuth();
    const { data: notifications = [] } = useNotifications();
    const navigate = useNavigate();

    const [profileAnchor, setProfileAnchor] =
        useState<null | HTMLElement>(null);
    const profileOpen = Boolean(profileAnchor);

    const unreadCount = notifications.filter(
        (item) => !item.isRead
    ).length;

    const handleProfileOpen = (e: React.MouseEvent<HTMLElement>) => {
        setProfileAnchor(e.currentTarget);
    };

    const handleProfileClose = () => {
        setProfileAnchor(null);
    };

    const handleLogout = () => {
        handleProfileClose();
        logout();
        navigate(ROUTES.LOGIN);
    };

    const handleGoToProfile = () => {
        handleProfileClose();
        navigate(ROUTES.PROFILE);
    };

    return (
        <AppBar
            position="fixed"
            elevation={0}
            sx={{
                bgcolor: "#FFFFFF",
                borderBottom: "1px solid #E5E7EB",
                color: "#111827",
                zIndex: (theme) => theme.zIndex.drawer + 1,
            }}
        >
            <Toolbar
                sx={{
                    minHeight: "64px !important",
                    px: { xs: 2, md: 3 },
                    gap: 1,
                }}
            >
                {/* ── Hamburger (mobile/tablet only) ── */}
                <IconButton
                    edge="start"
                    onClick={onMenuClick}
                    sx={{
                        display: { xs: "flex", md: "none" },
                        color: "#6B7280",
                        mr: 0.5,
                        borderRadius: "10px",
                        "&:hover": {
                            bgcolor: "#F3F4F6",
                            color: "#111827",
                        },
                    }}
                >
                    <MenuIcon fontSize="small" />
                </IconButton>

                {/* ── Page title (left) ── */}
                <Box
                    sx={{
                        display: { xs: "none", md: "flex" },
                        flexDirection: "column",
                        justifyContent: "center",
                        mr: 3,
                    }}
                >
                    <Typography
                        sx={{
                            fontSize: "1rem",
                            fontWeight: 600,
                            color: "#111827",
                            lineHeight: 1.3,
                            letterSpacing: "-0.01em",
                            whiteSpace: "nowrap",
                        }}
                    >
                        Enterprise Leave
                    </Typography>
                    <Typography
                        sx={{
                            fontSize: "0.6875rem",
                            color: "#6B7280",
                            fontWeight: 400,
                            lineHeight: 1.2,
                        }}
                    >
                        HR Management System
                    </Typography>
                </Box>

                {/* ── Search bar (center, grows) ── */}
                <Box
                    sx={{
                        flex: 1,
                        maxWidth: { xs: "100%", sm: 480 },
                        mx: "auto",
                    }}
                >
                    <InputBase
                        placeholder="Search employees, departments, leave..."
                        readOnly
                        startAdornment={
                            <InputAdornment
                                position="start"
                                sx={{ ml: 0.5, mr: 0 }}
                            >
                                <SearchIcon
                                    sx={{ color: "#9CA3AF", fontSize: 18 }}
                                />
                            </InputAdornment>
                        }
                        sx={{
                            width: "100%",
                            height: 38,
                            px: 1.5,
                            bgcolor: "#F9FAFB",
                            border: "1.5px solid #E5E7EB",
                            borderRadius: "10px",
                            fontSize: "0.875rem",
                            color: "#6B7280",
                            transition:
                                "border-color 0.15s ease, box-shadow 0.15s ease",
                            cursor: "text",
                            "& .MuiInputBase-input": {
                                pl: 1,
                                py: 0,
                                cursor: "text",
                                "&::placeholder": {
                                    color: "#9CA3AF",
                                    opacity: 1,
                                    fontSize: "0.875rem",
                                },
                            },
                            "&:hover": {
                                borderColor: "#D1D5DB",
                                bgcolor: "#FFFFFF",
                            },
                            "&.Mui-focused": {
                                borderColor: "#2563EB",
                                boxShadow: "0 0 0 3px rgba(37,99,235,0.12)",
                                bgcolor: "#FFFFFF",
                            },
                        }}
                    />
                </Box>

                {/* ── Right section ── */}
                <Box
                    sx={{
                        display: "flex",
                        alignItems: "center",
                        gap: 0.5,
                        ml: 1,
                        flexShrink: 0,
                    }}
                >
                    {/* Notifications */}
                    <Tooltip title="Notifications" arrow>
                        <IconButton
                            onClick={() => navigate(ROUTES.PROFILE)}
                            sx={{
                                width: 38,
                                height: 38,
                                borderRadius: "10px",
                                color: "#6B7280",
                                transition: "all 0.15s ease",
                                "&:hover": {
                                    bgcolor: "#F3F4F6",
                                    color: "#111827",
                                },
                            }}
                        >
                            <Badge
                                badgeContent={unreadCount}
                                color="error"
                                sx={{
                                    "& .MuiBadge-badge": {
                                        fontSize: "0.625rem",
                                        minWidth: 16,
                                        height: 16,
                                        padding: "0 4px",
                                    },
                                }}
                            >
                                <NotificationsNoneIcon sx={{ fontSize: 20 }} />
                            </Badge>
                        </IconButton>
                    </Tooltip>

                    {/* Divider between icons and profile */}
                    <Box
                        sx={{
                            width: 1,
                            height: 24,
                            bgcolor: "#E5E7EB",
                            mx: 0.5,
                        }}
                    />

                    {/* Profile trigger */}
                    <Box
                        onClick={handleProfileOpen}
                        sx={{
                            display: "flex",
                            alignItems: "center",
                            gap: 1,
                            px: 1,
                            py: 0.625,
                            borderRadius: "10px",
                            cursor: "pointer",
                            transition: "background-color 0.15s ease",
                            userSelect: "none",
                            "&:hover": {
                                bgcolor: "#F3F4F6",
                            },
                        }}
                    >
                        <Avatar
                            sx={{
                                width: 32,
                                height: 32,
                                bgcolor: "#2563EB",
                                fontSize: "0.75rem",
                                fontWeight: 700,
                                flexShrink: 0,
                            }}
                        >
                            {auth.user
                                ? getInitials(auth.user.fullName)
                                : "U"}
                        </Avatar>

                        <Box
                            sx={{
                                display: { xs: "none", sm: "flex" },
                                flexDirection: "column",
                                alignItems: "flex-start",
                            }}
                        >
                            <Typography
                                sx={{
                                    fontSize: "0.8125rem",
                                    fontWeight: 600,
                                    color: "#111827",
                                    lineHeight: 1.3,
                                    whiteSpace: "nowrap",
                                }}
                            >
                                {auth.user?.fullName ?? "User"}
                            </Typography>
                            <Typography
                                sx={{
                                    fontSize: "0.6875rem",
                                    color: "#6B7280",
                                    fontWeight: 400,
                                    lineHeight: 1.2,
                                    whiteSpace: "nowrap",
                                }}
                            >
                                {auth.user?.role ?? ""}
                            </Typography>
                        </Box>

                        <KeyboardArrowDownIcon
                            sx={{
                                fontSize: 16,
                                color: "#9CA3AF",
                                display: { xs: "none", sm: "block" },
                                transition: "transform 0.15s ease",
                                transform: profileOpen
                                    ? "rotate(180deg)"
                                    : "rotate(0deg)",
                            }}
                        />
                    </Box>
                </Box>
            </Toolbar>

            {/* ── Profile dropdown menu ── */}
            <Menu
                anchorEl={profileAnchor}
                open={profileOpen}
                onClose={handleProfileClose}
                transformOrigin={{ horizontal: "right", vertical: "top" }}
                anchorOrigin={{ horizontal: "right", vertical: "bottom" }}
                slotProps={{
                    paper: {
                        sx: {
                            mt: 1,
                            minWidth: 220,
                            borderRadius: "14px",
                            border: "1px solid #E5E7EB",
                            boxShadow:
                                "0px 10px 15px -3px rgba(0,0,0,0.07), 0px 4px 6px -2px rgba(0,0,0,0.04)",
                            overflow: "visible",
                        },
                    },
                }}
            >
                {/* User info header */}
                <Box sx={{ px: 2, pt: 1.5, pb: 1.25 }}>
                    <Box
                        sx={{
                            display: "flex",
                            alignItems: "center",
                            gap: 1.25,
                        }}
                    >
                        <Avatar
                            sx={{
                                width: 36,
                                height: 36,
                                bgcolor: "#2563EB",
                                fontSize: "0.8125rem",
                                fontWeight: 700,
                            }}
                        >
                            {auth.user
                                ? getInitials(auth.user.fullName)
                                : "U"}
                        </Avatar>
                        <Box>
                            <Typography
                                sx={{
                                    fontSize: "0.875rem",
                                    fontWeight: 600,
                                    color: "#111827",
                                    lineHeight: 1.3,
                                }}
                            >
                                {auth.user?.fullName ?? "User"}
                            </Typography>
                            <Typography
                                sx={{
                                    fontSize: "0.75rem",
                                    color: "#6B7280",
                                    lineHeight: 1.3,
                                }}
                            >
                                {auth.user?.email ?? ""}
                            </Typography>
                        </Box>
                    </Box>
                </Box>

                <Divider sx={{ borderColor: "#F3F4F6" }} />

                <Box sx={{ py: 0.75, px: 0.75 }}>
                    <MenuItem
                        onClick={handleGoToProfile}
                        sx={{
                            borderRadius: "8px",
                            py: 1,
                            px: 1.5,
                            gap: 0.5,
                            "&:hover": { bgcolor: "#F3F4F6" },
                        }}
                    >
                        <ListItemIcon sx={{ minWidth: 32 }}>
                            <PersonOutlinedIcon
                                sx={{ fontSize: 18, color: "#6B7280" }}
                            />
                        </ListItemIcon>
                        <ListItemText
                            primary={
                                <Typography
                                    sx={{
                                        fontSize: "0.875rem",
                                        fontWeight: 500,
                                        color: "#374151",
                                    }}
                                >
                                    My Profile
                                </Typography>
                            }
                        />
                    </MenuItem>

                    <Divider sx={{ borderColor: "#F3F4F6", my: 0.75 }} />

                    <MenuItem
                        onClick={handleLogout}
                        sx={{
                            borderRadius: "8px",
                            py: 1,
                            px: 1.5,
                            gap: 0.5,
                            "&:hover": {
                                bgcolor: "#FEF2F2",
                                "& .logout-icon, & .logout-text": {
                                    color: "#EF4444",
                                },
                            },
                        }}
                    >
                        <ListItemIcon sx={{ minWidth: 32 }}>
                            <LogoutIcon
                                className="logout-icon"
                                sx={{
                                    fontSize: 18,
                                    color: "#6B7280",
                                    transition: "color 0.15s ease",
                                }}
                            />
                        </ListItemIcon>
                        <ListItemText
                            primary={
                                <Typography
                                    className="logout-text"
                                    sx={{
                                        fontSize: "0.875rem",
                                        fontWeight: 500,
                                        color: "#374151",
                                        transition: "color 0.15s ease",
                                    }}
                                >
                                    Sign Out
                                </Typography>
                            }
                        />
                    </MenuItem>
                </Box>
            </Menu>
        </AppBar>
    );
}