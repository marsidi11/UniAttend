<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Card Management</h1>
      <Button>Assign Card</Button>
    </div>

    <!-- Filters -->
    <div class="flex gap-4 bg-white p-4 rounded-lg shadow">
      <div class="w-64">
        <label class="block text-sm font-medium text-gray-700">Status</label>
        <select
          v-model="selectedStatus"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          <option value="">All</option>
          <option value="assigned">Assigned</option>
          <option value="unassigned">Unassigned</option>
        </select>
      </div>
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
    </div>

    <!-- Students Table -->
    <div class="bg-white shadow rounded-lg">
      <DataTable
        :data="filteredStudents"
        :columns="columns"
        :loading="isLoading"
        :actions="tableActions"
      />
    </div>

    <!-- Assign Card Modal -->
    <Modal v-model="showAssignModal" title="Assign Card">
      <CardAssignForm
        v-if="showAssignModal"
        :student="selectedStudent"
        :departments="departments"
        @submit="handleAssignCard"
        @cancel="showAssignModal = false"
      />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useStudentStore } from '@/stores/student.store'
import { useDepartmentStore } from '@/stores/department.store'
import type { Student } from '@/types/student.types'
import Button from '@/shared/components/ui/Button.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import CardAssignForm from '../components/CardAssignForm.vue'
import type { TableItem } from '@/types/tableItem.types'

const studentStore = useStudentStore()
const departmentStore = useDepartmentStore()

const { students, isLoading } = storeToRefs(studentStore)
const { departments } = storeToRefs(departmentStore)

const showAssignModal = ref(false)
const selectedStudent = ref<Student | null>(null)
const selectedStatus = ref('')
const selectedDepartment = ref('')

const columns = [
  { key: 'studentId', label: 'Student ID' },
  { key: 'fullName', label: 'Student Name' },
  { key: 'departmentName', label: 'Department' },
  { key: 'cardId', label: 'Card ID' },
  { key: 'isActive', label: 'Status',
    render: (value: boolean) => value ? 'Active' : 'Inactive'
  }
]

const tableActions = [
  { 
    label: 'Assign Card', 
    icon: 'add_card',
    show: (item: TableItem) => !(item as Student).cardId,
    action: (item: TableItem) => handleAssignCardClick(item as Student)
  },
  { 
    label: 'Remove Card', 
    icon: 'remove_card',
    show: (item: TableItem) => !!(item as Student).cardId,
    action: (item: TableItem) => handleRemoveCard(item as Student)
  }
]

const filteredStudents = computed(() => {
  let filtered = [...students.value]
  
  if (selectedStatus.value) {
    const hasCard = selectedStatus.value === 'assigned'
    filtered = filtered.filter(s => Boolean(s.cardId) === hasCard)
  }
  
  if (selectedDepartment.value) {
    filtered = filtered.filter(s => s.departmentId === Number(selectedDepartment.value))
  }
  
  return filtered
})

onMounted(async () => {
  await Promise.all([
    departmentStore.fetchDepartments()
  ])
})

function handleAssignCardClick(student: Student) {
  selectedStudent.value = student
  showAssignModal.value = true
}

async function handleAssignCard(data: { studentId: number, cardId: string }) {
  try {
    await studentStore.assignCard(data.studentId, data.cardId)
    showAssignModal.value = false
  } catch (err) {
    console.error('Failed to assign card:', err)
  }
}

async function handleRemoveCard(student: Student) {
  if (!student.id) return
  
  if (confirm('Are you sure you want to remove this card?')) {
    try {
      await studentStore.removeCard(student.id)
    } catch (err) {
      console.error('Failed to remove card:', err)
    }
  }
}
</script>