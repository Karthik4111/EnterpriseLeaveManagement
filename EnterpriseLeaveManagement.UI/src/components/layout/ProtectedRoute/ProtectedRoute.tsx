import { Navigate, Outlet } from "react-router-dom";

import { ROUTES } from "@/constants/routes";
import { useAuth } from "@/context/AuthContext";
import type { Role } from "@/constants/roles";

interface ProtectedRouteProps {
    allowedRoles?: Role[];
}

export default function ProtectedRoute({
    allowedRoles,
}: ProtectedRouteProps) {
    const { auth } = useAuth();

    if (!auth.isAuthenticated) {
        return (
            <Navigate
                to={ROUTES.LOGIN}
                replace
            />
        );
    }

    if (
        allowedRoles &&
        allowedRoles.length > 0 &&
        (!auth.user ||
            !allowedRoles.includes(auth.user.role as Role))
    ) {
        return (
            <Navigate
                to={ROUTES.UNAUTHORIZED}
                replace
            />
        );
    }

    return <Outlet />;
}