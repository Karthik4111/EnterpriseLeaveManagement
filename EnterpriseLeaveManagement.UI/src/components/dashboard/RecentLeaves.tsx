import { Avatar, Box, Button, Card, CardContent, Chip, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

import { LEAVE_STATUS_LABELS } from "@/types/leave";
import type { LeaveRequest } from "@/types/leave";
import type { Employee } from "@/types/employee";
import { ROUTES } from "@/constants/routes";

interface RecentLeavesProps {
    leaves: LeaveRequest[];
    employees: Employee[];
    leaveTypeMap: Map<string, string>;
}

function getStatusColor(status: number) {
    switch (status) {
        case 2:
            return "success" as const;
        case 3:
            return "error" as const;
        case 4:
            return "default" as const;
        default:
            return "warning" as const;
    }
}

function formatDate(dateStr: string): string {
    return new Date(dateStr).toLocaleDateString("en-US", {
        month: "short",
        day: "numeric",
        year: "numeric",
    });
}

function getInitials(name: string): string {
    return name
        .split(" ")
        .map((n) => n[0])
        .slice(0, 2)
        .join("")
        .toUpperCase();
}

const AVATAR_COLORS = [
    "#2563EB",
    "#6366F1",
    "#22C55E",
    "#F59E0B",
    "#EF4444",
    "#8B5CF6",
];

export default function RecentLeaves({
    leaves,
    employees,
    leaveTypeMap,
}: RecentLeavesProps) {
    const navigate = useNavigate();

    const employeeMap = new Map(employees.map((e) => [e.id, e]));

    const recentLeaves = [...leaves]
        .sort(
            (a, b) =>
                new Date(b.startDate).getTime() -
                new Date(a.startDate).getTime()
        )
        .slice(0, 6);

    return (
        <Card elevation={0} sx={{ height: "100%" }}>
            <CardContent sx={{ p: 3, "&:last-child": { pb: 3 } }}>
                <Box
                    sx={{
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "space-between",
                        mb: 2.5,
                    }}
                >
                    <Typography
                        variant="h6"
                        sx={{ fontWeight: 600, color: "#111827" }}
                    >
                        Recent Leave Requests
                    </Typography>
                    <Button
                        size="small"
                        variant="text"
                        onClick={() => navigate(ROUTES.LEAVE_APPROVALS)}
                        sx={{ fontSize: "0.8125rem", fontWeight: 500 }}
                    >
                        View all
                    </Button>
                </Box>

                {recentLeaves.length === 0 ? (
                    <Box sx={{ textAlign: "center", py: 5 }}>
                        <Typography
                            sx={{ color: "#9CA3AF", fontSize: "0.9rem" }}
                        >
                            No leave requests yet
                        </Typography>
                    </Box>
                ) : (
                    <Box
                        sx={{
                            display: "flex",
                            flexDirection: "column",
                            gap: 0.75,
                        }}
                    >
                        {recentLeaves.map((leave, index) => {
                            const employee = employeeMap.get(leave.employeeId);
                            const leaveTypeName =
                                leaveTypeMap.get(leave.leaveTypeId) ?? "Leave";
                            const initials = employee
                                ? getInitials(employee.fullName)
                                : "?";
                            const avatarColor =
                                AVATAR_COLORS[index % AVATAR_COLORS.length];

                            return (
                                <Box
                                    key={leave.id}
                                    sx={{
                                        display: "flex",
                                        alignItems: "center",
                                        gap: 2,
                                        p: 1.75,
                                        borderRadius: "12px",
                                        bgcolor: "#FAFAFA",
                                        border: "1px solid #F3F4F6",
                                        transition: "all 0.15s ease",
                                        "&:hover": {
                                            bgcolor: "#F3F4F6",
                                            borderColor: "#E5E7EB",
                                            transform: "translateX(2px)",
                                        },
                                    }}
                                >
                                    <Avatar
                                        sx={{
                                            width: 36,
                                            height: 36,
                                            bgcolor: avatarColor,
                                            fontSize: "0.75rem",
                                            fontWeight: 700,
                                            flexShrink: 0,
                                        }}
                                    >
                                        {initials}
                                    </Avatar>

                                    <Box sx={{ flex: 1, minWidth: 0 }}>
                                        <Typography
                                            sx={{
                                                fontSize: "0.875rem",
                                                fontWeight: 600,
                                                color: "#111827",
                                                lineHeight: 1.3,
                                                overflow: "hidden",
                                                textOverflow: "ellipsis",
                                                whiteSpace: "nowrap",
                                            }}
                                        >
                                            {employee?.fullName ??
                                                "Unknown Employee"}
                                        </Typography>
                                        <Typography
                                            sx={{
                                                fontSize: "0.75rem",
                                                color: "#6B7280",
                                                lineHeight: 1.3,
                                            }}
                                        >
                                            {leaveTypeName} &middot;{" "}
                                            {leave.numberOfDays} day
                                            {leave.numberOfDays !== 1
                                                ? "s"
                                                : ""}
                                        </Typography>
                                    </Box>

                                    <Box
                                        sx={{
                                            display: "flex",
                                            flexDirection: "column",
                                            alignItems: "flex-end",
                                            gap: 0.5,
                                            flexShrink: 0,
                                        }}
                                    >
                                        <Chip
                                            label={
                                                LEAVE_STATUS_LABELS[
                                                    leave.status
                                                ] ?? "Unknown"
                                            }
                                            color={getStatusColor(leave.status)}
                                            size="small"
                                        />
                                        <Typography
                                            sx={{
                                                fontSize: "0.6875rem",
                                                color: "#9CA3AF",
                                            }}
                                        >
                                            {formatDate(leave.startDate)}
                                        </Typography>
                                    </Box>
                                </Box>
                            );
                        })}
                    </Box>
                )}
            </CardContent>
        </Card>
    );
}
