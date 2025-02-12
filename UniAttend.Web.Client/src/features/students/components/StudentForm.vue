<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <div class="grid grid-cols-2 gap-4">
      <!-- First Name -->
      <div>
        <label for="firstName" class="block text-sm font-medium text-gray-700">First Name</label>
        <input
          id="firstName"
          v-model="form.firstName"
          type="text"
          required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>

      <!-- Last Name -->
      <div>
        <label for="lastName" class="block text-sm font-medium text-gray-700">Last Name</label>
        <input
          id="lastName"
          v-model="form.lastName"
          type="text"
          required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
    </div>

    <!-- Student ID -->
    <div>
      <label for="studentId" class="block text-sm font-medium text-gray-700">Student ID</label>
      <input
        id="studentId"
        v-model="form.studentId"
        type="text"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      />
    </div>

    <!-- Email -->
    <div>
      <label for="email" class="block text-sm font-medium text-gray-700">Email</label>
      <input
        id="email"
        v-model="form.email"
        type="email"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      />
    </div>

    <!-- Department -->
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

    <!-- Card ID -->
    <div>
      <label for="cardId" class="block text-sm font-medium text-gray-700">Card ID</label>
      <input
        id="cardId"
        v-model="form.cardId"
        type="text"
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      />
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
import type { 
  RegisterStudentCommand, 
  DepartmentDto,
  UserDetailsDto 
} from '@/api/generated/data-contracts'

interface Props {
  student?: UserDetailsDto | null
  departments: DepartmentDto[]
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: RegisterStudentCommand): void
  (e: 'cancel'): void
}>()

const isLoading = ref(false)
const form = ref<RegisterStudentCommand>({
  firstName: '',
  lastName: '',
  studentId: '',
  email: '',
  departmentId: 0,
  cardId: ''
})

watch(() => props.student, (newStudent) => {
  if (newStudent) {
    form.value = {
      firstName: newStudent.firstName ?? '',
      lastName: newStudent.lastName ?? '',
      studentId: newStudent.username ?? '',
      email: newStudent.email ?? '',
      departmentId: newStudent.departmentId ?? 0,
      cardId: ''
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