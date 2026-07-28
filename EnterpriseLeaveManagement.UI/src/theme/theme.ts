import { createTheme } from "@mui/material/styles";
import palette from "./palette";

const theme = createTheme({
    palette,

    shape: {
        borderRadius: 16,
    },

    typography: {
        fontFamily: "'Inter', 'Roboto', 'Helvetica', 'Arial', sans-serif",

        h1: {
            fontSize: "2.25rem",
            fontWeight: 700,
            lineHeight: 1.2,
            letterSpacing: "-0.02em",
            color: "#111827",
        },

        h2: {
            fontSize: "1.875rem",
            fontWeight: 700,
            lineHeight: 1.25,
            letterSpacing: "-0.015em",
            color: "#111827",
        },

        h3: {
            fontSize: "1.5rem",
            fontWeight: 600,
            lineHeight: 1.3,
            letterSpacing: "-0.01em",
            color: "#111827",
        },

        h4: {
            fontSize: "1.25rem",
            fontWeight: 600,
            lineHeight: 1.4,
            letterSpacing: "-0.01em",
            color: "#111827",
        },

        h5: {
            fontSize: "1.125rem",
            fontWeight: 600,
            lineHeight: 1.4,
            color: "#111827",
        },

        h6: {
            fontSize: "1rem",
            fontWeight: 600,
            lineHeight: 1.5,
            color: "#111827",
        },

        body1: {
            fontSize: "0.9375rem",
            fontWeight: 400,
            lineHeight: 1.6,
            color: "#111827",
        },

        body2: {
            fontSize: "0.875rem",
            fontWeight: 400,
            lineHeight: 1.6,
            color: "#6B7280",
        },

        caption: {
            fontSize: "0.75rem",
            fontWeight: 400,
            lineHeight: 1.5,
            color: "#6B7280",
        },

        overline: {
            fontSize: "0.6875rem",
            fontWeight: 600,
            lineHeight: 1.5,
            letterSpacing: "0.08em",
            textTransform: "uppercase",
        },

        button: {
            fontSize: "0.875rem",
            fontWeight: 500,
            lineHeight: 1.5,
            letterSpacing: "0.01em",
            textTransform: "none",
        },
    },

    shadows: [
        "none",
        "0px 1px 2px rgba(0,0,0,0.05)",
        "0px 1px 3px rgba(0,0,0,0.07), 0px 1px 2px rgba(0,0,0,0.04)",
        "0px 4px 6px -1px rgba(0,0,0,0.07), 0px 2px 4px -1px rgba(0,0,0,0.04)",
        "0px 10px 15px -3px rgba(0,0,0,0.07), 0px 4px 6px -2px rgba(0,0,0,0.04)",
        "0px 20px 25px -5px rgba(0,0,0,0.07), 0px 10px 10px -5px rgba(0,0,0,0.03)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
        "0px 25px 50px -12px rgba(0,0,0,0.15)",
    ],

    components: {
        MuiCssBaseline: {
            styleOverrides: {
                body: {
                    backgroundColor: "#F5F7FB",
                    scrollbarWidth: "thin",
                    scrollbarColor: "#D1D5DB #F3F4F6",
                    "&::-webkit-scrollbar": {
                        width: 6,
                    },
                    "&::-webkit-scrollbar-track": {
                        background: "#F3F4F6",
                    },
                    "&::-webkit-scrollbar-thumb": {
                        background: "#D1D5DB",
                        borderRadius: 3,
                    },
                },
            },
        },

        MuiPaper: {
            defaultProps: {
                elevation: 0,
            },
            styleOverrides: {
                root: {
                    borderRadius: 16,
                    border: "1px solid #E5E7EB",
                    backgroundImage: "none",
                },
                elevation1: {
                    boxShadow: "0px 1px 3px rgba(0,0,0,0.07), 0px 1px 2px rgba(0,0,0,0.04)",
                    border: "1px solid #E5E7EB",
                },
                elevation2: {
                    boxShadow: "0px 4px 6px -1px rgba(0,0,0,0.07), 0px 2px 4px -1px rgba(0,0,0,0.04)",
                    border: "1px solid #E5E7EB",
                },
            },
        },

        MuiCard: {
            defaultProps: {
                elevation: 0,
            },
            styleOverrides: {
                root: {
                    borderRadius: 16,
                    border: "1px solid #E5E7EB",
                    boxShadow: "0px 1px 3px rgba(0,0,0,0.07), 0px 1px 2px rgba(0,0,0,0.04)",
                    transition: "box-shadow 0.2s ease, transform 0.2s ease",
                    "&:hover": {
                        boxShadow: "0px 10px 15px -3px rgba(0,0,0,0.07), 0px 4px 6px -2px rgba(0,0,0,0.04)",
                        transform: "translateY(-1px)",
                    },
                },
            },
        },

        MuiCardContent: {
            styleOverrides: {
                root: {
                    padding: "24px",
                    "&:last-child": {
                        paddingBottom: "24px",
                    },
                },
            },
        },

        MuiButton: {
            defaultProps: {
                variant: "contained",
                disableElevation: true,
            },
            styleOverrides: {
                root: {
                    borderRadius: 10,
                    padding: "8px 20px",
                    fontWeight: 500,
                    fontSize: "0.875rem",
                    letterSpacing: "0.01em",
                    transition: "all 0.15s ease",
                    textTransform: "none",
                },
                contained: {
                    boxShadow: "0px 1px 2px rgba(0,0,0,0.08)",
                    "&:hover": {
                        boxShadow: "0px 4px 6px -1px rgba(0,0,0,0.1), 0px 2px 4px -1px rgba(0,0,0,0.06)",
                        transform: "translateY(-1px)",
                    },
                    "&:active": {
                        transform: "translateY(0)",
                        boxShadow: "0px 1px 2px rgba(0,0,0,0.08)",
                    },
                },
                outlined: {
                    borderWidth: "1.5px",
                    "&:hover": {
                        borderWidth: "1.5px",
                        backgroundColor: "rgba(37,99,235,0.04)",
                    },
                },
                text: {
                    "&:hover": {
                        backgroundColor: "rgba(37,99,235,0.06)",
                    },
                },
                sizeSmall: {
                    padding: "5px 14px",
                    fontSize: "0.8125rem",
                    borderRadius: 8,
                },
                sizeLarge: {
                    padding: "11px 28px",
                    fontSize: "0.9375rem",
                    borderRadius: 12,
                },
            },
        },

        MuiTextField: {
            defaultProps: {
                fullWidth: true,
                size: "small",
            },
        },

        MuiOutlinedInput: {
            styleOverrides: {
                root: {
                    borderRadius: 10,
                    fontSize: "0.9375rem",
                    backgroundColor: "#FFFFFF",
                    transition: "box-shadow 0.15s ease, border-color 0.15s ease",
                    "& fieldset": {
                        borderColor: "#D1D5DB",
                        borderWidth: "1.5px",
                    },
                    "&:hover fieldset": {
                        borderColor: "#9CA3AF",
                    },
                    "&.Mui-focused fieldset": {
                        borderColor: "#2563EB",
                        borderWidth: "2px",
                    },
                    "&.Mui-focused": {
                        boxShadow: "0 0 0 3px rgba(37,99,235,0.12)",
                    },
                },
                input: {
                    padding: "9px 14px",
                },
            },
        },

        MuiInputLabel: {
            styleOverrides: {
                root: {
                    fontSize: "0.9375rem",
                    color: "#6B7280",
                    "&.Mui-focused": {
                        color: "#2563EB",
                    },
                },
            },
        },

        MuiSelect: {
            styleOverrides: {
                root: {
                    borderRadius: 10,
                },
            },
        },

        MuiChip: {
            styleOverrides: {
                root: {
                    borderRadius: 8,
                    fontWeight: 500,
                    fontSize: "0.8125rem",
                    height: 26,
                },
                colorPrimary: {
                    backgroundColor: "#EFF6FF",
                    color: "#1D4ED8",
                    border: "1px solid #BFDBFE",
                },
                colorSecondary: {
                    backgroundColor: "#EEF2FF",
                    color: "#4338CA",
                    border: "1px solid #C7D2FE",
                },
                colorSuccess: {
                    backgroundColor: "#F0FDF4",
                    color: "#15803D",
                    border: "1px solid #BBF7D0",
                },
                colorWarning: {
                    backgroundColor: "#FFFBEB",
                    color: "#B45309",
                    border: "1px solid #FDE68A",
                },
                colorError: {
                    backgroundColor: "#FEF2F2",
                    color: "#B91C1C",
                    border: "1px solid #FECACA",
                },
            },
        },

        MuiDialog: {
            styleOverrides: {
                paper: {
                    borderRadius: 20,
                    boxShadow: "0px 20px 25px -5px rgba(0,0,0,0.10), 0px 10px 10px -5px rgba(0,0,0,0.04)",
                    border: "1px solid #E5E7EB",
                },
            },
        },

        MuiDialogTitle: {
            styleOverrides: {
                root: {
                    padding: "24px 28px 16px",
                    fontSize: "1.125rem",
                    fontWeight: 600,
                    color: "#111827",
                },
            },
        },

        MuiDialogContent: {
            styleOverrides: {
                root: {
                    padding: "8px 28px 20px",
                },
            },
        },

        MuiDialogActions: {
            styleOverrides: {
                root: {
                    padding: "16px 28px 24px",
                    gap: 8,
                },
            },
        },

        MuiTableContainer: {
            styleOverrides: {
                root: {
                    borderRadius: 16,
                    border: "1px solid #E5E7EB",
                    boxShadow: "0px 1px 3px rgba(0,0,0,0.07), 0px 1px 2px rgba(0,0,0,0.04)",
                    overflow: "hidden",
                },
            },
        },

        MuiTable: {
            styleOverrides: {
                root: {
                    borderCollapse: "separate",
                    borderSpacing: 0,
                },
            },
        },

        MuiTableHead: {
            styleOverrides: {
                root: {
                    backgroundColor: "#F9FAFB",
                    "& .MuiTableCell-head": {
                        backgroundColor: "#F9FAFB",
                        color: "#374151",
                        fontWeight: 600,
                        fontSize: "0.8125rem",
                        letterSpacing: "0.04em",
                        textTransform: "uppercase",
                        borderBottom: "1px solid #E5E7EB",
                        padding: "12px 16px",
                        whiteSpace: "nowrap",
                    },
                },
            },
        },

        MuiTableRow: {
            styleOverrides: {
                root: {
                    transition: "background-color 0.1s ease",
                    "&:hover": {
                        backgroundColor: "#F9FAFB",
                    },
                    "&:last-child td": {
                        borderBottom: 0,
                    },
                },
            },
        },

        MuiTableCell: {
            styleOverrides: {
                root: {
                    borderBottom: "1px solid #F3F4F6",
                    padding: "14px 16px",
                    fontSize: "0.9rem",
                    color: "#374151",
                },
            },
        },

        MuiTablePagination: {
            styleOverrides: {
                root: {
                    borderTop: "1px solid #E5E7EB",
                    backgroundColor: "#F9FAFB",
                    color: "#6B7280",
                    fontSize: "0.875rem",
                },
                selectLabel: {
                    fontSize: "0.875rem",
                    color: "#6B7280",
                },
                displayedRows: {
                    fontSize: "0.875rem",
                    color: "#6B7280",
                },
            },
        },

        MuiDivider: {
            styleOverrides: {
                root: {
                    borderColor: "#E5E7EB",
                },
            },
        },

        MuiDrawer: {
            styleOverrides: {
                paper: {
                    backgroundColor: "#111827",
                    borderRight: "none",
                    boxShadow: "4px 0 24px rgba(0,0,0,0.15)",
                },
            },
        },

        MuiAppBar: {
            defaultProps: {
                elevation: 0,
            },
            styleOverrides: {
                root: {
                    backgroundColor: "#FFFFFF",
                    borderBottom: "1px solid #E5E7EB",
                    color: "#111827",
                },
            },
        },

        MuiListItemButton: {
            styleOverrides: {
                root: {
                    borderRadius: 10,
                    margin: "2px 8px",
                    padding: "8px 12px",
                    transition: "all 0.15s ease",
                    "&.Mui-selected": {
                        backgroundColor: "rgba(37,99,235,0.12)",
                        color: "#2563EB",
                        "&:hover": {
                            backgroundColor: "rgba(37,99,235,0.18)",
                        },
                        "& .MuiListItemIcon-root": {
                            color: "#2563EB",
                        },
                    },
                    "&:hover": {
                        backgroundColor: "rgba(255,255,255,0.06)",
                    },
                },
            },
        },

        MuiListItemIcon: {
            styleOverrides: {
                root: {
                    minWidth: 36,
                    color: "#9CA3AF",
                },
            },
        },

        MuiListItemText: {
            styleOverrides: {
                primary: {
                    fontSize: "0.9rem",
                    fontWeight: 500,
                },
            },
        },

        MuiTooltip: {
            styleOverrides: {
                tooltip: {
                    backgroundColor: "#1F2937",
                    color: "#F9FAFB",
                    fontSize: "0.8125rem",
                    fontWeight: 500,
                    padding: "6px 12px",
                    borderRadius: 8,
                },
                arrow: {
                    color: "#1F2937",
                },
            },
        },

        MuiAlert: {
            styleOverrides: {
                root: {
                    borderRadius: 12,
                    fontSize: "0.9rem",
                    "&.MuiAlert-standardSuccess": {
                        backgroundColor: "#F0FDF4",
                        color: "#15803D",
                        border: "1px solid #BBF7D0",
                    },
                    "&.MuiAlert-standardWarning": {
                        backgroundColor: "#FFFBEB",
                        color: "#B45309",
                        border: "1px solid #FDE68A",
                    },
                    "&.MuiAlert-standardError": {
                        backgroundColor: "#FEF2F2",
                        color: "#B91C1C",
                        border: "1px solid #FECACA",
                    },
                    "&.MuiAlert-standardInfo": {
                        backgroundColor: "#EFF6FF",
                        color: "#1D4ED8",
                        border: "1px solid #BFDBFE",
                    },
                },
            },
        },

        MuiSnackbar: {
            styleOverrides: {
                root: {
                    "& .MuiPaper-root": {
                        borderRadius: 12,
                        boxShadow: "0px 10px 15px -3px rgba(0,0,0,0.1), 0px 4px 6px -2px rgba(0,0,0,0.05)",
                    },
                },
            },
        },

        MuiMenu: {
            styleOverrides: {
                paper: {
                    borderRadius: 12,
                    boxShadow: "0px 10px 15px -3px rgba(0,0,0,0.07), 0px 4px 6px -2px rgba(0,0,0,0.04)",
                    border: "1px solid #E5E7EB",
                    marginTop: 4,
                },
            },
        },

        MuiMenuItem: {
            styleOverrides: {
                root: {
                    borderRadius: 8,
                    margin: "2px 6px",
                    padding: "8px 12px",
                    fontSize: "0.9rem",
                    transition: "background-color 0.1s ease",
                    "&:hover": {
                        backgroundColor: "#F3F4F6",
                    },
                    "&.Mui-selected": {
                        backgroundColor: "#EFF6FF",
                        color: "#2563EB",
                        "&:hover": {
                            backgroundColor: "#DBEAFE",
                        },
                    },
                },
            },
        },

        MuiAutocomplete: {
            styleOverrides: {
                paper: {
                    borderRadius: 12,
                    boxShadow: "0px 10px 15px -3px rgba(0,0,0,0.07), 0px 4px 6px -2px rgba(0,0,0,0.04)",
                    border: "1px solid #E5E7EB",
                },
                option: {
                    borderRadius: 8,
                    margin: "2px 6px",
                    fontSize: "0.9rem",
                },
            },
        },

        MuiSkeleton: {
            styleOverrides: {
                root: {
                    borderRadius: 8,
                    backgroundColor: "#F3F4F6",
                },
            },
        },

        MuiLinearProgress: {
            styleOverrides: {
                root: {
                    borderRadius: 99,
                    height: 6,
                    backgroundColor: "#E5E7EB",
                },
                bar: {
                    borderRadius: 99,
                },
            },
        },

        MuiCircularProgress: {
            styleOverrides: {
                root: {
                    color: "#2563EB",
                },
            },
        },

        MuiSwitch: {
            styleOverrides: {
                root: {
                    padding: 8,
                },
                switchBase: {
                    "&.Mui-checked": {
                        color: "#FFFFFF",
                        "& + .MuiSwitch-track": {
                            backgroundColor: "#2563EB",
                            opacity: 1,
                        },
                    },
                },
                track: {
                    borderRadius: 99,
                    backgroundColor: "#D1D5DB",
                    opacity: 1,
                },
                thumb: {
                    boxShadow: "0px 1px 3px rgba(0,0,0,0.2)",
                },
            },
        },

        MuiCheckbox: {
            styleOverrides: {
                root: {
                    color: "#D1D5DB",
                    "&.Mui-checked": {
                        color: "#2563EB",
                    },
                    "&:hover": {
                        backgroundColor: "rgba(37,99,235,0.06)",
                    },
                },
            },
        },

        MuiRadio: {
            styleOverrides: {
                root: {
                    color: "#D1D5DB",
                    "&.Mui-checked": {
                        color: "#2563EB",
                    },
                    "&:hover": {
                        backgroundColor: "rgba(37,99,235,0.06)",
                    },
                },
            },
        },

        MuiTabs: {
            styleOverrides: {
                root: {
                    borderBottom: "1px solid #E5E7EB",
                },
                indicator: {
                    backgroundColor: "#2563EB",
                    height: 2,
                    borderRadius: "2px 2px 0 0",
                },
            },
        },

        MuiTab: {
            styleOverrides: {
                root: {
                    textTransform: "none",
                    fontWeight: 500,
                    fontSize: "0.9rem",
                    color: "#6B7280",
                    minHeight: 44,
                    padding: "8px 16px",
                    "&.Mui-selected": {
                        color: "#2563EB",
                        fontWeight: 600,
                    },
                },
            },
        },

        MuiBreadcrumbs: {
            styleOverrides: {
                root: {
                    fontSize: "0.875rem",
                    color: "#6B7280",
                },
                separator: {
                    color: "#9CA3AF",
                },
            },
        },

        MuiAvatarGroup: {
            styleOverrides: {
                avatar: {
                    borderColor: "#FFFFFF",
                    fontSize: "0.8125rem",
                    fontWeight: 600,
                },
            },
        },
    },
});

export default theme;