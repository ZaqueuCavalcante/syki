<script lang="ts">
    import '../app.css';

    let { data, children } = $props();
</script>

<svelte:head>
  <title>{data.siteTitle}</title>
  <meta name="description" content={data.siteDescription} />

  <meta property="og:title" content={data.siteTitle} />
  <meta property="og:description" content={data.siteDescription} />
  <meta property="og:image" content={data.siteImage} />
  <meta property="og:url" content={data.siteUrl} />
  <meta property="og:type" content="website" />
</svelte:head>

{@render children?.()}
