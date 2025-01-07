<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Students</h1>
      <Button @click="openCreateModal">Add Student</Button>
    </div>

    <!-- Filters -->
    <div class="flex gap-4 bg-white p-4 rounded-lg shadow">
      <div class="w-64">
        <label class="block text-sm font-medium text-gray-700">Department</label>
        <select
          v-model="selectedDepartment"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          <option value="">All Departments</option>
          <option v-for="dept in departments" :key="dept.id" :value="dept.id">
            {{ dept.name }}
          </option>
        </select>
      </div>
      <div class="w-64">
        <label class="block text-sm font-medium text-gray-700">Status</label>
        <select
          v-model="selectedStatus"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          <option value="">All</option>
          <option value="true">Active</option>
          <option value="false">Inactive</option>
        </select>
      </div>
    </div>

    <!-- Students Table -->
    <div class="bg-white shadow rounded-lg">
      <DataTable
        :data="filteredStudents"
        :columns="columns"
        :loading="isLoading"
        :actions="tableActions"
        @row-click="handleRowClick"
      />
    </div>

    <!-- Create/Edit Student Modal -->
    <Modal v-model="showModal" :title="modalTitle">
      <StudentForm
        v-if="showModal"
        :student="selectedStudent"
        :departments="departments"
        @submit="handleSubmit"
        @cancel="showModal = false"
      />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useStudentStore } from '@/stores/student.store'
import { useDepartmentStore } from '@/stores/department.store'
import type { Student } from '@/types/student.types'
import Button from '@/shared/components/ui/Button.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import StudentForm from '../components/StudentForm.vue'
import type { TableItem } from '@/types/tableItem.types'

const router = useRouter()
const studentStore = useStudentStore()
const departmentStore = useDepartmentStore()

const { students, isLoading } = storeToRefs(studentStore)
const { departments } = storeToRefs(departmentStore)

const showModal = ref(false)
const selectedStudent = ref<Student | null>(null)
const selectedDepartment = ref('')
const selectedStatus = ref('')

const columns = [
  { key: 'studentId', label: 'Student ID' },
  { key: 'fullName', label: 'Full Name' },
  { key: 'departmentName', label: 'Department' },
  { key: 'email', label: 'Email' },
  { key: 'isActive', label: 'Status',
    render: (value: boolean) => value ? 'Active' : 'Inactive'
  }
]

const tableActions = [
  { 
    label: 'View Details', 
    icon: 'visibility', 
    action: (item: TableItem) => handleViewDetails(item as Student)
  },
  { 
    label: 'Edit', 
    icon: 'edit', 
    action: (item: TableItem) => handleEdit(item as Student)
  }
]

const modalTitle = computed(() => 
  selectedStudent.value ? 'Edit Student' : 'Add Student'
)

const filteredStudents = computed(() => {
  let filtered = [...students.value]
  
  if (selectedDepartment.value) {
    filtered = filtered.filter(s => s.departmentId === Number(selectedDepartment.value))
  }
  
  if (selectedStatus.value !== '') {
    filtered = filtered.filter(s => s.isActive === (selectedStatus.value === 'true'))
  }
  
  return filtered
})

onMounted(async () => {
  await Promise.all([
    studentStore.fetchStudents(),
    departmentStore.fetchDepartments()
  ])
})

function openCreateModal() {
  selectedStudent.value = null
  showModal.value = true
}

function handleViewDetails(student: Student) {
  router.push(`/dashboard/students/${student.id}`)
}

function handleEdit(student: Student) {
  selectedStudent.value = student
  showModal.value = true
}

async function handleSubmit(studentData: Partial<Student>) {
  try {
    if (selectedStudent.value?.id) {
      await studentStore.updateStudent(selectedStudent.value.id, studentData)
    } else {
      await studentStore.createStudent(studentData)
    }
    showModal.value = false
  } catch (err) {
    console.error('Failed to save student:', err)
  }
}

function handleRowClick(student: TableItem) {
  handleViewDetails(student as Student)
}
</script>