import DashboardIcon from "@mui/icons-material/Dashboard";
import GroupIcon from "@mui/icons-material/Group";
import BusinessIcon from "@mui/icons-material/Business";
import EventNoteIcon from "@mui/icons-material/EventNote";
import PersonIcon from "@mui/icons-material/Person";
import ApprovalIcon from "@mui/icons-material/Approval";
import BeachAccessIcon from "@mui/icons-material/BeachAccess";

import {
    Box,
    Divider,
    Drawer,
    List,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Toolbar,
} from "@mui/material";
import type { ReactNode } from "react";

import { Link, useLocation } from "react-router-dom";

import { ROUTES } from "@/constants/routes";
import useAuth from "@/hooks/useAuth";
import { ROLES } from "@/constants/roles";

const drawerWidth = 260;

interface SidebarProps {
    open: boolean;
    onClose: () => void;
}

interface SidebarItem {
    text: string;
    icon: ReactNode;
    path: string;
}

export default function Sidebar({
    open,
    onClose,
}: SidebarProps) {
    const location = useLocation();
    const { auth } = useAuth();

    const menuItems: SidebarItem[] = [
        {
            text: "Dashboard",
            icon: <DashboardIcon />,
            path: ROUTES.DASHBOARD,
        },
        {
            text: "Employees",
            icon: <GroupIcon />,
            path: ROUTES.EMPLOYEES,
        },
        {
            text: "Departments",
            icon: <BusinessIcon />,
            path: ROUTES.DEPARTMENTS,
        },
        {
            text: "Request Leave",
            icon: <EventNoteIcon />,
            path: ROUTES.LEAVE_REQUEST,
        },
        {
            text: "My Leaves",
            icon: <EventNoteIcon />,
            path: ROUTES.MY_LEAVES,
        },
        {
            text: "Leave Balance",
            icon: <BeachAccessIcon />,
            path: ROUTES.LEAVE_BALANCE,
        },
        {
            text: "Profile",
            icon: <PersonIcon />,
            path: ROUTES.PROFILE,
        },
    ];

    if (
        auth.user?.role === ROLES.MANAGER ||
        auth.user?.role === ROLES.ADMIN
    ) {
        menuItems.splice(5, 0, {
            text: "Leave Approvals",
            icon: <ApprovalIcon />,
            path: ROUTES.LEAVE_APPROVALS,
        });
    }

    return (
        <Drawer
            open={open}
            onClose={onClose}
            variant="temporary"
            sx={{
                "& .MuiDrawer-paper": {
                    width: drawerWidth,
                },
            }}
        >
            <Toolbar />

            <Box>
                <List>
                    {menuItems.map((item) => (
                        <ListItemButton
                            key={item.text}
                            component={Link}
                            to={item.path}
                            selected={location.pathname === item.path}
                            onClick={onClose}
                        >
                            <ListItemIcon>
                                {item.icon}
                            </ListItemIcon>

                            <ListItemText
                                primary={item.text}
                            />
                        </ListItemButton>
                    ))}
                </List>

                <Divider />
            </Box>
        </Drawer>
    );
}