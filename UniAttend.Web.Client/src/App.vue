<!-- App.vue -->
<template>
  <component :is="layout">
    <router-view v-slot="{ Component }">
      <component :is="Component" />
    </router-view>
  </component>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import MainLayout from '@/shared/layouts/MainLayout.vue';
import AuthLayout from '@/shared/layouts/AuthLayout.vue';
import DashboardLayout from '@/shared/layouts/DashboardLayout.vue';

const route = useRoute();

const layout = computed(() => {
  // If route is within dashboard
  if (route.path.startsWith('/dashboard')) {
    return DashboardLayout;
  }

  // If route requires auth layout
  if (route.meta.layout === 'auth') {
    return AuthLayout;
  }

  // Default to main layout
  return MainLayout;
});
</script>