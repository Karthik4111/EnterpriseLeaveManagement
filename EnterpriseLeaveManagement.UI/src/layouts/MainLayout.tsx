import {
    Box,
    Drawer,
    Toolbar
} from "@mui/material";

import type { ReactNode } from "react";

import Sidebar from "@/components/layout/Sidebar";
import AppHeader from "@/components/layout/AppHeader";
import MainContent from "@/components/layout/MainContent";

const drawerWidth = 240;

interface Props {
    children: ReactNode;
}

function MainLayout({ children }: Props) {

    return (
        <Box sx={{ display: "flex" }}>

            <AppHeader />

            <Drawer
                variant="permanent"
                sx={{
                    width: drawerWidth,
                    flexShrink: 0,
                    "& .MuiDrawer-paper": {
                        width: drawerWidth,
                        boxSizing: "border-box",
                    },
                }}
            >
                <Toolbar />
                <Sidebar />
            </Drawer>

            <MainContent>
                {children}
            </MainContent>

        </Box>
    );
}

export default MainLayout;