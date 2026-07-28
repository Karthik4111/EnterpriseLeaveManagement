import type { ReactNode } from "react";
import { alpha } from "@mui/material/styles";
import { Box, Card, CardContent, Typography } from "@mui/material";

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
            elevation={0}
            sx={{
                height: "100%",
                cursor: "default",
                transition: "all 0.2s ease",
                "&:hover": {
                    transform: "translateY(-3px)",
                    boxShadow:
                        "0px 12px 20px -5px rgba(0,0,0,0.08), 0px 4px 8px -2px rgba(0,0,0,0.04)",
                },
            }}
        >
            <CardContent sx={{ p: 3, "&:last-child": { pb: 3 } }}>
                <Box
                    sx={{
                        display: "flex",
                        alignItems: "flex-start",
                        justifyContent: "space-between",
                        gap: 2,
                    }}
                >
                    <Box sx={{ flex: 1, minWidth: 0 }}>
                        <Typography
                            sx={{
                                fontSize: "0.6875rem",
                                fontWeight: 600,
                                color: "#6B7280",
                                textTransform: "uppercase",
                                letterSpacing: "0.07em",
                                mb: 1.5,
                                whiteSpace: "nowrap",
                            }}
                        >
                            {title}
                        </Typography>

                        <Typography
                            sx={{
                                fontSize: "1.875rem",
                                fontWeight: 700,
                                color: "#111827",
                                lineHeight: 1,
                                letterSpacing: "-0.02em",
                            }}
                        >
                            {value}
                        </Typography>
                    </Box>

                    <Box
                        sx={{
                            width: 48,
                            height: 48,
                            borderRadius: "14px",
                            bgcolor: alpha(color, 0.12),
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "center",
                            color: color,
                            flexShrink: 0,
                            "& .MuiSvgIcon-root": { fontSize: 22 },
                        }}
                    >
                        {icon}
                    </Box>
                </Box>
            </CardContent>
        </Card>
    );
}