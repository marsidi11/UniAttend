<template>
  <span
    :class="[
      'inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium',
      statuscourseSessions[status]
    ]"
  >
    <slot></slot>
  </span>
</template>

<script setup lang="ts">
defineProps<{
  status: 'success' | 'warning' | 'error' | 'info'
}>()

const statuscourseSessions = {
  success: 'bg-green-100 text-green-800',
  warning: 'bg-yellow-100 text-yellow-800',
  error: 'bg-red-100 text-red-800',
  info: 'bg-blue-100 text-blue-800'
}
</script>