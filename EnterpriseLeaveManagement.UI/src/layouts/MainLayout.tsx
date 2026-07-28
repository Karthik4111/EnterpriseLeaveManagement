import { Box, Toolbar } from "@mui/material";
import { Outlet } from "react-router-dom";
import { useState } from "react";

import Navbar from "@/components/layout/Navbar/Navbar";
import Sidebar from "@/components/layout/Sidebar/Sidebar";
import Footer from "@/components/layout/Footer/Footer";

export default function MainLayout() {
    const [sidebarOpen, setSidebarOpen] = useState(false);

    return (
        <Box sx={{ display: "flex" }}>
            <Navbar
                onMenuClick={() =>
                    setSidebarOpen(true)
                }
            />

            <Sidebar
                open={sidebarOpen}
                onClose={() =>
                    setSidebarOpen(false)
                }
            />

            <Box
                component="main"
                sx={{
                    flexGrow: 1,
                    minHeight: "100vh",
                    display: "flex",
                    flexDirection: "column",
                    bgcolor: "background.default",
                }}
            >
                <Toolbar />

                <Box
                    sx={{
                        flexGrow: 1,
                        p: 3,
                    }}
                >
                    <Outlet />
                </Box>

                <Footer />
            </Box>
        </Box>
    );
}