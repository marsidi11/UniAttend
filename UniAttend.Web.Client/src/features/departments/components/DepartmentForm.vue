<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <div>
      <label for="name" class="block text-sm font-medium text-gray-700">Name</label>
      <input
        id="name"
        v-model="form.name"
        type="text"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      />
    </div>

    <div>
      <label class="flex items-center">
        <input
          type="checkbox"
          v-model="form.isActive"
          class="rounded border-gray-300 text-indigo-600 focus:ring-indigo-500"
        />
        <span class="ml-2 text-sm text-gray-600">Active</span>
      </label>
    </div>

    <div class="flex justify-end space-x-3">
      <Button type="button" variant="secondary" @click="$emit('cancel')">Cancel</Button>
      <Button type="submit" :loading="isLoading">Save</Button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import Button from '@/shared/components/ui/Button.vue'
import type { Department } from '@/types/department.types'

interface Props {
  department?: Department | null
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: Partial<Department>): void
  (e: 'cancel'): void
}>()

const isLoading = ref(false)
const form = ref({
  name: '',
  isActive: true,
})

watch(() => props.department, (newDepartment) => {
  if (newDepartment) {
    form.value = {
      name: newDepartment.name,
      isActive: newDepartment.isActive,
    }
  }
}, { immediate: true })

async function handleSubmit() {
  try {
    isLoading.value = true
    emit('submit', { ...form.value })
  } finally {
    isLoading.value = false
  }
}
</script>