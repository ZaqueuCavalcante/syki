// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  runtimeConfig: {
    // Server-only. Usada nas chamadas SSR ao backend (ex.: checagem de auth da landing).
    // Se vazia, cai no backendUrl público. Aponte para a URL interna (ex.: rede privada
    // da Railway) para reduzir a latência da checagem no caminho do usuário logado.
    internalBackendUrl: "",
    public: {
      backendUrl: "",
    },
  },

  modules: [
    "@nuxt/ui",
    "@nuxt/fonts",
    "@nuxt/eslint",
    "@vueuse/nuxt",
    "@nuxt/content",
  ],

  app: {
    head: {
      title: 'Estud',
      htmlAttrs: { lang: 'pt-BR' },
      meta: [
        { name: 'description', content: 'Gestão acadêmica para quem leva a educação a sério' },
        { name: 'robots', content: 'noindex, nofollow' },
        { property: 'og:site_name', content: 'Estud' },
        { property: 'og:locale', content: 'pt_BR' },
        { property: 'og:type', content: 'website' },
        { property: 'og:title', content: 'Estud' },
        { property: 'og:description', content: 'Gestão acadêmica para quem leva a educação a sério' },
        { property: 'og:image', content: 'https://estud.com.br/images/home-page.png' },
        { property: 'og:image:type', content: 'image/png' },
        { property: 'og:image:width', content: '2400' },
        { property: 'og:image:height', content: '1260' },
        { property: 'og:image:alt', content: 'Tela inicial do Estud' },
        { property: 'og:url', content: 'https://estud.com.br' },
        { name: 'twitter:card', content: 'summary_large_image' },
        { name: 'twitter:title', content: 'Estud' },
        { name: 'twitter:description', content: 'Gestão acadêmica para quem leva a educação a sério' },
        { name: 'twitter:image', content: 'https://estud.com.br/images/home-page.png' },
        { name: 'twitter:image:alt', content: 'Tela inicial do Estud' }
      ],
      link: [
        { rel: 'canonical', href: 'https://estud.com.br' }
      ]
    }
  },

  content: {
    build: {
      markdown: {
        toc: { depth: 3, searchDepth: 3 },
      },
    },
  },

  devtools: {
    enabled: true,
  },

  css: ["~/assets/css/main.css"],

  routeRules: {
    "/api/**": {
      cors: true,
    },
  },

  vite: {
    optimizeDeps: {
      include: [
        "zod",
        "date-fns",
        "@unovis/vue",
        "@vue/devtools-kit",
        "@vue/devtools-core",
        "@tanstack/vue-table",
        "@internationalized/date",
      ],
    },
  },

  compatibilityDate: "2024-07-11",

  eslint: {
    config: {
      stylistic: {
        braceStyle: "1tbs",
        commaDangle: "never",
      },
    },
  },

  fonts: {
    families: [
      { name: "Saira", provider: "google", weights: [300, 400, 500, 600, 700] },
      {
        name: "Saira Condensed",
        provider: "google",
        weights: [300, 400, 500, 700],
      },
    ],
  },
});
