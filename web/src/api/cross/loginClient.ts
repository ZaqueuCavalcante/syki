import api from "@/syki-api";
import type { AxiosResponse } from "axios";
import type { ApiResult, ErrorOut } from "../dtos";
import type { UUID } from "crypto";

export interface LoginIn {
    email: string;
    password: string;
}

export interface LoginOut
{
    id: UUID;
    name: string;
    email: string;
    accessToken: string;
    role: 'Adm' | 'Academic' | 'Teacher' | 'Student';
}

export default async function login(data: LoginIn): Promise<ApiResult<LoginOut>> {
    let response: AxiosResponse<LoginOut | ErrorOut>;

    try {
        response = await api.post('/login', data, { validateStatus: () => true });
    } catch (error) {
        return { isSuccess: false, error: { code: "0", message: "Error" } };
    }

    if (response.status >= 200 && response.status < 300) {
        return { isSuccess: true, data: response.data as LoginOut };
    } else {
        return { isSuccess: false, error: response.data as ErrorOut };
    }
}
