import {
    BrowserRouter,
    Route,
    Routes,
} from "react-router-dom";

import LoginPage from "@/features/authentication/pages/LoginPage";
import DashboardPage from "@/features/dashboard/pages/DashboardPage";

import ProtectedRoute from "./ProtectedRoute";
import PublicRoute from "./PublicRoute";

import UnauthorizedPage from "@/pages/UnauthorizedPage";
import NotFoundPage from "@/pages/NotFoundPage";

function AppRoutes() {
    return (
        <BrowserRouter>
            <Routes>
                <Route element={<PublicRoute />}>
                    <Route
                        path="/login"
                        element={<LoginPage />}
                    />
                </Route>

                <Route element={<ProtectedRoute />}>
                    <Route
                        path="/"
                        element={<DashboardPage />}
                    />
                </Route>

                <Route
                    path="/unauthorized"
                    element={<UnauthorizedPage />}
                />

                <Route
                    path="*"
                    element={<NotFoundPage />}
                />
            </Routes>
        </BrowserRouter>
    );
}

export default AppRoutes;