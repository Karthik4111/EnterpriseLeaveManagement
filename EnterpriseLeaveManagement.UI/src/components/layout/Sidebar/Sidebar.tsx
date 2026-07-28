import DashboardIcon from "@mui/icons-material/Dashboard";
import GroupIcon from "@mui/icons-material/Group";
import BusinessIcon from "@mui/icons-material/Business";
import EventNoteIcon from "@mui/icons-material/EventNote";
import PersonIcon from "@mui/icons-material/Person";

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

import { Link, useLocation } from "react-router-dom";

import { ROUTES } from "@/constants/routes";

const drawerWidth = 260;

interface SidebarProps {
    open: boolean;
    onClose: () => void;
}

const menuItems = [
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
        text: "Leave",
        icon: <EventNoteIcon />,
        path: ROUTES.MY_LEAVES,
    },
    {
        text: "Profile",
        icon: <PersonIcon />,
        path: ROUTES.PROFILE,
    },
];

export default function Sidebar({
    open,
    onClose,
}: SidebarProps) {
    const location = useLocation();

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
                            selected={
                                location.pathname === item.path
                            }
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