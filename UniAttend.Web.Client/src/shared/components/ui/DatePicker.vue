<template>
  <input
    type="date"
    :value="modelValue"
    @input="$emit('update:modelValue', ($event.target as HTMLInputElement).value)"
    class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500"
  />
</template>

<script setup lang="ts">
defineProps<{
  modelValue?: string
  placeholder?: string
}>();

defineEmits<{
  (e: 'update:modelValue', value: string): void
}>();
</script>