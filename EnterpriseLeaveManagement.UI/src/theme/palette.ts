import type { PaletteOptions } from "@mui/material/styles";

const palette: PaletteOptions = {
    mode: "light",

    primary: {
        main: "#2563EB",
        light: "#3B82F6",
        dark: "#1D4ED8",
        contrastText: "#ffffff",
    },

    secondary: {
        main: "#6366F1",
        light: "#818CF8",
        dark: "#4F46E5",
        contrastText: "#ffffff",
    },

    success: {
        main: "#22C55E",
        light: "#4ADE80",
        dark: "#16A34A",
        contrastText: "#ffffff",
    },

    warning: {
        main: "#F59E0B",
        light: "#FCD34D",
        dark: "#D97706",
        contrastText: "#ffffff",
    },

    error: {
        main: "#EF4444",
        light: "#FCA5A5",
        dark: "#DC2626",
        contrastText: "#ffffff",
    },

    background: {
        default: "#F5F7FB",
        paper: "#FFFFFF",
    },

    divider: "#E5E7EB",

    text: {
        primary: "#111827",
        secondary: "#6B7280",
        disabled: "#9CA3AF",
    },
};

export default palette;