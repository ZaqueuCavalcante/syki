import { items } from './items.js';

export function load() {
	return {
		info: items.map((item) => ({
			value: item.value,
			title: item.title,
			content: item.content,
		}))
	};
}
