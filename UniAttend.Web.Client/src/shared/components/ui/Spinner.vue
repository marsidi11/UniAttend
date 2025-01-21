<template>
  <div
    :class="[
      'inline-block animate-spin rounded-full border-4 border-solid',
      `h-${size} w-${size}`,
      colorClasses[color]
    ]"
    role="status"
  >
    <span class="sr-only">Loading...</span>
  </div>
</template>

<script setup lang="ts">
type Color = 'primary' | 'white'
type Size = 4 | 6 | 8

interface Props {
  size?: Size
  color?: Color
}

withDefaults(defineProps<Props>(), {
  size: 6,
  color: 'primary'
})

const colorClasses: Record<Color, string> = {
  primary: 'border-indigo-600 border-r-transparent',
  white: 'border-white border-r-transparent'
} as const
</script>