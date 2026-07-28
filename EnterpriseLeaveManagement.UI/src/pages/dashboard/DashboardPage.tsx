import Grid from "@mui/material/Grid";

import GroupIcon from "@mui/icons-material/Group";
import BusinessIcon from "@mui/icons-material/Business";
import PendingActionsIcon from "@mui/icons-material/PendingActions";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import CancelIcon from "@mui/icons-material/Cancel";
import BeachAccessIcon from "@mui/icons-material/BeachAccess";

import DashboardHeader from "@/components/dashboard/DashboardHeader";
import StatisticCard from "@/components/dashboard/StatisticCard";
import { useDashboardSummary } from "@/hooks/useDashboard";

export default function DashboardPage() {
    const { data } = useDashboardSummary();

    return (
        <>
            <DashboardHeader />

            <Grid
                container
                spacing={3}
            >
                <Grid
                    size={{
                        xs: 12,
                        sm: 6,
                        md: 4,
                        lg: 2,
                    }}
                >
                    <StatisticCard
                        title="Employees"
                        value={data?.totalEmployees ?? 0}
                        icon={<GroupIcon />}
                        color="#1976d2"
                    />
                </Grid>

                <Grid
                    size={{
                        xs: 12,
                        sm: 6,
                        md: 4,
                        lg: 2,
                    }}
                >
                    <StatisticCard
                        title="Departments"
                        value={data?.totalDepartments ?? 0}
                        icon={<BusinessIcon />}
                        color="#2e7d32"
                    />
                </Grid>

                <Grid
                    size={{
                        xs: 12,
                        sm: 6,
                        md: 4,
                        lg: 2,
                    }}
                >
                    <StatisticCard
                        title="Pending"
                        value={data?.pendingRequests ?? 0}
                        icon={<PendingActionsIcon />}
                        color="#ed6c02"
                    />
                </Grid>

                <Grid
                    size={{
                        xs: 12,
                        sm: 6,
                        md: 4,
                        lg: 2,
                    }}
                >
                    <StatisticCard
                        title="Approved"
                        value={data?.approvedRequests ?? 0}
                        icon={<CheckCircleIcon />}
                        color="#2e7d32"
                    />
                </Grid>

                <Grid
                    size={{
                        xs: 12,
                        sm: 6,
                        md: 4,
                        lg: 2,
                    }}
                >
                    <StatisticCard
                        title="Rejected"
                        value={data?.rejectedRequests ?? 0}
                        icon={<CancelIcon />}
                        color="#d32f2f"
                    />
                </Grid>

                <Grid
                    size={{
                        xs: 12,
                        sm: 6,
                        md: 4,
                        lg: 2,
                    }}
                >
                    <StatisticCard
                        title="On Leave Today"
                        value={
                            data?.employeesOnLeaveToday ?? 0
                        }
                        icon={<BeachAccessIcon />}
                        color="#6a1b9a"
                    />
                </Grid>
            </Grid>
        </>
    );
}