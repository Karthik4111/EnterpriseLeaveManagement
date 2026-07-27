import {
    Box,
    List,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Toolbar,
} from "@mui/material";

import { Link, useLocation } from "react-router-dom";

import { navigationItems } from "@/navigation/navigationItems";

const drawerWidth = 240;

function Sidebar() {
    const location = useLocation();

    return (
        <Box
            sx={{
                width: drawerWidth,
                flexShrink: 0,
            }}
        >
            <Toolbar />

            <List>
                {navigationItems.map((item) => {
                    const Icon = item.icon;

                    return (
                        <ListItemButton
                            key={item.path}
                            component={Link}
                            to={item.path}
                            selected={location.pathname === item.path}
                        >
                            <ListItemIcon>
                                <Icon />
                            </ListItemIcon>

                            <ListItemText primary={item.title} />
                        </ListItemButton>
                    );
                })}
            </List>
        </Box>
    );
}

export default Sidebar;