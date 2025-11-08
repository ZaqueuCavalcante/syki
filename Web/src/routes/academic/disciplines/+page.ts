import type { UUID } from 'crypto';
import type { PageLoad } from './$types';
import { PUBLIC_API_URL } from '$env/static/public';

type GetDisciplinesOut = {
	total: number;
	items: GetDisciplinesItemOut[];
};

type GetDisciplinesItemOut = {
    id: UUID;
	name: string;
	code: string;
    teachers: number;
};

export const load: PageLoad = async ({ fetch }) => {
	const res = await fetch(`${PUBLIC_API_URL}/academic/disciplines`, { credentials: 'include' });

	if (!res.ok) {
		throw new Error('Failed to fetch disciplines');
	}

	let items = await res.json();

    return { total: items.length, items } as GetDisciplinesOut;
};
