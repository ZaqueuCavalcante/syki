<script lang="ts">
	let { data } = $props();
</script>

<h1>blog post</h1>

<h1>{data.post.title}</h1>
<div>{@html data.post.content}</div>
