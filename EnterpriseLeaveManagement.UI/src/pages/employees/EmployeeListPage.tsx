import { useMemo, useState } from "react";

import PageHeader from "@/components/common/PageHeader/PageHeader";
import SearchBar from "@/components/common/SearchBar/SearchBar";

import DataTable, {
    type DataColumn,
} from "@/components/common/DataTable/DataTable";

import TableActions from "@/components/common/TableActions/TableActions";

import type { Employee } from "@/types/employee";
import { useEmployees } from "@/hooks/useEmployees";

export default function EmployeeListPage() {
    const { data: employeeList = [], isLoading } =
        useEmployees();

    const [search, setSearch] = useState("");

    const filteredEmployees = useMemo(() => {
        const keyword = search.trim().toLowerCase();

        if (!keyword) {
            return employeeList;
        }

        return employeeList.filter(
            (employee) =>
                employee.employeeCode
                    .toLowerCase()
                    .includes(keyword) ||
                employee.fullName
                    .toLowerCase()
                    .includes(keyword) ||
                employee.department
                    .toLowerCase()
                    .includes(keyword) ||
                employee.designation
                    .toLowerCase()
                    .includes(keyword) ||
                employee.email
                    .toLowerCase()
                    .includes(keyword)
        );
    }, [employeeList, search]);

    const handleView = (employee: Employee) => {
        console.log("View Employee:", employee);
    };

    const columns: DataColumn<Employee>[] = [
        {
            field: "employeeCode",
            headerName: "Employee ID",
            sortable: true,
        },
        {
            field: "fullName",
            headerName: "Name",
            sortable: true,
        },
        {
            field: "department",
            headerName: "Department",
            sortable: true,
        },
        {
            field: "designation",
            headerName: "Designation",
            sortable: true,
        },
        {
            field: "email",
            headerName: "Email",
            sortable: true,
        },
        {
            field: "id",
            headerName: "Actions",
            sortable: false,
            align: "center",
            render: (employee) => (
                <TableActions
                    onView={() =>
                        handleView(employee)
                    }
                />
            ),
        },
    ];

    return (
        <>
            <PageHeader
                title="Employees"
                subtitle="Employee records loaded from backend API."
            />

            <SearchBar
                value={search}
                onChange={setSearch}
                placeholder="Search employees..."
            />

            <DataTable
                columns={columns}
                rows={filteredEmployees}
                loading={isLoading}
            />
        </>
    );
}