<template>
  <router-view v-slot="{ Component }">
    <component :is="getLayout">
      <component :is="Component" />
    </component>
  </router-view>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import MainLayout from '@/shared/layouts/MainLayout.vue';
import AuthLayout from '@/shared/layouts/AuthLayout.vue';
import DashboardLayout from '@/shared/layouts/DashboardLayout.vue';

const route = useRoute();
const getLayout = computed(() => {
  if (route.meta.requiresAuth === false) return AuthLayout;
  if (route.path.includes('dashboard')) return DashboardLayout;
  return MainLayout;
});
</script>