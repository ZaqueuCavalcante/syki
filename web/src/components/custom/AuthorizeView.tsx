import type { PropsWithChildren } from "react";
import { useAuth, type User } from "./AuthProvider";

type AuthorizeViewProps = PropsWithChildren & {
    roles?: User['role'][];
}

export default function AuthorizeView({ roles, children }: AuthorizeViewProps) {
    const { user } = useAuth();

    if (user === undefined) return <></>;

    if (user === null || (roles && !roles.includes(user.role))) {
        return <></>;
    }

    return children;
}
