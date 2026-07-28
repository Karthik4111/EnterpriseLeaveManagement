import { useMemo } from "react";
import type { ReactNode } from "react";

import DashboardIcon from "@mui/icons-material/Dashboard";
import GroupIcon from "@mui/icons-material/Group";
import BusinessIcon from "@mui/icons-material/Business";
import EventNoteIcon from "@mui/icons-material/EventNote";
import PersonIcon from "@mui/icons-material/Person";
import ApprovalIcon from "@mui/icons-material/Approval";
import BeachAccessIcon from "@mui/icons-material/BeachAccess";
import WorkspacesOutlinedIcon from "@mui/icons-material/WorkspacesOutlined";

import {
    Avatar,
    Box,
    Divider,
    Drawer,
    List,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Toolbar,
    Typography,
} from "@mui/material";

import { Link, useLocation } from "react-router-dom";

import { ROUTES } from "@/constants/routes";
import useAuth from "@/hooks/useAuth";
import { ROLES } from "@/constants/roles";

const DRAWER_WIDTH = 260;

// Colours kept as constants so they are easy to update
const C = {
    bg: "#111827",
    border: "rgba(255,255,255,0.06)",
    activeBg: "#1D4ED8",
    activeHoverBg: "#1E40AF",
    hoverBg: "rgba(255,255,255,0.06)",
    activeText: "#FFFFFF",
    inactiveText: "#9CA3AF",
    activeIcon: "#FFFFFF",
    inactiveIcon: "#6B7280",
    indicator: "#60A5FA",
    sectionLabel: "#4B5563",
    userNameText: "#F3F4F6",
    userRoleText: "#6B7280",
    versionText: "#4B5563",
    divider: "rgba(255,255,255,0.06)",
    avatarBg: "#2563EB",
    logoBg: "#2563EB",
};

interface SidebarProps {
    open: boolean;
    onClose: () => void;
}

interface SidebarItem {
    text: string;
    icon: ReactNode;
    path: string;
}

interface SidebarSection {
    label: string;
    items: SidebarItem[];
}

function getInitials(name: string): string {
    return name
        .split(" ")
        .map((n) => n[0])
        .slice(0, 2)
        .join("")
        .toUpperCase();
}

