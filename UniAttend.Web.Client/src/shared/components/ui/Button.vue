<template>
  <button
    :class="[
      'px-4 py-2 rounded-md font-medium focus:outline-none focus:ring-2 focus:ring-offset-2',
      variantClasses[variant],
      { 'opacity-50 cursor-not-allowed': disabled }
    ]"
    :disabled="disabled"
    :type="type"
    @click="$emit('click')"
  >
    <slot></slot>
  </button>
</template>

<script setup lang="ts">
defineProps<{
  variant?: 'primary' | 'secondary' | 'danger'
  disabled?: boolean
  type?: 'button' | 'submit'
}>()

const variantClasses = {
  primary: 'bg-indigo-600 text-white hover:bg-indigo-700 focus:ring-indigo-500',
  secondary: 'bg-gray-200 text-gray-700 hover:bg-gray-300 focus:ring-gray-500',
  danger: 'bg-red-600 text-white hover:bg-red-700 focus:ring-red-500'
}
</script>