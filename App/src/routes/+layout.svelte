<script lang="ts">
	import "../app.css";
	let { children } = $props();
</script>

<svelte:head>
	<title>Estud</title>
</svelte:head>

{@render children()}
