import { useMemo, useState } from "react";
import type { ReactNode } from "react";

import {
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TablePagination,
    TableRow,
    TableSortLabel,
} from "@mui/material";

import LoadingState from "@/components/common/LoadingState/LoadingState";
import EmptyState from "@/components/common/EmptyState/EmptyState";

export interface DataColumn<T> {
    field: keyof T;
    headerName: string;
    align?: "left" | "center" | "right";
    render?: (row: T) => ReactNode;
    sortable?: boolean;
}

interface DataTableProps<T> {
    columns: DataColumn<T>[];
    rows: T[];
    loading?: boolean;
}

type Order = "asc" | "desc";

export default function DataTable<T extends { id: number | string }>({
    columns,
    rows,
    loading = false,
}: DataTableProps<T>) {
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(5);

    const [order, setOrder] = useState<Order>("asc");
    const [orderBy, setOrderBy] = useState<keyof T | null>(null);

    const handleSort = (field: keyof T) => {
        if (orderBy === field) {
            setOrder((previous) =>
                previous === "asc" ? "desc" : "asc"
            );
        } else {
            setOrderBy(field);
            setOrder("asc");
        }
    };

    const sortedRows = useMemo(() => {
        if (!orderBy) {
            return rows;
        }

        return [...rows].sort((a, b) => {
            const first = String(a[orderBy]).toLowerCase();
            const second = String(b[orderBy]).toLowerCase();

            if (first < second) {
                return order === "asc" ? -1 : 1;
            }

            if (first > second) {
                return order === "asc" ? 1 : -1;
            }

            return 0;
        });
    }, [rows, orderBy, order]);

    const pagedRows = useMemo(() => {
        const start = page * rowsPerPage;
        return sortedRows.slice(
            start,
            start + rowsPerPage
        );
    }, [sortedRows, page, rowsPerPage]);

    if (loading) {
        return <LoadingState rows={rowsPerPage} />;
    }

    if (!loading && rows.length === 0) {
        return (
            <EmptyState message="No records found." />
        );
    }

    return (
        <Paper elevation={2}>
            <TableContainer>
                <Table>
                    <TableHead>
                        <TableRow>
                            {columns.map((column) => (
                                <TableCell
                                    key={String(column.field)}
                                    align={column.align ?? "left"}
                                    sx={{
                                        fontWeight: 700,
                                    }}
                                >
                                    {column.sortable === false ? (
                                        column.headerName
                                    ) : (
                                        <TableSortLabel
                                            active={
                                                orderBy ===
                                                column.field
                                            }
                                            direction={
                                                orderBy ===
                                                column.field
                                                    ? order
                                                    : "asc"
                                            }
                                            onClick={() =>
                                                handleSort(
                                                    column.field
                                                )
                                            }
                                        >
                                            {
                                                column.headerName
                                            }
                                        </TableSortLabel>
                                    )}
                                </TableCell>
                            ))}
                        </TableRow>
                    </TableHead>

                    <TableBody>
                        {pagedRows.map((row) => (
                            <TableRow
                                key={row.id}
                                hover
                            >
                                {columns.map(
                                    (column) => (
                                        <TableCell
                                            key={String(
                                                column.field
                                            )}
                                            align={
                                                column.align ??
                                                "left"
                                            }
                                        >
                                            {column.render
                                                ? column.render(
                                                      row
                                                  )
                                                : String(
                                                      row[
                                                          column
                                                              .field
                                                      ]
                                                  )}
                                        </TableCell>
                                    )
                                )}
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>

            <TablePagination
                component="div"
                count={rows.length}
                page={page}
                rowsPerPage={rowsPerPage}
                rowsPerPageOptions={[
                    5,
                    10,
                    25,
                    50,
                ]}
                onPageChange={(
                    _,
                    newPage
                ) => setPage(newPage)}
                onRowsPerPageChange={(
                    event
                ) => {
                    setRowsPerPage(
                        Number(event.target.value)
                    );
                    setPage(0);
                }}
            />
        </Paper>
    );
}