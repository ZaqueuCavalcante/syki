import { expect, test } from 'vitest';

test('Should multiply', () => {
	let double = 2 * 2.5;

	expect(double).toEqual(5.0);
});
