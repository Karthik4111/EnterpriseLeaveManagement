import { useMemo } from "react";
import Grid from "@mui/material/Grid";
import {
    AreaChart,
    Area,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    ResponsiveContainer,
    PieChart,
    Pie,
    Cell,
    Legend,
} from "recharts";

import GroupIcon from "@mui/icons-material/Group";
import BusinessIcon from "@mui/icons-material/Business";
import PendingActionsIcon from "@mui/icons-material/PendingActions";
import BeachAccessIcon from "@mui/icons-material/BeachAccess";

import { Avatar, Box, Card, CardContent, Typography } from "@mui/material";

import DashboardHeader from "@/components/dashboard/DashboardHeader";
import StatisticCard from "@/components/dashboard/StatisticCard";
import QuickActions from "@/components/dashboard/QuickActions";
import RecentLeaves from "@/components/dashboard/RecentLeaves";
import UpcomingHolidays from "@/components/dashboard/UpcomingHolidays";

import { useDashboardSummary } from "@/hooks/useDashboard";
import {
    useLeaveRequests,
    useLeaveTypes,
    useNotifications,
} from "@/hooks/useLeave";
import { useEmployees } from "@/hooks/useEmployees";

const PIE_COLORS = ["#F59E0B", "#22C55E", "#EF4444"];
const AVATAR_COLORS = ["#2563EB", "#6366F1", "#22C55E", "#F59E0B", "#EF4444"];

function getInitials(name: string): string {
    return name
        .split(" ")
        .map((n) => n[0])
        .slice(0, 2)
        .join("")
        .toUpperCase();
}

