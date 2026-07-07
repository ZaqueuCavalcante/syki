// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  runtimeConfig: {
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
        '@vue/devtools-kit',
        '@vue/devtools-core',
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
      { name: "Saira Condensed", provider: "google", weights: [300, 400, 500, 700] },
    ],
  },
});
