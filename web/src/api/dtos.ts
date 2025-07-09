export interface ErrorOut
{
    code: string;
    message: string;
}

export interface ApiResult<T> {
    isSuccess: boolean;
    data?: T;
    error?: ErrorOut;
}
