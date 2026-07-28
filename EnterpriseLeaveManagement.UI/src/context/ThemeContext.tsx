import type { PropsWithChildren } from "react";
import { CssBaseline, ThemeProvider } from "@mui/material";
import theme from "@/theme/theme";

interface ThemeContextProps extends PropsWithChildren {}

export default function AppThemeProvider({
    children,
}: ThemeContextProps) {
    return (
        <ThemeProvider theme={theme}>
            <CssBaseline />
            {children}
        </ThemeProvider>
    );
}