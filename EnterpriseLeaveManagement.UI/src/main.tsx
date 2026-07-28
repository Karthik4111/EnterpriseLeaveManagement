import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router-dom";

import App from "./App";

import QueryProvider from "@/providers/QueryProvider";
import AppThemeProvider from "@/context/ThemeContext";
import { AuthProvider } from "@/context/AuthContext";
import { SnackbarProvider } from "@/context/SnackbarContext";

import "@/api/interceptors";
import "./index.css";

createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <BrowserRouter>
            <QueryProvider>
                <AppThemeProvider>
                    <AuthProvider>
                        <SnackbarProvider>
                            <App />
                        </SnackbarProvider>
                    </AuthProvider>
                </AppThemeProvider>
            </QueryProvider>
        </BrowserRouter>
    </StrictMode>
);