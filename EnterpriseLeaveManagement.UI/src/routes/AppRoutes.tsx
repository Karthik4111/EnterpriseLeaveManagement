import { Navigate, Route, Routes } from "react-router-dom";

import { ROUTES } from "@/constants/routes";
import { ROLES } from "@/constants/roles";

import LoginPage from "@/pages/auth/LoginPage";
import UnauthorizedPage from "@/pages/auth/UnauthorizedPage";
import DashboardPage from "@/pages/dashboard/DashboardPage";
import NotFoundPage from "@/pages/NotFoundPage";

import MainLayout from "@/layouts/MainLayout";
import ProtectedRoute from "@/components/layout/ProtectedRoute/ProtectedRoute";

import EmployeeListPage from "@/pages/employees/EmployeeListPage";
import DepartmentListPage from "@/pages/departments/DepartmentListPage";
import LeaveRequestPage from "@/pages/leave/LeaveRequestPage";
import MyLeavesPage from "@/pages/leave/MyLeavesPage";
import LeaveApprovalPage from "@/pages/leave/LeaveApprovalPage";
import LeaveBalancePage from "@/pages/leave/LeaveBalancePage";
import ProfilePage from "@/pages/profile/ProfilePage";

export default function AppRoutes() {
    return (
        <Routes>
            <Route
                path={ROUTES.ROOT}
                element={
                    <Navigate
                        to={ROUTES.LOGIN}
                        replace
                    />
                }
            />

            <Route
                path={ROUTES.LOGIN}
                element={<LoginPage />}
            />

            <Route
                path={ROUTES.UNAUTHORIZED}
                element={<UnauthorizedPage />}
            />

            <Route element={<ProtectedRoute />}>
                <Route element={<MainLayout />}>
                    <Route
                        path={ROUTES.DASHBOARD}
                        element={<DashboardPage />}
                    />

                    <Route
                        path={ROUTES.EMPLOYEES}
                        element={<EmployeeListPage />}
                    />

                    <Route
                        path={ROUTES.DEPARTMENTS}
                        element={<DepartmentListPage />}
                    />

                    <Route
                        path={ROUTES.LEAVE_REQUEST}
                        element={<LeaveRequestPage />}
                    />

                    <Route
                        path={ROUTES.MY_LEAVES}
                        element={<MyLeavesPage />}
                    />

                    <Route
                        path={ROUTES.LEAVE_BALANCE}
                        element={<LeaveBalancePage />}
                    />

                    <Route
                        path={ROUTES.PROFILE}
                        element={<ProfilePage />}
                    />
                </Route>
            </Route>

            <Route
                element={
                    <ProtectedRoute
                        allowedRoles={[
                            ROLES.MANAGER,
                            ROLES.ADMIN,
                        ]}
                    />
                }
            >
                <Route element={<MainLayout />}>
                    <Route
                        path={ROUTES.LEAVE_APPROVALS}
                        element={<LeaveApprovalPage />}
                    />
                </Route>
            </Route>

            <Route
                path={ROUTES.NOT_FOUND}
                element={<NotFoundPage />}
            />
        </Routes>
    );
}