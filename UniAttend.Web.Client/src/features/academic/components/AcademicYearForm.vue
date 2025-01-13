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
      <label for="startDate" class="block text-sm font-medium text-gray-700">Start Date</label>
      <input
        id="startDate"
        v-model="form.startDate"
        type="date"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      />
    </div>

    <div>
      <label for="endDate" class="block text-sm font-medium text-gray-700">End Date</label>
      <input
        id="endDate"
        v-model="form.endDate"
        type="date"
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
import type { AcademicYearDto, CreateAcademicYearCommand } from '@/api/generated/data-contracts'

interface Props {
  academicYear?: AcademicYearDto | null
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: CreateAcademicYearCommand): void;
  (e: 'cancel'): void;
}>()

const isLoading = ref(false)
const form = ref({
  name: '',
  startDate: '',
  endDate: '',
  isActive: true,
})

watch(() => props.academicYear, (newYear) => {
  if (newYear) {
    form.value = {
      name: newYear.name ?? '',
      startDate: formatDateForInput(newYear.startDate ?? ''),
      endDate: formatDateForInput(newYear.endDate ?? ''),
      isActive: newYear.isActive ?? true,
    }
  }
}, { immediate: true })

function formatDateForInput(date: string): string {
  return new Date(date).toISOString().split('T')[0]
}

async function handleSubmit() {
  try {
    isLoading.value = true
    emit('submit', {
      name: form.value.name,
      startDate: form.value.startDate ? new Date(form.value.startDate).toISOString() : undefined,
      endDate: form.value.endDate ? new Date(form.value.endDate).toISOString() : undefined
    })
  } finally {
    isLoading.value = false
  }
}
</script>