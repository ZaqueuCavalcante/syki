<script lang="ts">
    import '../app.css';

    let isDrawerOpen = $state(true);

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

<div class="drawer drawer-open">
  <input id="my-drawer-4" type="checkbox" class="drawer-toggle" />

  <div class="drawer-content">
    {@render children?.()}
  </div>

  <div class="drawer-side is-drawer-close:overflow-visible">
    <label for="my-drawer-4" aria-label="close sidebar" class="drawer-overlay"></label>
    <div class="is-drawer-close:w-14 is-drawer-open:w-64 bg-base-200 flex flex-col items-start min-h-full">
      <ul class="menu w-full grow">

        <li>
          <a href="/" class="is-drawer-close:tooltip is-drawer-close:tooltip-right" data-tip="Home" aria-label="Home">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" stroke-linejoin="round" stroke-linecap="round" stroke-width="2" fill="none" stroke="currentColor" class="inline-block size-4 my-1.5"><path d="M15 21v-8a1 1 0 0 0-1-1h-4a1 1 0 0 0-1 1v8"></path><path d="M3 10a2 2 0 0 1 .709-1.528l7-5.999a2 2 0 0 1 2.582 0l7 5.999A2 2 0 0 1 21 10v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"></path></svg>
            <span class="is-drawer-close:hidden">Home</span>
          </a>
        </li>

        <li>
          <a href="/academic/campi" class="is-drawer-close:tooltip is-drawer-close:tooltip-right" data-tip="Campi" aria-label="Campi">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" stroke-linejoin="round" stroke-linecap="round" stroke-width="2" fill="none" stroke="currentColor" class="inline-block size-4 my-1.5"><path d="M20 7h-9"></path><path d="M14 17H5"></path><circle cx="17" cy="17" r="3"></circle><circle cx="7" cy="7" r="3"></circle></svg>
            <span class="is-drawer-close:hidden">Campi</span>
          </a>
        </li>

        <li>
          <a href="/academic/disciplines" class="is-drawer-close:tooltip is-drawer-close:tooltip-right" data-tip="Disciplinas" aria-label="Disciplinas">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" stroke-linejoin="round" stroke-linecap="round" stroke-width="2" fill="none" stroke="currentColor" class="inline-block size-4 my-1.5"><path d="M20 7h-9"></path><path d="M14 17H5"></path><circle cx="17" cy="17" r="3"></circle><circle cx="7" cy="7" r="3"></circle></svg>
            <span class="is-drawer-close:hidden">Disciplinas</span>
          </a>
        </li>
      </ul>
    </div>
  </div>
</div>
