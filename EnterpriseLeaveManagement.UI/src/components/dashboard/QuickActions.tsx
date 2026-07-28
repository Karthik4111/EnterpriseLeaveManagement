import type { ComponentType } from "react";
import type { SvgIconProps } from "@mui/material";
import { Box, Card, CardContent, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

import PersonAddIcon from "@mui/icons-material/PersonAdd";
import EventNoteIcon from "@mui/icons-material/EventNote";
import ApprovalIcon from "@mui/icons-material/Approval";
import BusinessIcon from "@mui/icons-material/Business";
import BeachAccessIcon from "@mui/icons-material/BeachAccess";
import AccountBalanceIcon from "@mui/icons-material/AccountBalance";

import { ROUTES } from "@/constants/routes";

interface Action {
    label: string;
    Icon: ComponentType<SvgIconProps>;
    path: string;
    color: string;
    bg: string;
}

const ACTIONS: Action[] = [
    {
        label: "Add Employee",
        Icon: PersonAddIcon,
        path: ROUTES.CREATE_EMPLOYEE,
        color: "#2563EB",
        bg: "#EFF6FF",
    },
    {
        label: "Apply Leave",
        Icon: EventNoteIcon,
        path: ROUTES.LEAVE_REQUEST,
        color: "#6366F1",
        bg: "#EEF2FF",
    },
    {
        label: "Approvals",
        Icon: ApprovalIcon,
        path: ROUTES.LEAVE_APPROVALS,
        color: "#22C55E",
        bg: "#F0FDF4",
    },
    {
        label: "Departments",
        Icon: BusinessIcon,
        path: ROUTES.DEPARTMENTS,
        color: "#F59E0B",
        bg: "#FFFBEB",
    },
    {
        label: "My Leaves",
        Icon: BeachAccessIcon,
        path: ROUTES.MY_LEAVES,
        color: "#EF4444",
        bg: "#FEF2F2",
    },
    {
        label: "Leave Balance",
        Icon: AccountBalanceIcon,
        path: ROUTES.LEAVE_BALANCE,
        color: "#8B5CF6",
        bg: "#F5F3FF",
    },
];

export default function QuickActions() {
    const navigate = useNavigate();

    return (
        <Card elevation={0} sx={{ height: "100%" }}>
            <CardContent sx={{ p: 3, "&:last-child": { pb: 3 } }}>
                <Typography
                    variant="h6"
                    sx={{ fontWeight: 600, color: "#111827", mb: 2.5 }}
                >
                    Quick Actions
                </Typography>

                <Box
                    sx={{
                        display: "grid",
                        gridTemplateColumns: "1fr 1fr",
                        gap: 1.5,
                    }}
                >
                    {ACTIONS.map(({ label, Icon, path, color, bg }) => (
                        <Box
                            key={label}
                            onClick={() => navigate(path)}
                            sx={{
                                display: "flex",
                                flexDirection: "column",
                                alignItems: "center",
                                justifyContent: "center",
                                gap: 1,
                                p: 2,
                                borderRadius: "12px",
                                bgcolor: bg,
                                cursor: "pointer",
                                border: "1px solid transparent",
                                transition: "all 0.15s ease",
                                userSelect: "none",
                                "&:hover": {
                                    transform: "translateY(-2px)",
                                    boxShadow: "0 4px 12px rgba(0,0,0,0.08)",
                                    borderColor: color + "30",
                                },
                                "&:active": { transform: "translateY(0)" },
                            }}
                        >
                            <Icon sx={{ color, fontSize: 22 }} />
                            <Typography
                                sx={{
                                    fontSize: "0.75rem",
                                    fontWeight: 500,
                                    color: "#374151",
                                    textAlign: "center",
                                    lineHeight: 1.3,
                                }}
                            >
                                {label}
                            </Typography>
                        </Box>
                    ))}
                </Box>
            </CardContent>
        </Card>
    );
}
