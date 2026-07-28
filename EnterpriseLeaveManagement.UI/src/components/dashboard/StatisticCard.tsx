import type { ReactNode } from "react";

import {
    Box,
    Card,
    CardContent,
    Typography,
} from "@mui/material";

interface StatisticCardProps {
    title: string;
    value: number | string;
    icon: ReactNode;
    color: string;
}

export default function StatisticCard({
    title,
    value,
    icon,
    color,
}: StatisticCardProps) {
    return (
        <Card
            elevation={2}
            sx={{
                height: "100%",
                transition: "0.3s",
                cursor: "pointer",
                "&:hover": {
                    transform: "translateY(-4px)",
                    boxShadow: 6,
                },
            }}
        >
            <CardContent>
                <Box
                    sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                    }}
                >
                    <Box>
                        <Typography
                            variant="body2"
                            sx={{
                                color: "text.secondary",
                            }}
                        >
                            {title}
                        </Typography>

                        <Typography
                            variant="h4"
                            sx={{
                                mt: 2,
                                fontWeight: 700,
                            }}
                        >
                            {value}
                        </Typography>
                    </Box>

                    <Box
                        sx={{
                            width: 56,
                            height: 56,
                            borderRadius: "50%",
                            backgroundColor: color,
                            display: "flex",
                            justifyContent: "center",
                            alignItems: "center",
                            color: "#fff",
                        }}
                    >
                        {icon}
                    </Box>
                </Box>
            </CardContent>
        </Card>
    );
}