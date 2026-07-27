import { Box } from "@mui/material";
import type { ReactNode } from "react";

interface Props {
    children: ReactNode;
}

function MainContent({ children }: Props) {
    return (
        <Box
            component="main"
            sx={{
                flexGrow: 1,
                p: 4,
                mt: 8,
            }}
        >
            {children}
        </Box>
    );
}

export default MainContent;