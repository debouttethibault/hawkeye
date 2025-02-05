import { fileURLToPath, URL } from 'node:url';
import TailwindCSS from '@tailwindcss/vite';

import { defineConfig } from 'vite';
import Vue from '@vitejs/plugin-vue';
import VueDevTools from 'vite-plugin-vue-devtools';

// https://vite.dev/config/
export default defineConfig({
  plugins: [Vue(), VueDevTools(), TailwindCSS()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
    },
  },
});
