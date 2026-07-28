import {
    Alert,
    Snackbar,
} from "@mui/material";

import {
    createContext,
    useCallback,
    useMemo,
    useState,
    type ReactNode,
} from "react";

type SnackbarSeverity =
    | "success"
    | "error"
    | "warning"
    | "info";

interface SnackbarState {
    open: boolean;
    message: string;
    severity: SnackbarSeverity;
}

interface SnackbarContextType {
    showSuccess: (message: string) => void;
    showError: (message: string) => void;
    showWarning: (message: string) => void;
    showInfo: (message: string) => void;
}

const SnackbarContext =
    createContext<SnackbarContextType | null>(
        null
    );

interface SnackbarProviderProps {
    children: ReactNode;
}

export function SnackbarProvider({
    children,
}: SnackbarProviderProps) {
    const [snackbar, setSnackbar] =
        useState<SnackbarState>({
            open: false,
            message: "",
            severity: "success",
        });

    const showSnackbar = useCallback(
        (
            message: string,
            severity: SnackbarSeverity
        ) => {
            setSnackbar({
                open: true,
                message,
                severity,
            });
        },
        []
    );

    const handleClose = () => {
        setSnackbar((previous) => ({
            ...previous,
            open: false,
        }));
    };

    const value = useMemo(
        () => ({
            showSuccess: (message: string) =>
                showSnackbar(
                    message,
                    "success"
                ),

            showError: (message: string) =>
                showSnackbar(
                    message,
                    "error"
                ),

            showWarning: (message: string) =>
                showSnackbar(
                    message,
                    "warning"
                ),

            showInfo: (message: string) =>
                showSnackbar(
                    message,
                    "info"
                ),
        }),
        [showSnackbar]
    );

    return (
        <SnackbarContext.Provider
            value={value}
        >
            {children}

            <Snackbar
                open={snackbar.open}
                autoHideDuration={3000}
                onClose={handleClose}
                anchorOrigin={{
                    vertical: "top",
                    horizontal: "right",
                }}
            >
                <Alert
                    severity={
                        snackbar.severity
                    }
                    onClose={handleClose}
                    variant="filled"
                    sx={{
                        width: "100%",
                    }}
                >
                    {snackbar.message}
                </Alert>
            </Snackbar>
        </SnackbarContext.Provider>
    );
}

export default SnackbarContext;