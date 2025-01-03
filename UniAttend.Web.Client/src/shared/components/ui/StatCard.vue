<template>
  <div class="bg-white overflow-hidden shadow rounded-lg">
    <div class="px-4 py-5 sm:p-6">
      <dt class="text-sm font-medium text-gray-500 truncate">
        {{ title }}
      </dt>
      <dd class="mt-1 text-3xl font-semibold text-gray-900">
        {{ value }}
      </dd>
      <div v-if="change" class="mt-2">
        <span
          :class="[
            'inline-flex items-baseline text-sm font-semibold',
            change >= 0 ? 'text-green-600' : 'text-red-600'
          ]"
        >
          {{ change >= 0 ? '+' : '' }}{{ change }}%
        </span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  title: string
  value: string | number
  change?: number
}>()
</script>