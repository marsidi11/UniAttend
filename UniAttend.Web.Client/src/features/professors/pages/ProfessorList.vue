<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Professors</h1>
      <Button @click="openCreateModal">Add Professor</Button>
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

    <!-- Professors Table -->
    <div class="bg-white shadow rounded-lg">
      <DataTable
        :data="filteredProfessors"
        :columns="columns"
        :loading="isLoading"
        :actions="tableActions"
        @row-click="handleRowClick"
      />
    </div>

    <!-- Create/Edit Professor Modal -->
    <Modal v-model="showModal" :title="modalTitle">
      <ProfessorForm
        v-if="showModal"
        :professor="selectedProfessor"
        :departments="departments"
        @submit="handleSubmit"
        @cancel="showModal = false"
      />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useProfessorStore } from '@/stores/professor.store'
import { useDepartmentStore } from '@/stores/department.store'
import type { Professor } from '@/types/professor.types'
import Button from '@/shared/components/ui/Button.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import ProfessorForm from '../components/ProfessorForm.vue'
import type { TableItem } from '@/types/tableItem.types'

const professorStore = useProfessorStore()
const departmentStore = useDepartmentStore()

const { professors, isLoading } = storeToRefs(professorStore)
const { departments } = storeToRefs(departmentStore)

const showModal = ref(false)
const selectedProfessor = ref<Professor | null>(null)
const selectedDepartment = ref('')
const selectedStatus = ref('')

const columns = [
  { key: 'fullName', label: 'Full Name' },
  { key: 'departmentName', label: 'Department' },
  { key: 'email', label: 'Email' },
  { key: 'specialization', label: 'Specialization' },
  { key: 'isActive', label: 'Status',
    render: (value: boolean) => value ? 'Active' : 'Inactive'
  }
]

const tableActions = [
  { 
    label: 'Edit', 
    icon: 'edit', 
    action: (item: TableItem) => handleEdit(item as Professor)
  }
]

const modalTitle = computed(() => 
  selectedProfessor.value ? 'Edit Professor' : 'Add Professor'
)

const filteredProfessors = computed(() => {
  let filtered = [...professors.value]
  
  if (selectedDepartment.value) {
    filtered = filtered.filter(p => p.departmentId === Number(selectedDepartment.value))
  }
  
  if (selectedStatus.value !== '') {
    filtered = filtered.filter(p => p.isActive === (selectedStatus.value === 'true'))
  }
  
  return filtered
})

onMounted(async () => {
  await Promise.all([
    professorStore.fetchProfessors(),
    departmentStore.fetchDepartments()
  ])
})

function openCreateModal() {
  selectedProfessor.value = null
  showModal.value = true
}

function handleEdit(professor: Professor) {
  selectedProfessor.value = professor
  showModal.value = true
}

async function handleSubmit(professorData: Partial<Professor>) {
  try {
    if (selectedProfessor.value?.id) {
      await professorStore.updateProfessor(selectedProfessor.value.id, professorData)
    } else {
      await professorStore.createProfessor(professorData)
    }
    showModal.value = false
  } catch (err) {
    console.error('Failed to save professor:', err)
  }
}

function handleRowClick(professor: TableItem) {
  handleEdit(professor as Professor)
}
</script>