import type { UUID } from 'crypto';
import type { PageLoad } from './$types';
import { PUBLIC_API_URL } from '$env/static/public';

type GetCampiItemOut = {
    id: UUID;
	name: string;
	city: string;
    state: string;
    capacity: number;
    students: number;
    teachers: number;
	fillRate: number;
};

type GetCampiOut = {
	total: number;
	items: GetCampiItemOut[];
};

export const load: PageLoad = async ({ fetch }) => {
	const res = await fetch(`${PUBLIC_API_URL}/academic/campi`);

	if (!res.ok) {
		throw new Error('Failed to fetch campi');
	}

	return await res.json() as GetCampiOut;
};
