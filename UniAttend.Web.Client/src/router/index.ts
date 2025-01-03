import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '@/stores/auth.store';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      component: () => import('@/shared/layouts/MainLayout.vue'),
      meta: { requiresAuth: false }
    },
    {
      path: '/login',
      component: () => import('@/features/auth/pages/LoginPage.vue'),
      meta: { requiresAuth: false }
    },
    {
      path: '/register',
      component: () => import('@/features/auth/pages/RegisterPage.vue'),
      meta: { requiresAuth: false }
    },
    {
      path: '/dashboard',
      component: () => import('@/features/dashboard/pages/DashboardPage.vue'),
      meta: { requiresAuth: true }
    }
  ]
});

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  if (to.meta.requiresAuth && !authStore.token) {
    next('/login');
  } else {
    next();
  }
});

export default router;