import { useContext } from "react";

import SnackbarContext from "@/context/SnackbarContext";

export default function useSnackbar() {
    const context =
        useContext(SnackbarContext);

    if (!context) {
        throw new Error(
            "useSnackbar must be used within SnackbarProvider."
        );
    }

    return context;
}