export default function Sidebar({ open, onClose }: SidebarProps) {
    const location = useLocation();
    const { auth } = useAuth();

    const isManagerOrAdmin =
        auth.user?.role === ROLES.MANAGER ||
        auth.user?.role === ROLES.ADMIN;

    const sections: SidebarSection[] = useMemo(() => {
        const leaveItems: SidebarItem[] = [
            {
                text: "Request Leave",
                icon: <EventNoteIcon fontSize="small" />,
                path: ROUTES.LEAVE_REQUEST,
            },
            {
                text: "My Leaves",
                icon: <EventNoteIcon fontSize="small" />,
                path: ROUTES.MY_LEAVES,
            },
            {
                text: "Leave Balance",
                icon: <BeachAccessIcon fontSize="small" />,
                path: ROUTES.LEAVE_BALANCE,
            },
            ...(isManagerOrAdmin
                ? [
                      {
                          text: "Leave Approvals",
                          icon: <ApprovalIcon fontSize="small" />,
                          path: ROUTES.LEAVE_APPROVALS,
                      },
                  ]
                : []),
        ];

        return [
            {
                label: "Main",
                items: [
                    {
                        text: "Dashboard",
                        icon: <DashboardIcon fontSize="small" />,
                        path: ROUTES.DASHBOARD,
                    },
                ],
            },
            {
                label: "People",
                items: [
                    {
                        text: "Employees",
                        icon: <GroupIcon fontSize="small" />,
                        path: ROUTES.EMPLOYEES,
                    },
                    {
                        text: "Departments",
                        icon: <BusinessIcon fontSize="small" />,
                        path: ROUTES.DEPARTMENTS,
                    },
                ],
            },
            {
                label: "Leave",
                items: leaveItems,
            },
            {
                label: "Account",
                items: [
                    {
                        text: "Profile",
                        icon: <PersonIcon fontSize="small" />,
                        path: ROUTES.PROFILE,
                    },
                ],
            },
        ];
    }, [isManagerOrAdmin]);

    const isActive = (path: string) => location.pathname === path;

    const drawerContent = (
        <Box
            sx={{
                display: "flex",
                flexDirection: "column",
                height: "100%",
                bgcolor: C.bg,
                color: C.activeText,
                overflow: "hidden",
            }}
        >
            {/* Spacer so content starts below the fixed AppBar */}
            <Toolbar
                sx={{
                    minHeight: "64px !important",
                    flexShrink: 0,
                    borderBottom: `1px solid ${C.border}`,
                    display: "flex",
                    alignItems: "center",
                    px: 2.5,
                    gap: 1.5,
                }}
            >
                {/* Brand logo */}
                <Box
                    sx={{
                        width: 34,
                        height: 34,
                        borderRadius: "10px",
                        bgcolor: C.logoBg,
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        flexShrink: 0,
                        boxShadow: "0 2px 8px rgba(37,99,235,0.45)",
                    }}
                >
                    <WorkspacesOutlinedIcon sx={{ color: "#fff", fontSize: 18 }} />
                </Box>

                <Box>
                    <Typography
                        sx={{
                            fontSize: "0.875rem",
                            fontWeight: 700,
                            color: "#F9FAFB",
                            lineHeight: 1.2,
                            letterSpacing: "-0.01em",
                        }}
                    >
                        Enterprise Leave
                    </Typography>
                    <Typography
                        sx={{
                            fontSize: "0.6875rem",
                            color: C.userRoleText,
                            fontWeight: 400,
                            lineHeight: 1.3,
                        }}
                    >
                        HR Management System
                    </Typography>
                </Box>
            </Toolbar>

            {/* Scrollable menu area */}
            <Box
                sx={{
                    flex: 1,
                    overflowY: "auto",
                    overflowX: "hidden",
                    py: 1.5,
                    "&::-webkit-scrollbar": { width: 0 },
                    scrollbarWidth: "none",
                }}
            >
                {sections.map((section, sIdx) => (
                    <Box key={section.label} sx={{ mb: 0.5 }}>
                        {/* Section label */}
                        <Typography
                            sx={{
                                px: 2.5,
                                pt: sIdx === 0 ? 0.5 : 1.5,
                                pb: 0.75,
                                fontSize: "0.625rem",
                                fontWeight: 600,
                                letterSpacing: "0.1em",
                                textTransform: "uppercase",
                                color: C.sectionLabel,
                                userSelect: "none",
                            }}
                        >
                            {section.label}
                        </Typography>

                        <List disablePadding sx={{ px: 1.5 }}>
                            {section.items.map((item) => {
                                const active = isActive(item.path);
                                return (
                                    <ListItemButton
                                        key={item.text}
                                        component={Link}
                                        to={item.path}
                                        onClick={onClose}
                                        selected={active}
                                        sx={{
                                            borderRadius: "10px",
                                            mb: "3px",
                                            px: 1.5,
                                            py: 0.875,
                                            minHeight: 40,
                                            position: "relative",
                                            transition:
                                                "background-color 0.15s ease, box-shadow 0.15s ease, color 0.15s ease",
                                            color: active
                                                ? C.activeText
                                                : C.inactiveText,
                                            bgcolor: active
                                                ? C.activeBg
                                                : "transparent",
                                            boxShadow: active
                                                ? "0 2px 8px rgba(29,78,216,0.35)"
                                                : "none",
                                            "&:hover": {
                                                bgcolor: active
                                                    ? C.activeHoverBg
                                                    : C.hoverBg,
                                                color: C.activeText,
                                            },
                                            "&.Mui-selected": {
                                                bgcolor: C.activeBg,
                                                color: C.activeText,
                                                "&:hover": {
                                                    bgcolor: C.activeHoverBg,
                                                },
                                                "& .MuiListItemIcon-root": {
                                                    color: C.activeIcon,
                                                },
                                            },
                                        }}
                                    >
                                        {/* Active left-edge indicator */}
                                        {active && (
                                            <Box
                                                sx={{
                                                    position: "absolute",
                                                    left: -6,
                                                    top: "50%",
                                                    transform: "translateY(-50%)",
                                                    width: 3,
                                                    height: 18,
                                                    borderRadius: "0 3px 3px 0",
                                                    bgcolor: C.indicator,
                                                    flexShrink: 0,
                                                }}
                                            />
                                        )}

                                        <ListItemIcon
                                            sx={{
                                                minWidth: 32,
                                                color: active
                                                    ? C.activeIcon
                                                    : C.inactiveIcon,
                                                transition: "color 0.15s ease",
                                            }}
                                        >
                                            {item.icon}
                                        </ListItemIcon>

                                        <ListItemText
                                            primary={
                                                <Typography
                                                    sx={{
                                                        fontSize: "0.875rem",
                                                        fontWeight:
                                                            active ? 600 : 400,
                                                        letterSpacing:
                                                            "0.005em",
                                                        lineHeight: 1.4,
                                                        color: "inherit",
                                                    }}
                                                >
                                                    {item.text}
                                                </Typography>
                                            }
                                        />
                                    </ListItemButton>
                                );
                            })}
                        </List>
                    </Box>
                ))}
            </Box>

            {/* Bottom user / footer section */}
            <Box
                sx={{
                    flexShrink: 0,
                    borderTop: `1px solid ${C.divider}`,
                    pt: 1.5,
                    pb: 2,
                    px: 2,
                }}
            >
                {auth.user && (
                    <Box
                        sx={{
                            display: "flex",
                            alignItems: "center",
                            gap: 1.25,
                            px: 1,
                            py: 0.875,
                            borderRadius: "10px",
                            cursor: "default",
                            transition: "background-color 0.15s ease",
                            "&:hover": {
                                bgcolor: "rgba(255,255,255,0.05)",
                            },
                        }}
                    >
                        <Avatar
                            sx={{
                                width: 32,
                                height: 32,
                                bgcolor: C.avatarBg,
                                fontSize: "0.75rem",
                                fontWeight: 700,
                                flexShrink: 0,
                                boxShadow: "0 1px 4px rgba(0,0,0,0.3)",
                            }}
                        >
                            {getInitials(auth.user.fullName)}
                        </Avatar>

                        <Box sx={{ overflow: "hidden", flex: 1 }}>
                            <Typography
                                sx={{
                                    fontSize: "0.8125rem",
                                    fontWeight: 600,
                                    color: C.userNameText,
                                    lineHeight: 1.3,
                                    overflow: "hidden",
                                    textOverflow: "ellipsis",
                                    whiteSpace: "nowrap",
                                }}
                            >
                                {auth.user.fullName}
                            </Typography>
                            <Typography
                                sx={{
                                    fontSize: "0.6875rem",
                                    color: C.userRoleText,
                                    lineHeight: 1.3,
                                    fontWeight: 400,
                                }}
                            >
                                {auth.user.role}
                            </Typography>
                        </Box>
                    </Box>
                )}

                <Divider
                    sx={{
                        borderColor: C.divider,
                        my: 1,
                    }}
                />

                <Typography
                    sx={{
                        px: 1,
                        fontSize: "0.6875rem",
                        color: C.versionText,
                        fontWeight: 500,
                        letterSpacing: "0.02em",
                    }}
                >
                    Version 1.0
                </Typography>
            </Box>
        </Box>
    );

    const paperSx = {
        width: DRAWER_WIDTH,
        boxSizing: "border-box",
        bgcolor: C.bg,
        border: "none",
        borderRight: `1px solid ${C.border}`,
        boxShadow: "none",
        backgroundImage: "none",
    } as const;

    return (
        <>
            {/* ── Permanent sidebar on desktop (md and above) ── */}
            <Drawer
                variant="permanent"
                sx={{
                    display: { xs: "none", md: "block" },
                    width: DRAWER_WIDTH,
                    flexShrink: 0,
                    "& .MuiDrawer-paper": paperSx,
                }}
            >
                {drawerContent}
            </Drawer>

            {/* ── Temporary drawer on tablet / mobile ── */}
            <Drawer
                variant="temporary"
                open={open}
                onClose={onClose}
                ModalProps={{ keepMounted: true }}
                sx={{
                    display: { xs: "block", md: "none" },
                    "& .MuiDrawer-paper": {
                        ...paperSx,
                        borderRight: "none",
                        boxShadow: "4px 0 32px rgba(0,0,0,0.45)",
                    },
                }}
            >
                {drawerContent}
            </Drawer>
        </>
    );
}