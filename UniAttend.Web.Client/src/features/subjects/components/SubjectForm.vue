<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <!-- Name Field -->
    <div>
      <label for="name" class="block text-sm font-medium text-gray-700">Name</label>
      <input id="name" v-model="form.name" type="text" required maxlength="255"
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
    </div>

    <!-- Description Field -->
    <div>
      <label for="description" class="block text-sm font-medium text-gray-700">Description</label>
      <textarea id="description" v-model="form.description" rows="3" maxlength="1000" required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"></textarea>
    </div>

    <!-- Credits Field -->
    <div>
      <label for="credits" class="block text-sm font-medium text-gray-700">Credits</label>
      <input id="credits" v-model.number="form.credits" type="number" min="1" max="30" required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
    </div>

    <!-- Department Field -->
    <div>
      <label for="departmentId" class="block text-sm font-medium text-gray-700">Department</label>
      <select id="departmentId" v-model.number="form.departmentId" required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm">
        <option value="">Select Department</option>
        <option v-for="dept in departments" :key="dept.id" :value="dept.id">
          {{ dept.name }}
        </option>
      </select>
    </div>

    <!-- Submit Buttons -->
    <div class="flex justify-end space-x-3">
      <Button type="button" variant="secondary" @click="$emit('cancel')">Cancel</Button>
      <Button type="submit" :loading="isLoading">Save</Button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import Button from '@/shared/components/ui/Button.vue'
import type { SubjectDto, DepartmentDto, CreateSubjectCommand } from '@/api/generated/data-contracts'

interface Props {
  subject?: SubjectDto | null
  departments: DepartmentDto[]
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: CreateSubjectCommand): void
  (e: 'cancel'): void
}>()

const isLoading = ref(false)
const form = ref<CreateSubjectCommand>({
  name: '',
  departmentId: 0,
  description: '',
  credits: 1,
})

watch(() => props.subject, (newSubject) => {
  if (newSubject) {
    form.value = {
      name: newSubject.name,
      departmentId: newSubject.departmentId,
      description: newSubject.description ?? '',
      credits: newSubject.credits,
    }
  }
}, { immediate: true })

async function handleSubmit() {
  try {
    isLoading.value = true

    // Validate required fields
    if (!form.value.departmentId || form.value.departmentId === 0) {
      throw new Error('Department is required')
    }

    if (!form.value.name?.trim()) {
      throw new Error('Name is required')
    }

    if (!form.value.credits || form.value.credits < 1 || form.value.credits > 30) {
      throw new Error('Credits must be between 1 and 30')
    }

    console.log('Submitting form:', form.value) // Debug log
    emit('submit', form.value)
  } catch (error) {
    console.error('Form submission error:', error)
  } finally {
    isLoading.value = false
  }
}
</script>