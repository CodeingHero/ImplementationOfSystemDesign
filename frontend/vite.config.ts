import { fileURLToPath, URL } from 'node:url';
import { defineConfig, loadEnv } from 'vite';
import vue from '@vitejs/plugin-vue';
import AutoImport from 'unplugin-auto-import/vite';
import Components from 'unplugin-vue-components/vite';
import { ElementPlusResolver } from 'unplugin-vue-components/resolvers';
import IconResolver from 'unplugin-icons/resolver';
import Icons from 'unplugin-icons/vite';

// https://vitejs.dev/config/
export default defineConfig({
  server: {
    proxy: {
      '/user': {
        target: loadEnv('', process.cwd()).VITE_SYSTEM_DESIGN_API_URL,
        changeOrigin: true,
        secure: false
      },
      '/sd': {
        target: loadEnv('', process.cwd()).VITE_SYSTEM_DESIGN_API_URL,
        changeOrigin: true,
        secure: false
      },
      '/s/': {
        target: loadEnv('', process.cwd()).VITE_SYSTEM_DESIGN_API_URL,
        changeOrigin: true,
        secure: false
      }
    }
  },
  plugins: [
    vue(),
    AutoImport({
      imports: ['vue'],
      resolvers: [ElementPlusResolver(), IconResolver()],
      eslintrc: { enabled: true }
    }),
    Components({
      resolvers: [
        ElementPlusResolver(),
        IconResolver({
          enabledCollections: ['ep']
        })
      ]
    }),
    Icons({
      autoInstall: true
    })
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  }
});
