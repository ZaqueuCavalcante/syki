<script lang="ts">
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

<svelte:head>
	<title>Disciplinas</title>
</svelte:head>

<div class="flex min-h-screen items-center justify-center">
	<div class="rounded-box border-base-content/5 bg-base-100 overflow-x-auto border">
		<table class="table">
			<thead>
				<tr>
					<th>Nome</th>
					<th>Código</th>
					<th>Professores</th>
				</tr>
			</thead>
			<tbody>
                {#each data.items.slice(0, 10) as item}
                    <tr>
                        <td>{item.name}</td>
                        <td>{item.code}</td>
                        <td>{item.teachers}</td>
                    </tr>
			    {/each}
			</tbody>
		</table>
        <div class="join mt-4 mb-2 justify-center w-full">
            <input class="join-item btn btn-square" type="radio" name="options" aria-label="1" />
            <input class="join-item btn btn-square" type="radio" name="options" aria-label="2" />
            <input class="join-item btn btn-square" type="radio" name="options" aria-label="3" />
            <input class="join-item btn btn-square" type="radio" name="options" aria-label="4" />
        </div>
	</div>
</div>
