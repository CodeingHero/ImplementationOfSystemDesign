import { createRouter, createWebHistory } from 'vue-router';
import { tokenManager } from '@/stores/myToken';
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      //主页有两个个children,默认的是推一个AppLayout，因为在AppLayout里面有个洞(RouterView)
      //所以用children的形式给这个洞
      path: '/',
      name: 'home',
      component: () => import('@/components/Layout/AppLayout.vue'),
      meta: { requireAuth: true },
      children: [
        {
          path: '',
          name: 'index',
          component: () => import('@/views/IndexView.vue'),
          meta: { breadcrumb: '' } // Add breadcrumb here
        },
        {
          path: '/basic',
          name: 'basic',
          component: () => import('@/views/Introduction/IntroView.vue'),
          meta: { breadcrumb: 'Basic' }
        },
        {
          path: '/userInfo',
          name: 'userInfo',
          component: () => import('@/views/User/UserInfoView.vue'),
          meta: { breadcrumb: 'UserInfo' }
        },
        {
          path: '/about',
          name: 'about',
          component: () => import('../views/AboutView.vue'),
          meta: { breadcrumb: 'About' }
        },
        {
          path: '/shortenUrl',
          name: 'shortenUrl',
          component: () => import('@/views/Systems/ShortenUrl/ShortenUrlView.vue'),
          meta: { breadcrumb: 'ShortenUrl' }
        },
        {
          path: '/pasteBin/',
          name: 'pasteBin',
          component: () => import('@/views/Systems/Pastebin/PasteBinView.vue'),
          meta: { breadcrumb: 'PasteBin' }
        }
      ]
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/Login/LoginView.vue'),
      meta: { breadcrumb: 'Login' }
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'ErrorPage',
      component: () => import('../views/ErrorView.vue'),
      meta: { breadcrumb: 'ErrorPage' }
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('@/views/Login/RegisterView.vue'),
      meta: { breadcrumb: 'Register' }
    }
  ]
});

router.beforeEach((to, from, next) => {
  if (to.matched.some((r) => r.meta?.requireAuth)) {
    const store = tokenManager();
    if (!store.token?.accessToken) {
      next({ name: 'login', query: { redirect: to.fullPath } });
      return;
    }
  }
  next();
});

export default router;
