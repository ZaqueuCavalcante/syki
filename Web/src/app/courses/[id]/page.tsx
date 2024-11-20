export default async function Courses({
    params,
}: {
    params: Promise<{ id: string }>
}) {
    const { id } = await params
    return (
        <div className="grid grid-rows-[20px_1fr_20px] items-center justify-items-center min-h-screen p-8 pb-20 gap-16 sm:p-20">
            <div className="flex gap-4 items-center flex-col sm:flex-row">
                <h5>{id}</h5>
            </div>
        </div>
    );
}
