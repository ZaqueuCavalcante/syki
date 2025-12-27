import tailwindcss from '@tailwindcss/vite';

import { defineConfig } from 'vite';
import { sveltekit } from '@sveltejs/kit/vite';

export default defineConfig({
	plugins: [sveltekit(), tailwindcss()],
    resolve: process.env.VITEST ? { conditions: ['browser'] } : undefined
});
