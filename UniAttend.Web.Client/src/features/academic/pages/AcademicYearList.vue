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
      <label for="departmentId" class="block text-sm font-medium text-gray-700">Department</label>
      <select
        id="departmentId"
        v-model="form.departmentId"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      >
        <option v-for="dept in departments" :key="dept.id" :value="dept.id">
          {{ dept.name }}
        </option>
      </select>
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
import type { DepartmentDto, SubjectDto } from '@/api/generated/data-contracts'

interface FormData {
  name: string;
  departmentId: number;
  isActive: boolean;
}

interface Props {
  subject?: SubjectDto | null;
  departments: DepartmentDto[];
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: FormData): void;
  (e: 'cancel'): void;
}>()

const isLoading = ref(false)
const form = ref<FormData>({
  name: '',
  departmentId: 0,
  isActive: true,
})

watch(() => props.subject, (newSubject) => {
  if (newSubject) {
    form.value = {
      name: newSubject.name ?? '',
      departmentId: newSubject.departmentId ?? 0,
      isActive: newSubject.isActive ?? true,
    }
  }
}, { immediate: true })

async function handleSubmit() {
  try {
    isLoading.value = true
    emit('submit', form.value)
  } finally {
    isLoading.value = false
  }
}
</script>