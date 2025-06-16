import { UUID } from "crypto";

type Params = Promise<{ id: UUID }>;

export default async function ClassPage({ params } : { params: Params }) {
    const { id } = await params;
    return (
        <div>CLASS WITH ID = {id}</div>
    )
}
