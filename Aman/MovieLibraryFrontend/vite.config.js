import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig({
  plugins: [react()],
  build: {
    outDir: 'dist',
    emptyOutDir: true
  },
  server: {
    open: true,
    port: 5173,
    strictPort: true,
    proxy: {
      '/api': 'https://localhost:5156/api'
    }
  }
});
