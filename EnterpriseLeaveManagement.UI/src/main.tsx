import React from "react";
import ReactDOM from "react-dom/client";

import { QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { ToastContainer } from "react-toastify";

import App from "./App";

import { AuthProvider } from "@/context/AuthContext";
import { queryClient } from "@/lib/reactQuery";

import "react-toastify/dist/ReactToastify.css";
import "./index.css";

ReactDOM.createRoot(
    document.getElementById("root")!
).render(
    <React.StrictMode>
        <QueryClientProvider client={queryClient}>
            <AuthProvider>
                <App />

                <ToastContainer
                    position="top-right"
                    autoClose={3000}
                    newestOnTop
                    pauseOnHover
                    closeOnClick
                    theme="colored"
                />

                <ReactQueryDevtools initialIsOpen={false} />
            </AuthProvider>
        </QueryClientProvider>
    </React.StrictMode>
);