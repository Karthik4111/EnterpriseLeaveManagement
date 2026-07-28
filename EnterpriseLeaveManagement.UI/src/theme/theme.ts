import { createTheme } from "@mui/material/styles";
import palette from "./palette";

const theme = createTheme({
    palette,

    shape: {
        borderRadius: 10,
    },

    typography: {
        fontFamily:
            "'Inter', 'Roboto', 'Helvetica', 'Arial', sans-serif",

        h4: {
            fontWeight: 700,
        },

        h5: {
            fontWeight: 600,
        },

        h6: {
            fontWeight: 600,
        },

        button: {
            textTransform: "none",
            fontWeight: 600,
        },
    },

    components: {
        MuiPaper: {
            styleOverrides: {
                root: {
                    borderRadius: 12,
                },
            },
        },

        MuiButton: {
            defaultProps: {
                variant: "contained",
            },
        },

        MuiTextField: {
            defaultProps: {
                fullWidth: true,
                size: "small",
            },
        },
    },
});

export default theme;