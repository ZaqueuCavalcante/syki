<script lang="ts">
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

<svelte:head>
	<title>Campi</title>
</svelte:head>

<div class="flex min-h-screen items-center justify-center">
	<div class="rounded-box border-base-content/5 bg-base-100 overflow-x-auto border">
		<table class="table">
			<thead>
				<tr>
					<th>Nome</th>
					<th>Cidade</th>
					<th>Professores</th>
					<th>Alunos</th>
					<th>Capacidade</th>
					<th>Ocupação</th>
				</tr>
			</thead>
			<tbody>
                {#each data.items as item}
                    <tr>
                        <td>{item.name}</td>
                        <td>{item.city} - {item.state}</td>
                        <td>{item.teachers}</td>
                        <td>{item.students}</td>
                        <td>{item.capacity}</td>
                        <td>{item.fillRate}%</td>
                    </tr>
			    {/each}
			</tbody>
		</table>
	</div>
</div>
