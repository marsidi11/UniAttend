<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Departments</h1>
      <Button v-if="isAdmin" @click="openCreateModal">Add Department</Button>
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
          <option value="true">Active</option>
          <option value="false">Inactive</option>
        </select>
      </div>
    </div>

    <!-- Departments Table -->
    <div class="bg-white shadow rounded-lg">
      <DataTable
        :data="filteredDepartments"
        :columns="columns"
        :loading="isLoading"
        :actions="tableActions"
        @row-click="handleRowClick"
      />
    </div>

    <!-- Create/Edit Department Modal -->
    <Modal v-model="showModal" :title="modalTitle">
      <DepartmentForm
        v-if="showModal"
        :department="selectedDepartment"
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
import { useDepartmentStore } from '@/stores/department.store'
import { useAuthStore } from '@/stores/auth.store'
import type { Department, CreateDepartmentRequest, UpdateDepartmentRequest } from '@/types/department.types'
import type { TableItem } from '@/types/tableItem.types'
import Button from '@/shared/components/ui/Button.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import DepartmentForm from '../components/DepartmentForm.vue'

const router = useRouter()
const departmentStore = useDepartmentStore()
const authStore = useAuthStore()

const { departments, isLoading } = storeToRefs(departmentStore)

const showModal = ref(false)
const selectedDepartment = ref<Department | null>(null)
const selectedStatus = ref('')

const isAdmin = computed(() => authStore.userRole === 'admin')

const columns = [
  { key: 'name', label: 'Name' },
  { key: 'subjectsCount', label: 'Subjects' },
  { key: 'studentsCount', label: 'Students' },
  { key: 'professorsCount', label: 'Professors' },
  { key: 'isActive', label: 'Status',
    render: (value: boolean) => value ? 'Active' : 'Inactive'
  }
]

const tableActions = computed(() => isAdmin.value ? [
  { 
    label: 'View Details', 
    icon: 'visibility',
    action: (item: TableItem) => handleViewDetails(item as Department)
  },
  { 
    label: 'Edit', 
    icon: 'edit',
    action: (item: TableItem) => handleEdit(item as Department)
  }
] : [])

const modalTitle = computed(() => 
  selectedDepartment.value ? 'Edit Department' : 'Add Department'
)

const filteredDepartments = computed(() => {
  let filtered = [...departments.value]
  
  if (selectedStatus.value !== '') {
    filtered = filtered.filter(d => d.isActive === (selectedStatus.value === 'true'))
  }
  
  return filtered
})

onMounted(async () => {
  await departmentStore.fetchDepartments()
})

function openCreateModal() {
  selectedDepartment.value = null
  showModal.value = true
}

function handleViewDetails(department: Department) {
  router.push(`/dashboard/departments/${department.id}`)
}

function handleEdit(department: Department) {
  selectedDepartment.value = department
  showModal.value = true
}

function handleRowClick(department: TableItem) {
  handleViewDetails(department as Department)
}

async function handleSubmit(departmentData: Partial<Department>) {
  try {
    if (selectedDepartment.value?.id) {
      const updateRequest: UpdateDepartmentRequest = {
        name: departmentData.name || '',
        isActive: departmentData.isActive ?? true
      }
      await departmentStore.updateDepartment(
        selectedDepartment.value.id, 
        updateRequest
      )
    } else {
      const createRequest: CreateDepartmentRequest = {
        name: departmentData.name || ''
      }
      await departmentStore.createDepartment(createRequest)
    }
    showModal.value = false
    // Refresh the departments list
    await departmentStore.fetchDepartments()
  } catch (err) {
    console.error('Failed to save department:', err)
  }
}
</script>