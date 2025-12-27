import { posts } from '../data.js';
import { error } from '@sveltejs/kit';
import type { PageServerLoad } from './$types';

export const load: PageServerLoad = ({ params }) => {
	const post = posts.find((post) => post.slug === params.slug);

    if (!post) error(404);

	return {
		post
	};
};
