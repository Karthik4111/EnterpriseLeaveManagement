import { Box, Toolbar } from "@mui/material";
import { Outlet } from "react-router-dom";
import { useState } from "react";

import Navbar from "@/components/layout/Navbar/Navbar";
import Sidebar from "@/components/layout/Sidebar/Sidebar";
import Footer from "@/components/layout/Footer/Footer";

export default function MainLayout() {
    const [sidebarOpen, setSidebarOpen] = useState(false);

    return (
        <Box sx={{ display: "flex", bgcolor: "background.default" }}>
            {/* ── Fixed Navbar (spans full width above everything) ── */}
            <Navbar onMenuClick={() => setSidebarOpen(true)} />

            {/* ── Sidebar (permanent on md+, temporary on mobile) ── */}
            <Sidebar
                open={sidebarOpen}
                onClose={() => setSidebarOpen(false)}
            />

            {/* ── Main content column ── */}
            <Box
                component="main"
                sx={{
                    flexGrow: 1,
                    minHeight: "100vh",
                    display: "flex",
                    flexDirection: "column",
                    bgcolor: "background.default",
                    // On desktop the permanent sidebar occupies space via flex,
                    // so no explicit margin needed. On mobile the sidebar is
                    // a temporary drawer (portaled) and doesn't consume flex space.
                    minWidth: 0, // prevent overflow in flex child
                    overflow: "hidden",
                }}
            >
                {/* Spacer that matches the fixed Navbar height */}
                <Toolbar sx={{ minHeight: "64px !important", flexShrink: 0 }} />

                {/* ── Scrollable page area ── */}
                <Box
                    sx={{
                        flexGrow: 1,
                        overflowY: "auto",
                        overflowX: "hidden",
                        display: "flex",
                        flexDirection: "column",
                        // Custom scrollbar
                        scrollbarWidth: "thin",
                        scrollbarColor: "#D1D5DB #F5F7FB",
                        "&::-webkit-scrollbar": { width: 6 },
                        "&::-webkit-scrollbar-track": { background: "#F5F7FB" },
                        "&::-webkit-scrollbar-thumb": {
                            background: "#D1D5DB",
                            borderRadius: 3,
                        },
                    }}
                >
                    {/* ── Page content wrapper ── */}
                    <Box
                        sx={{
                            flexGrow: 1,
                            px: { xs: 2, sm: 3, md: 4 },
                            pt: { xs: 2.5, md: 3.5 },
                            pb: { xs: 2, md: 3 },
                            // Comfortable max reading width; pages can override if needed
                            maxWidth: 1440,
                            width: "100%",
                            mx: "auto",
                            boxSizing: "border-box",
                        }}
                    >
                        <Outlet />
                    </Box>

                    <Footer />
                </Box>
            </Box>
        </Box>
    );
}