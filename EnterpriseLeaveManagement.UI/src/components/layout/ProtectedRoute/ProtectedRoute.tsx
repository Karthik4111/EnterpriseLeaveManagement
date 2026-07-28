import { Navigate, Outlet } from "react-router-dom";

import { ROUTES } from "@/constants/routes";
import { useAuth } from "@/context/AuthContext";

export default function ProtectedRoute() {
    const { auth } = useAuth();

    if (!auth.isAuthenticated) {
        return (
            <Navigate
                to={ROUTES.LOGIN}
                replace
            />
        );
    }

    return <Outlet />;
}