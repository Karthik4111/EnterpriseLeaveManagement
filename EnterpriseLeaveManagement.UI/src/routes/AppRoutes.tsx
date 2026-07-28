import { Navigate, Route, Routes } from "react-router-dom";

import { ROUTES } from "@/constants/routes";

import LoginPage from "@/pages/auth/LoginPage";
import DashboardPage from "@/pages/dashboard/DashboardPage";
import NotFoundPage from "@/pages/NotFoundPage";

import MainLayout from "@/layouts/MainLayout";
import ProtectedRoute from "@/components/layout/ProtectedRoute/ProtectedRoute";

import EmployeeListPage from "@/pages/employees/EmployeeListPage";

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

            <Route element={<ProtectedRoute />}>
                <Route element={<MainLayout />}>
                    <Route
                        path={ROUTES.DASHBOARD}
                        element={<DashboardPage />}
                    />
                </Route>
            </Route>

            <Route
                path={ROUTES.NOT_FOUND}
                element={<NotFoundPage />}
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
                </Route>
            </Route>
        </Routes>
    );
}