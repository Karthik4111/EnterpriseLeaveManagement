import { useMemo } from "react";

import { Typography } from "@mui/material";

import PageHeader from "@/components/common/PageHeader/PageHeader";
import DataTable, {
	type DataColumn,
} from "@/components/common/DataTable/DataTable";

import useAuth from "@/hooks/useAuth";
import { useLeaveAllocations } from "@/hooks/useLeave";
import { ROLES } from "@/constants/roles";
import type { LeaveAllocation } from "@/types/leave";

export default function LeaveBalancePage() {
	const { auth } = useAuth();
	const { data: allocations = [], isLoading } =
		useLeaveAllocations();

	const visibleAllocations = useMemo(() => {
		if (auth.user?.role === ROLES.ADMIN) {
			return allocations;
		}

		if (!auth.user?.id) {
			return [];
		}

		return allocations.filter(
			(allocation) =>
				allocation.employeeId === auth.user?.id
		);
	}, [allocations, auth.user?.id, auth.user?.role]);

	const columns: DataColumn<LeaveAllocation>[] = [
		{
			field: "employeeName",
			headerName: "Employee",
		},
		{
			field: "leaveTypeName",
			headerName: "Leave Type",
		},
		{
			field: "year",
			headerName: "Year",
		},
		{
			field: "allocatedDays",
			headerName: "Allocated Days",
		},
	];

	return (
		<>
			<PageHeader
				title="Leave Balance"
				subtitle="View leave allocations by employee and leave type."
			/>

			{auth.user?.role !== ROLES.ADMIN && (
				<Typography sx={{ mb: 2 }} color="text.secondary">
					Showing your personal leave allocation entries.
				</Typography>
			)}

			<DataTable
				columns={columns}
				rows={visibleAllocations}
				loading={isLoading}
			/>
		</>
	);
}