export default function DashboardPage() {
    const { data } = useDashboardSummary();
    const { data: leaveRequests = [] } = useLeaveRequests();
    const { data: leaveTypes = [] } = useLeaveTypes();
    const { data: notifications = [] } = useNotifications();
    const { data: employees = [] } = useEmployees();

    const leaveTypeMap = useMemo(
        () => new Map(leaveTypes.map((t) => [t.id, t.name])),
        [leaveTypes]
    );

    // Monthly leave trend — last 6 months
    const monthlyTrend = useMemo(() => {
        const map: Record<
            string,
            { month: string; Pending: number; Approved: number; Rejected: number }
        > = {};

        leaveRequests.forEach((req) => {
            const d = new Date(req.startDate);
            const key = `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, "0")}`;
            const label = d.toLocaleDateString("en-US", {
                month: "short",
                year: "2-digit",
            });

            if (!map[key])
                map[key] = { month: label, Pending: 0, Approved: 0, Rejected: 0 };

            if (req.status === 1) map[key].Pending++;
            else if (req.status === 2) map[key].Approved++;
            else if (req.status === 3) map[key].Rejected++;
        });

        return Object.entries(map)
            .sort(([a], [b]) => a.localeCompare(b))
            .slice(-6)
            .map(([, v]) => v);
    }, [leaveRequests]);

    // Status distribution for pie chart
    const statusData = useMemo(
        () => [
            { name: "Pending", value: data?.pendingRequests ?? 0 },
            { name: "Approved", value: data?.approvedRequests ?? 0 },
            { name: "Rejected", value: data?.rejectedRequests ?? 0 },
        ],
        [data]
    );

    const recentEmployees = employees.slice(0, 5);

    return (
        <Grid container spacing={3}>
            {/* ── Section 1: Welcome Banner ── */}
            <Grid size={12}>
                <DashboardHeader />
            </Grid>

            {/* ── Section 2: KPI Cards ── */}
            <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                <StatisticCard
                    title="Total Employees"
                    value={data?.totalEmployees ?? 0}
                    icon={<GroupIcon />}
                    color="#2563EB"
                />
            </Grid>
            <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                <StatisticCard
                    title="Departments"
                    value={data?.totalDepartments ?? 0}
                    icon={<BusinessIcon />}
                    color="#6366F1"
                />
            </Grid>
            <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                <StatisticCard
                    title="Pending Requests"
                    value={data?.pendingRequests ?? 0}
                    icon={<PendingActionsIcon />}
                    color="#F59E0B"
                />
            </Grid>
            <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                <StatisticCard
                    title="On Leave Today"
                    value={data?.employeesOnLeaveToday ?? 0}
                    icon={<BeachAccessIcon />}
                    color="#22C55E"
                />
            </Grid>

            {/* ── Section 3: Leave Trend Chart ── */}
            <Grid size={{ xs: 12, md: 8 }}>
                <Card elevation={0} sx={{ height: "100%" }}>
                    <CardContent sx={{ p: 3, "&:last-child": { pb: 3 } }}>
                        <Typography
                            variant="h6"
                            sx={{ fontWeight: 600, color: "#111827" }}
                        >
                            Leave Request Trends
                        </Typography>
                        <Typography
                            sx={{
                                fontSize: "0.8125rem",
                                color: "#6B7280",
                                mt: 0.5,
                                mb: 3,
                            }}
                        >
                            Monthly breakdown by status
                        </Typography>

                        {monthlyTrend.length > 0 ? (
                            <ResponsiveContainer width="100%" height={220}>
                                <AreaChart
                                    data={monthlyTrend}
                                    margin={{
                                        top: 5,
                                        right: 10,
                                        left: -10,
                                        bottom: 0,
                                    }}
                                >
                                    <defs>
                                        <linearGradient
                                            id="gPending"
                                            x1="0"
                                            y1="0"
                                            x2="0"
                                            y2="1"
                                        >
                                            <stop
                                                offset="5%"
                                                stopColor="#F59E0B"
                                                stopOpacity={0.18}
                                            />
                                            <stop
                                                offset="95%"
                                                stopColor="#F59E0B"
                                                stopOpacity={0}
                                            />
                                        </linearGradient>
                                        <linearGradient
                                            id="gApproved"
                                            x1="0"
                                            y1="0"
                                            x2="0"
                                            y2="1"
                                        >
                                            <stop
                                                offset="5%"
                                                stopColor="#22C55E"
                                                stopOpacity={0.18}
                                            />
                                            <stop
                                                offset="95%"
                                                stopColor="#22C55E"
                                                stopOpacity={0}
                                            />
                                        </linearGradient>
                                        <linearGradient
                                            id="gRejected"
                                            x1="0"
                                            y1="0"
                                            x2="0"
                                            y2="1"
                                        >
                                            <stop
                                                offset="5%"
                                                stopColor="#EF4444"
                                                stopOpacity={0.18}
                                            />
                                            <stop
                                                offset="95%"
                                                stopColor="#EF4444"
                                                stopOpacity={0}
                                            />
                                        </linearGradient>
                                    </defs>
                                    <CartesianGrid
                                        strokeDasharray="3 3"
                                        stroke="#F3F4F6"
                                        vertical={false}
                                    />
                                    <XAxis
                                        dataKey="month"
                                        tick={{ fontSize: 12, fill: "#9CA3AF" }}
                                        axisLine={false}
                                        tickLine={false}
                                    />
                                    <YAxis
                                        tick={{ fontSize: 12, fill: "#9CA3AF" }}
                                        axisLine={false}
                                        tickLine={false}
                                        allowDecimals={false}
                                    />
                                    <Tooltip
                                        contentStyle={{
                                            borderRadius: 10,
                                            border: "1px solid #E5E7EB",
                                            boxShadow:
                                                "0 4px 6px rgba(0,0,0,0.05)",
                                            fontSize: 12,
                                        }}
                                    />
                                    <Area
                                        type="monotone"
                                        dataKey="Pending"
                                        stroke="#F59E0B"
                                        strokeWidth={2}
                                        fill="url(#gPending)"
                                        dot={false}
                                    />
                                    <Area
                                        type="monotone"
                                        dataKey="Approved"
                                        stroke="#22C55E"
                                        strokeWidth={2}
                                        fill="url(#gApproved)"
                                        dot={false}
                                    />
                                    <Area
                                        type="monotone"
                                        dataKey="Rejected"
                                        stroke="#EF4444"
                                        strokeWidth={2}
                                        fill="url(#gRejected)"
                                        dot={false}
                                    />
                                </AreaChart>
                            </ResponsiveContainer>
                        ) : (
                            <Box
                                sx={{
                                    height: 220,
                                    display: "flex",
                                    alignItems: "center",
                                    justifyContent: "center",
                                }}
                            >
                                <Typography
                                    sx={{
                                        color: "#9CA3AF",
                                        fontSize: "0.9rem",
                                    }}
                                >
                                    No trend data available yet
                                </Typography>
                            </Box>
                        )}
                    </CardContent>
                </Card>
            </Grid>

            {/* ── Section 3b: Status Pie Chart ── */}
            <Grid size={{ xs: 12, md: 4 }}>
                <Card elevation={0} sx={{ height: "100%" }}>
                    <CardContent sx={{ p: 3, "&:last-child": { pb: 3 } }}>
                        <Typography
                            variant="h6"
                            sx={{ fontWeight: 600, color: "#111827" }}
                        >
                            Leave Status
                        </Typography>
                        <Typography
                            sx={{
                                fontSize: "0.8125rem",
                                color: "#6B7280",
                                mt: 0.5,
                                mb: 1,
                            }}
                        >
                            Overall distribution
                        </Typography>
                        <ResponsiveContainer width="100%" height={230}>
                            <PieChart>
                                <Pie
                                    data={statusData}
                                    cx="50%"
                                    cy="45%"
                                    innerRadius={58}
                                    outerRadius={88}
                                    paddingAngle={3}
                                    dataKey="value"
                                >
                                    {statusData.map((_, i) => (
                                        <Cell
                                            key={`cell-${i}`}
                                            fill={
                                                PIE_COLORS[
                                                    i % PIE_COLORS.length
                                                ]
                                            }
                                        />
                                    ))}
                                </Pie>
                                <Tooltip
                                    contentStyle={{
                                        borderRadius: 10,
                                        border: "1px solid #E5E7EB",
                                        fontSize: 12,
                                    }}
                                />
                                <Legend
                                    iconSize={8}
                                    iconType="circle"
                                    wrapperStyle={{ fontSize: 12 }}
                                />
                            </PieChart>
                        </ResponsiveContainer>
                    </CardContent>
                </Card>
            </Grid>

            {/* ── Section 4: Recent Leaves + Quick Actions ── */}
            <Grid size={{ xs: 12, md: 7 }}>
                <RecentLeaves
                    leaves={leaveRequests}
                    employees={employees}
                    leaveTypeMap={leaveTypeMap}
                />
            </Grid>
            <Grid size={{ xs: 12, md: 5 }}>
                <QuickActions />
            </Grid>

            {/* ── Section 5: Notifications + Recent Employees ── */}
            <Grid size={{ xs: 12, md: 4 }}>
                <UpcomingHolidays notifications={notifications} />
            </Grid>
            <Grid size={{ xs: 12, md: 8 }}>
                <Card elevation={0}>
                    <CardContent sx={{ p: 3, "&:last-child": { pb: 3 } }}>
                        <Typography
                            variant="h6"
                            sx={{
                                fontWeight: 600,
                                color: "#111827",
                                mb: 2.5,
                            }}
                        >
                            Recent Employees
                        </Typography>

                        {recentEmployees.length === 0 ? (
                            <Box sx={{ textAlign: "center", py: 4 }}>
                                <Typography
                                    sx={{
                                        color: "#9CA3AF",
                                        fontSize: "0.9rem",
                                    }}
                                >
                                    No employees found
                                </Typography>
                            </Box>
                        ) : (
                            <Box
                                sx={{
                                    display: "flex",
                                    flexDirection: "column",
                                }}
                            >
                                {recentEmployees.map((emp, index) => (
                                    <Box
                                        key={emp.id}
                                        sx={{
                                            display: "flex",
                                            alignItems: "center",
                                            gap: 2,
                                            py: 1.5,
                                            px: 1,
                                            borderRadius: "10px",
                                            transition:
                                                "background-color 0.15s ease",
                                            "&:hover": { bgcolor: "#F9FAFB" },
                                            borderBottom:
                                                index <
                                                recentEmployees.length - 1
                                                    ? "1px solid #F3F4F6"
                                                    : "none",
                                        }}
                                    >
                                        <Avatar
                                            sx={{
                                                width: 36,
                                                height: 36,
                                                bgcolor:
                                                    AVATAR_COLORS[
                                                        index %
                                                            AVATAR_COLORS.length
                                                    ],
                                                fontSize: "0.75rem",
                                                fontWeight: 700,
                                                flexShrink: 0,
                                            }}
                                        >
                                            {getInitials(emp.fullName)}
                                        </Avatar>

                                        <Box sx={{ flex: 1, minWidth: 0 }}>
                                            <Typography
                                                sx={{
                                                    fontSize: "0.875rem",
                                                    fontWeight: 600,
                                                    color: "#111827",
                                                    lineHeight: 1.3,
                                                }}
                                            >
                                                {emp.fullName}
                                            </Typography>
                                            <Typography
                                                sx={{
                                                    fontSize: "0.75rem",
                                                    color: "#6B7280",
                                                    lineHeight: 1.3,
                                                    overflow: "hidden",
                                                    textOverflow: "ellipsis",
                                                    whiteSpace: "nowrap",
                                                }}
                                            >
                                                {emp.designation}&nbsp;&middot;&nbsp;
                                                {emp.department}
                                            </Typography>
                                        </Box>

                                        <Typography
                                            sx={{
                                                fontSize: "0.75rem",
                                                color: "#9CA3AF",
                                                flexShrink: 0,
                                                fontWeight: 500,
                                            }}
                                        >
                                            {emp.employeeCode}
                                        </Typography>
                                    </Box>
                                ))}
                            </Box>
                        )}
                    </CardContent>
                </Card>
            </Grid>
        </Grid>
    );
}