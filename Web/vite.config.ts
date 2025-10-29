import tailwindcss from '@tailwindcss/vite';

import { defineConfig } from 'vite';
import { sveltekit } from '@sveltejs/kit/vite';

export default defineConfig({
	plugins: [tailwindcss(), sveltekit()],
	resolve: process.env.VITEST ? { conditions: ['browser'] } : undefined
});
