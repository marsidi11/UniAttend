<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <!-- Department Filter -->
    <div>
      <label for="departmentId" class="block text-sm font-medium text-gray-700">Department</label>
      <select id="departmentId" v-model="selectedDepartment"
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm">
        <option value="">All Departments</option>
        <option v-for="dept in departments" :key="dept.id" :value="dept.id">
          {{ dept.name }}
        </option>
      </select>
    </div>

    <!-- Search -->
    <div>
      <label for="search" class="block text-sm font-medium text-gray-700">Search Students</label>
      <input id="search" v-model="searchQuery" type="text" placeholder="Search by name or student ID"
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
    </div>

    <!-- Students Selection -->
    <div class="border rounded-md max-h-64 overflow-y-auto">
      <div class="p-3 bg-gray-50 border-b">
        <label class="flex items-center">
          <input type="checkbox" :checked="isAllSelected" @change="toggleSelectAll"
            class="rounded border-gray-300 text-indigo-600 focus:ring-indigo-500" />
          <span class="ml-2 text-sm text-gray-600">Select All</span>
        </label>
      </div>

      <div v-if="isLoadingStudents" class="p-4 text-center">
        <Spinner :size="4" />
      </div>

      <div v-else-if="!filteredStudents.length" class="p-4 text-center text-gray-500">
        No students found
      </div>

      <div v-else class="divide-y">
        <label v-for="student in filteredStudents" :key="student.id"
          class="flex items-center p-3 hover:bg-gray-50 cursor-pointer">
          <input type="checkbox" v-model="selectedStudents" :value="student.id"
            class="rounded border-gray-300 text-indigo-600 focus:ring-indigo-500" />
          <span class="ml-2">
            {{ student.firstName }} {{ student.lastName }}
            <span class="text-sm text-gray-500">({{ student.username }})</span>
          </span>
        </label>
      </div>
    </div>

    <div class="flex justify-end space-x-3">
      <Button type="button" variant="secondary" @click="$emit('cancel')">Cancel</Button>
      <Button type="submit" :loading="isLoading" :disabled="!selectedStudents.length">
        Enroll Selected
      </Button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useStudentStore } from '@/stores/student.store'
import { useDepartmentStore } from '@/stores/department.store'
import Button from '@/shared/components/ui/Button.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import type { UserDetailsDto } from '@/api/generated/data-contracts'

// Define a proper interface for the student data
interface Student extends UserDetailsDto {
  id: number
  firstName: string
  lastName: string
  username: string // This is used as studentId
  departmentId: number
  isActive: boolean
}

defineProps<{
  groupId: number
}>()

const emit = defineEmits<{
  (e: 'submit', studentIds: number[]): void
  (e: 'cancel'): void
}>()

// Store setup
const studentStore = useStudentStore()
const departmentStore = useDepartmentStore()

const { students } = storeToRefs(studentStore)
const { departments } = storeToRefs(departmentStore)

// Component state
const isLoading = ref(false)
const isLoadingStudents = ref(false)
const selectedDepartment = ref('')
const searchQuery = ref('')
const selectedStudents = ref<number[]>([])

// Computed properties with proper type casting
const filteredStudents = computed(() => {
  let filtered = students.value
    .filter((s): s is Student => {
      if (!s) return false
      return s.isActive === true
    })

  if (selectedDepartment.value) {
    filtered = filtered.filter(s =>
      s.departmentId === Number(selectedDepartment.value)
    )
  }

  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase()
    filtered = filtered.filter(s =>
      s.firstName?.toLowerCase().includes(query) ||
      s.lastName?.toLowerCase().includes(query) ||
      s.username?.toLowerCase().includes(query)
    )
  }

  return filtered
})

const isAllSelected = computed(() =>
  filteredStudents.value.length > 0 &&
  filteredStudents.value.every(s => selectedStudents.value.includes(s.id))
)

// Methods
function toggleSelectAll() {
  if (isAllSelected.value) {
    selectedStudents.value = []
  } else {
    selectedStudents.value = filteredStudents.value.map(s => s.id)
  }
}

async function loadData() {
  isLoadingStudents.value = true
  try {
    await Promise.all([
      studentStore.fetchStudentsList(),
      departmentStore.fetchDepartments()
    ])
  } catch (err) {
    console.error('Failed to load data:', err)
  } finally {
    isLoadingStudents.value = false
  }
}

async function handleSubmit() {
  try {
    isLoading.value = true
    emit('submit', selectedStudents.value)
  } finally {
    isLoading.value = false
  }
}

// Lifecycle hooks
onMounted(() => {
  loadData()
})
</script>