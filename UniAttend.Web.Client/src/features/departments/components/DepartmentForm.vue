<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <div>
      <label for="name" class="block text-sm font-medium text-gray-700">Name</label>
      <input
        id="name"
        v-model="form.name"
        type="text"
        required
        maxlength="100"
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      />
      <p v-if="error" class="mt-1 text-sm text-red-600">{{ error }}</p>
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
import type { DepartmentDto } from '@/api/generated/data-contracts'

interface Props {
  department?: DepartmentDto | null
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: Partial<DepartmentDto>): void
  (e: 'cancel'): void
}>()

const isLoading = ref(false)
const error = ref('')
const form = ref({
  name: '',
  isActive: true,
})

watch(() => props.department, (newDepartment) => {
  if (newDepartment) {
    form.value = {
      name: newDepartment.name ?? '',
      isActive: newDepartment.isActive ?? true,
    }
  }
}, { immediate: true })

async function handleSubmit() {
  try {
    isLoading.value = true
    error.value = ''

    // Add validation
    if (!form.value.name.trim()) {
      error.value = 'Department name is required'
      return
    }

    if (form.value.name.length > 100) {
      error.value = 'Department name cannot exceed 100 characters'
      return
    }

    emit('submit', { ...form.value })
  } finally {
    isLoading.value = false
  }
}
</script>