import { Navigate } from "react-router";
import type { PropsWithChildren } from "react";
import { useAuth, type User } from "./AuthProvider";

type ProtectedRouteProps = PropsWithChildren & {
    roles?: User['role'][];
}
export default function ProtectedRoute({ roles, children }: ProtectedRouteProps) {
    const { user } = useAuth();

    if (user === undefined) return <></>;

    if (user === null) {
        return <Navigate to="/login" replace />;
    }

    if (roles && !roles.includes(user.role)) {
        console.log(roles)
        console.log(user)
        return <Navigate to="/unauthorized" replace />;
    }

    return <>{children}</>;
};
