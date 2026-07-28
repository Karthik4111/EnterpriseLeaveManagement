import type { ReactNode } from "react";

import {
    Box,
    Button,
    Typography,
} from "@mui/material";

interface PageHeaderProps {
    title: string;
    subtitle?: string;
    actionText?: string;
    actionIcon?: ReactNode;
    onActionClick?: () => void;
}

export default function PageHeader({
    title,
    subtitle,
    actionText,
    actionIcon,
    onActionClick,
}: PageHeaderProps) {
    return (
        <Box
            sx={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
                mb: 3,
                flexWrap: "wrap",
                gap: 2,
            }}
        >
            <Box>
                <Typography variant="h4">
                    {title}
                </Typography>

                {subtitle && (
                    <Typography
                        variant="body1"
                        color="text.secondary"
                    >
                        {subtitle}
                    </Typography>
                )}
            </Box>

            {actionText && (
                <Button
                    variant="contained"
                    startIcon={actionIcon}
                    onClick={onActionClick}
                >
                    {actionText}
                </Button>
            )}
        </Box>
    );
}