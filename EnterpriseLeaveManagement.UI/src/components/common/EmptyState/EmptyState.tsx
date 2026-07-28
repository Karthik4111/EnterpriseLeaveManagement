import InboxIcon from "@mui/icons-material/Inbox";

import {
    Box,
    Typography,
} from "@mui/material";

interface EmptyStateProps {
    message?: string;
}

export default function EmptyState({
    message = "No records found.",
}: EmptyStateProps) {
    return (
        <Box
            sx={{
                py: 8,
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                color: "text.secondary",
            }}
        >
            <InboxIcon
                sx={{
                    fontSize: 70,
                    mb: 2,
                }}
            />

            <Typography variant="h6">
                {message}
            </Typography>
        </Box>
    );
}